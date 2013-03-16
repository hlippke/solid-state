using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.State.Tests.TelephoneStates;
using Solid.State.Tests.TestStates;

namespace Solid.State.Tests
{
    [TestClass]
    public class StateMachineTests
    {
        // Private variables

        private enum TelephoneTrigger
        {
            PickingUpPhone,
            IncomingCall,
            DialedNumber,
            Answered,
            Timeout
        }

        private SolidMachine<TelephoneTrigger> BuildTelephoneStateMachine()
        {
            var sm = new SolidMachine<TelephoneTrigger>();

            sm.State<IdleState>()
                .IsInitialState()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>()
                .On(TelephoneTrigger.IncomingCall).GoesTo<RingingState>();

            sm.State<RingingState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<ConversationState>()
                .On(TelephoneTrigger.Timeout).GoesTo<IdleState>();

            sm.State<DiallingState>()
                .On(TelephoneTrigger.DialedNumber).GoesTo<WaitForAnswerState>();

            sm.State<WaitForAnswerState>()
                .On(TelephoneTrigger.Answered).GoesTo<ConversationState>()
                .On(TelephoneTrigger.Timeout).GoesTo<IdleState>();
                
            sm.Start();

            return sm;
        }

        /// <summary>
        /// Tests that a state cannot have multiple unguarded transitions with the
        /// same trigger.
        /// </summary>
        [TestMethod]
        public void ConfigureMultipleUnguardedTriggers()
        {
            var sm = new SolidMachine<TelephoneTrigger>();

            try
            {
                sm.State<IdleState>()
                    .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>()
                    .On(TelephoneTrigger.PickingUpPhone).GoesTo<RingingState>();

                Assert.Fail("Invalid configuration was permitted!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(ex is SolidStateException);
            }
        }

        [TestMethod]
        public void ConfigureUnguardedAndGuardedTrigger()
        {
            var sm = new SolidMachine<TelephoneTrigger>();

            try
            {
                sm.State<IdleState>()
                    .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>()
                    .On(TelephoneTrigger.PickingUpPhone, () => true).GoesTo<RingingState>();

                Assert.Fail("Invalid configuration was permitted!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(ex is SolidStateException);
            }
        }

        [TestMethod]
        public void ConfigureMultipleInitialStates()
        {
            try
            {
                var sm = new SolidMachine<TelephoneTrigger>();

                sm.State<IdleState>()
                    .IsInitialState()
                    .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>()
                    .On(TelephoneTrigger.IncomingCall).GoesTo<RingingState>();

                sm.State<RingingState>()
                    .IsInitialState()
                    .On(TelephoneTrigger.PickingUpPhone).GoesTo<ConversationState>()
                    .On(TelephoneTrigger.Timeout).GoesTo<IdleState>();

                Assert.Fail("Invalid configuration was permitted!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(ex is SolidStateException);
            }
        }

        [TestMethod]
        public void StartWithParameterizedState()
        {
            try
            {
                var sm = new SolidMachine<TelephoneTrigger>();
                sm.State<IdleState>()
                    .On(TelephoneTrigger.IncomingCall).GoesTo<StateWithoutParameterlessConstructor>();
                sm.Start();

                Assert.Fail("Startup without state resolver was permitted!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(ex is SolidStateException);
            }
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
                .On(TelephoneTrigger.Timeout).GoesTo<IdleState>();

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
                .On(TelephoneTrigger.Timeout).GoesTo<IdleState>();

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
        public void StartWithoutStates()
        {
            try
            {
                var sm = new SolidMachine<TelephoneTrigger>();
                sm.Start();

                Assert.Fail("Start without configured states permitted!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(ex is SolidStateException);
            }
        }

        [TestMethod]
        public void TriggerWithoutStarted()
        {
            try
            {
                var sm = new SolidMachine<TelephoneTrigger>();

                sm.State<IdleState>()
                    .IsInitialState()
                    .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>()
                    .On(TelephoneTrigger.IncomingCall).GoesTo<RingingState>();

                sm.Trigger(TelephoneTrigger.IncomingCall);

                Assert.Fail("Trigger on unstarted state machine succeeded!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(ex is SolidStateException);
            }
        }

        [TestMethod]
        public void InvalidTriggerSequence()
        {
            try
            {
                var sm = BuildTelephoneStateMachine();
                sm.Trigger(TelephoneTrigger.PickingUpPhone);
                sm.Trigger(TelephoneTrigger.PickingUpPhone);

                Assert.Fail("Invalid trigger sequence succeeded!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(ex is SolidStateException);
            }
        }
    }
}
