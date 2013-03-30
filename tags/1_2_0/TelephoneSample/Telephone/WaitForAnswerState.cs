using Solid.State;
using TelephoneSample.TelephoneParts;

namespace TelephoneSample.Telephone
{
    /// <summary>
    /// When we're waiting for an answer a periodic BEEP is heard on the speaker.
    /// </summary>
    public class WaitForAnswerState : TelephoneState
    {
        // Private variables

        private Speaker _speaker;

        // Constructor

        public WaitForAnswerState(Speaker speaker)
        {
            _speaker = speaker;
        }

        protected override void DoEntering(SolidMachine<TelephoneTrigger> machine)
        {
            _speaker.StartWaitForAnswerBeep();
        }

        protected override void DoExiting(SolidMachine<TelephoneTrigger> machine)
        {
            _speaker.StopWaitForAnswerBeep();
        }
    }
}