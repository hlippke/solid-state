using TelephoneSample.TelephoneParts;

namespace TelephoneSample.Telephone
{
    /// <summary>
    /// When the line is busy, the speaker emits fast BEEPS.
    /// </summary>
    public class LineBusyState : TelephoneState
    {
        // Private variables

        private Speaker _speaker;

        // Constructor

        public LineBusyState(Speaker speaker)
        {
            _speaker = speaker;
        }

        protected override void DoEntering(Solid.State.SolidMachine<TelephoneTrigger> machine)
        {
            _speaker.StartFastBeeps();
        }

        protected override void DoExiting(Solid.State.SolidMachine<TelephoneTrigger> machine)
        {
            _speaker.StopFastBeeps();
        }

    }
}