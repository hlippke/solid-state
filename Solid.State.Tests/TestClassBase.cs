using Solid.State.Tests.TelephoneStates;

namespace Solid.State.Tests
{
    /// <summary>
    /// Contains base test functionality for SolidMachine unit tests.
    /// </summary>
    public class TestClassBase
    {
        protected enum TelephoneTrigger
        {
            PickingUpPhone,
            RefusingToAnswer,
            IncomingCall,
            DialedNumber,
            Answered,
            HangingUp
        }

        protected SolidMachine<TelephoneTrigger> BuildTelephoneStateMachine()
        {
            var sm = new SolidMachine<TelephoneTrigger>();

            sm.State<IdleState>()
                .IsInitialState()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<DiallingState>()
                .On(TelephoneTrigger.IncomingCall).GoesTo<RingingState>();

            sm.State<RingingState>()
                .On(TelephoneTrigger.PickingUpPhone).GoesTo<ConversationState>()
                .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

            sm.State<DiallingState>()
                .On(TelephoneTrigger.DialedNumber).GoesTo<WaitForAnswerState>();

            sm.State<WaitForAnswerState>()
                .On(TelephoneTrigger.Answered).GoesTo<ConversationState>()
                .On(TelephoneTrigger.RefusingToAnswer).GoesTo<IdleState>();

            sm.State<ConversationState>()
                .On(TelephoneTrigger.HangingUp).GoesTo<IdleState>();

            sm.Start();

            return sm;
        }
    }
}