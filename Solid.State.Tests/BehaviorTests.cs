using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.State.Tests.TelephoneStates;
using Solid.State.Tests.TestStates;

namespace Solid.State.Tests
{
    [TestClass]
    public class BehaviorTests : TestClassBase
    {
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
                          string.Format("Telephone state is {0}, expected {1}", sm.CurrentState.GetType().Name, typeof(TelephoneBrokenState).Name));
            
            // Reset the machine to IdleState
            sm.Trigger(TelephoneTrigger.HangingUp);

            isPhoneWorking = true;

            sm.Trigger(TelephoneTrigger.PickingUpPhone);
            Assert.IsTrue(sm.CurrentState is DiallingState,
                          string.Format("Telephone state is {0}, expected {1}", sm.CurrentState.GetType().Name, typeof(DiallingState).Name));
        }

    }
}
