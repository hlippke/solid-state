using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.State.Tests.TelephoneStates;

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
    }
}
