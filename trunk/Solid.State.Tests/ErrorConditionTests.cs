using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.State.Tests.TelephoneStates;
using Solid.State.Tests.TestStates;

namespace Solid.State.Tests
{
    /// <summary>
    /// Contains unit tests where the correct behavior involves an exception being thrown.
    /// </summary>
    [TestClass]
    public class ErrorConditionTests : TestClassBase
    {
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
                    .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

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

                Assert.IsTrue(sm.CurrentState is DiallingState, "State expected to be DiallingState!");

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