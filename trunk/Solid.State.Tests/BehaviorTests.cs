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

        [TestMethod]
        public void OneStepUnguardedTransition()
        {
            var sm = BuildTelephoneStateMachine();
            Assert.IsTrue(sm.CurrentState is IdleState);

            sm.Trigger(TelephoneTrigger.PickingUpPhone);

            Assert.IsTrue(sm.CurrentState is DiallingState, "Unexpected current state!");
        }

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
    }
}