using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.State.Tests.TelephoneStates;
using Solid.State.Tests.TestStates;

namespace Solid.State.Tests
{
    [TestClass]
    public class BehaviorTests : TestClassBase, IStateResolver
    {
        // Methods (IStateResolver)

        /// <summary>
        /// Implements the IStateResolver interface which is used by
        /// some of the tests.
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        public ISolidState ResolveState(Type stateType)
        {
            if (stateType == typeof (StateWithoutParameterlessConstructor))
                return new StateWithoutParameterlessConstructor(666);
            else
                return (ISolidState) Activator.CreateInstance(stateType);
        }

        /// <summary>
        /// Tests that the first configured state becomes the initial state by default.
        /// </summary>
        [TestMethod]
        public void CorrectImplicitInitialState()
        {
            var sm = new SolidMachine<TelephoneTrigger>();

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>()
                .On(TelephoneTrigger.IncomingCall).GoesTo<RingingState>();

            sm.State<RingingState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<ConversationState>()
                .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

            sm.Start();

            Assert.IsTrue(sm.CurrentState is IdleState);
        }

        /// <summary>
        /// Tests that the state that is marked as initial state also becomes it.
        /// </summary>
        [TestMethod]
        public void CorrectExplicitInitialState()
        {
            var sm = new SolidMachine<TelephoneTrigger>();

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>()
                .On(TelephoneTrigger.IncomingCall).GoesTo<RingingState>();

            sm.State<RingingState>()
                .IsInitialState()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<ConversationState>()
                .On(TelephoneTrigger.NotAnswering).GoesTo<IdleState>();

            sm.Start();

            Assert.IsTrue(sm.CurrentState is RingingState);
        }

        /// <summary>
        /// Tests that an unguarded trigger leads to the correct target state.
        /// </summary>
        [TestMethod]
        public void OneStepUnguardedTransition()
        {
            var sm = BuildTelephoneStateMachine();
            Assert.IsTrue(sm.CurrentState is IdleState);

            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            Assert.IsTrue(sm.CurrentState is DiallingState, "Unexpected current state!");
        }

        /// <summary>
        /// Tests that the correct target state is selected when the same trigger have different guard clauses.
        /// </summary>
        [TestMethod]
        public void OneStepGuardedTransition()
        {
            var isPhoneWorking = false;

            var sm = new SolidMachine<TelephoneTrigger>();
            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone, () => isPhoneWorking).GoesTo<DiallingState>()
                .On(TelephoneTrigger.PickingUpPhone, () => !isPhoneWorking).GoesTo<TelephoneBrokenState>()
                .On(TelephoneTrigger.IncomingCall).GoesTo<RingingState>();

            sm.State<RingingState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<ConversationState>()
                .On(TelephoneTrigger.NotAnswering).GoesTo<IdleState>();

            sm.State<TelephoneBrokenState>()
                .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

            sm.Start();

            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            Assert.IsTrue(sm.CurrentState is TelephoneBrokenState,
                          string.Format("Telephone state is {0}, expected {1}", sm.CurrentState.GetType().Name,
                                        typeof (TelephoneBrokenState).Name));

            // Reset the machine to IdleState
            sm.Trigger(TelephoneTrigger.HangingUp);

            isPhoneWorking = true;

            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            Assert.IsTrue(sm.CurrentState is DiallingState,
                          string.Format("Telephone state is {0}, expected {1}", sm.CurrentState.GetType().Name,
                                        typeof (DiallingState).Name));
        }

        /// <summary>
        /// Tests that states are correctly instantiated when there is a state resolver present.
        /// (This class acts as the state resolver).
        /// </summary>
        [TestMethod]
        public void UsingStateResolver()
        {
            // Lets this test class act as the state resolver...
            var sm = new SolidMachine<TelephoneTrigger>(null, this);

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<StateWithoutParameterlessConstructor>();

            sm.Start();

            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            Assert.IsTrue(sm.CurrentState is StateWithoutParameterlessConstructor);
        }

        /// <summary>
        /// Tests that the state machine instance is used as state context if no explicit context is defined.
        /// </summary>
        [TestMethod]
        public void UsingImplicitContext()
        {
            var sm = new TestStateMachine();
            sm.CurrentDate = DateTime.MinValue.Date;

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<DateReportingState>();
            sm.Start();

            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            // The state should've been able to use the default context (the state machine) to set its CurrentDate property
            Assert.IsTrue(sm.CurrentDate.Equals(DateTime.Now.Date), "Wrong date in state machine!");
        }

        /// <summary>
        /// Tests that the defined state machine context reaches the state in the Entering method.
        /// </summary>
        [TestMethod]
        public void UsingExplicitContext()
        {
            var dateTime = new DateHolder();

            var sm = new SolidMachine<TelephoneTrigger>(dateTime);

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<DateReportingState>();
            sm.Start();

            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            // The state should be able to use the specified context (dateTime variable) to report the date.
            Assert.IsTrue(dateTime.CurrentDate.Equals(DateTime.Now.Date), "Wrong date in context!");
        }

        /// <summary>
        /// Tests that it's OK to configure reentrant states by pointing a transition back to the same state it belongs to.
        /// </summary>
        [TestMethod]
        public void ConfigurationReentrantState()
        {
            var sm = new TestStateMachine();

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<CountingState>();

            sm.State<CountingState>()
                .On(TelephoneTrigger.DialedNumber).GoesTo<CountingState>()
                .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

            sm.Start();

            // Goto CountingState, EnteringCount should be 1
            sm.Trigger(TelephoneTrigger.PickingUpPhone);
            Assert.IsTrue(sm.EnteringCount == 1, "EnteringCount != 1");

            // Reentry
            sm.Trigger(TelephoneTrigger.DialedNumber);
            Assert.IsTrue((sm.EnteringCount == 2) && (sm.ExitingCount == 1), "EnteringCount != 2 || ExitingCount != 1");

            // Back to Idle
            sm.Trigger(TelephoneTrigger.HangingUp);
            Assert.IsTrue((sm.EnteringCount == 2) && (sm.ExitingCount == 2), "EnteringCount != 2 || ExitingCount != 2");
        }

        /// <summary>
        /// Tests that transitions are performed in the correct order even if a trigger is fired inside a state's
        /// Entering method.
        /// </summary>
        [TestMethod]
        public void CorrectOrderOnTransitions()
        {
            var states = new List<string>();

            var sm = new SolidMachine<int>();
            sm.Transitioned += (sender, args) =>
                {
                    var stateName = args.TargetState.Name;
                    Console.WriteLine("Transitioned to " + stateName);

                    states.Add(stateName);
                    if (states.Count == 5)
                    {
                        var stateOrder = string.Join("_", states);
                        Assert.IsTrue(stateOrder == "IdleState_StepState1_StepState2_StepState3_StepState4",
                                      string.Format("Wrong state order: {0}", stateOrder));
                    }

                };

            sm.State<IdleState>()
                .On(0).GoesTo<StepState1>();
            sm.State<StepState1>()
                .On(1).GoesTo<StepState2>();
            sm.State<StepState2>()
                .On(2).GoesTo<StepState3>();
            sm.State<StepState3>()
                .On(3).GoesTo<StepState4>();
            sm.Start();

            sm.Trigger(0);
        }
        
        /// <summary>
        /// Tests that the invalid trigger handler is called when it has been specified,
        /// instead of an exception being thrown.
        /// </summary>
        [TestMethod]
        public void InvalidTriggerWithHandler()
        {
            var _handlerCalledMessage = "";

            var sm = new SolidMachine<TelephoneTrigger>();
            sm.OnInvalidTrigger((state, trigger) =>
                { _handlerCalledMessage = string.Format("{0}_{1}", state.Name, trigger); });

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>();

            sm.State<DiallingState>()
                .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

            sm.Start();

            sm.Trigger(TelephoneTrigger.PickingUpPhone);
            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            Assert.IsTrue(_handlerCalledMessage ==
                          string.Format("{0}_{1}", typeof (DiallingState).Name, TelephoneTrigger.PickingUpPhone),
                          string.Format("HandlerCalledMessage not what expected : {0}", _handlerCalledMessage));
        }

        /// <summary>
        /// Tests that the same state instance is used all the time if the state machine is configured to do so.
        /// </summary>
        [TestMethod]
        public void VerifyStateSingletons()
        {
            var sm = new TestStateMachine();
            // No setting of the value here, Singleton should be the default.

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<CountingState>();
            sm.State<CountingState>()
                .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

            sm.Start();

            // Goto CountingState
            sm.Trigger(TelephoneTrigger.PickingUpPhone);
            // Go back
            sm.Trigger(TelephoneTrigger.HangingUp);
            // Goto CountingState again
            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            // The CurrentState should have been freshly created, meaning that CountingState.EnteringSelfCount should be 1
            Assert.IsTrue(
                (sm.CurrentState is CountingState) && ((sm.CurrentState as CountingState).EnteringSelfCount == 2),
                string.Format("Unexpected EnteringSelfCount!"));

        }

        /// <summary>
        /// Tests that new target state instances are created on each transition if the state machine has been
        /// configured to do so.
        /// </summary>
        [TestMethod]
        public void InstantiateStatePerTransition()
        {
            var sm = new TestStateMachine();
            sm.StateInstantiationMode = StateInstantiationMode.PerTransition;

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<CountingState>();
            sm.State<CountingState>()
                .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

            sm.Start();

            // Goto CountingState
            sm.Trigger(TelephoneTrigger.PickingUpPhone);
            // Go back
            sm.Trigger(TelephoneTrigger.HangingUp);
            // Goto CountingState again
            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            // The CurrentState should have been freshly created, meaning that CountingState.EnteringSelfCount should be 1
            Assert.IsTrue(
                (sm.CurrentState is CountingState) && ((sm.CurrentState as CountingState).EnteringSelfCount == 1),
                string.Format("Unexpected EnteringSelfCount!"));

        }

        /// <summary>
        /// Tests that states are recorded correctly in the state history in the correct order.
        /// </summary>
        [TestMethod]
        public void StateHistory()
        {
            var sm = BuildTelephoneStateMachine();

            // State history should be empty
            Assert.IsTrue(sm.StateHistory.Length == 0, "State history is not empty!");
            
            sm.Trigger(TelephoneTrigger.PickingUpPhone);
            sm.Trigger(TelephoneTrigger.DialedNumber);
            sm.Trigger(TelephoneTrigger.Answered);

            // Assert
            Assert.IsTrue(sm.StateHistory.Length == 3, "State history doesn't contain 3 items");
            Assert.IsTrue(sm.StateHistory[0] == typeof(WaitForAnswerState), "Expected WaitForAnswerState at index 0");
            Assert.IsTrue(sm.StateHistory[1] == typeof(DiallingState), "Expected DiallingState at index 1");
            Assert.IsTrue(sm.StateHistory[2] == typeof (IdleState), "Expected IdleState at index 2");
        }

        /// <summary>
        /// Tests that the GoBack method works, even when guard clauses has caused the
        /// state machine to transition to different states from a specific source state.
        /// </summary>
        [TestMethod]
        public void GoBack()
        {
            var sm = new SolidMachine<TelephoneTrigger>();

            var isGoingToDialling = false;

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>();

            sm.State<DiallingState>()
                .On(TelephoneTrigger.Answered, () => isGoingToDialling).GoesTo<ConversationState>()
                .On(TelephoneTrigger.Answered, () => !isGoingToDialling).GoesTo<WaitForAnswerState>();

            sm.Start();

            sm.Trigger(TelephoneTrigger.PickingUpPhone);
            sm.Trigger(TelephoneTrigger.Answered);

            Assert.IsTrue(sm.CurrentState is WaitForAnswerState, "Expected state WaitForAnswerState");
            sm.GoBack();
            Assert.IsTrue(sm.CurrentState is DiallingState,
                          string.Format("Expected state DiallingState, was {0}", sm.CurrentState.GetType().Name));
            
            // Shift the track
            isGoingToDialling = true;
            sm.Trigger(TelephoneTrigger.Answered);

            Assert.IsTrue(sm.CurrentState is ConversationState, "Expected state ConversationState");
            sm.GoBack();
            Assert.IsTrue(sm.CurrentState is DiallingState, "Expected state DiallingState... again");

        }

        /// <summary>
        /// Tests that the Stop method makes a final call to the current state's Exit method to 
        /// allow it to do cleanup etc.
        /// </summary>
        [TestMethod]
        public void StopCallsExit()
        {
            var sm = new TestStateMachine();

            sm.State<IdleState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<CountingState>();
            sm.State<CountingState>()
                .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

            sm.Start();

            sm.Trigger(TelephoneTrigger.PickingUpPhone);
            sm.Trigger(TelephoneTrigger.HangingUp);
            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            Assert.IsTrue((sm.EnteringCount == 2) && (sm.ExitingCount == 1), "Unexpected EnteringCount / ExitingCount!");

            sm.Stop();

            Assert.IsTrue((sm.EnteringCount == 2) && (sm.ExitingCount == 2), "Unexpected EnteringCount / ExitingCount on Stop!");
        }
    }
}