using Solid.State;
using TelephoneSample.TelephoneParts;

namespace TelephoneSample.Telephone
{
    /// <summary>
    /// During dialling the phone speaker says BEEEEEEP.
    /// </summary>
    public class DiallingState : TelephoneState
    {
        // Private variables

        private Speaker _speaker;

        public DiallingState(Speaker speaker)
        {
            _speaker = speaker;
        }

        protected override void DoEntering(SolidMachine<TelephoneTrigger> machine)
        {
            _speaker.StartLongBeep();
        }

        protected override void DoExiting(SolidMachine<TelephoneTrigger> machine)
        {
            _speaker.StopLongBeep();
        }
    }
}