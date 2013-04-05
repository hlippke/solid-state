using Solid.State;
using TelephoneSample.TelephoneParts;

namespace TelephoneSample.Telephone
{
    /// <summary>
    /// When the line has disconnected because the other end hanged up,
    /// fast BEEPs are heard (same beeps as line busy, actually...)
    /// </summary>
    public class LineDisconnectedState : TelephoneState
    {
        // Private variables

        private readonly Speaker _speaker;

        // Constructor

        public LineDisconnectedState(Speaker speaker)
        {
            _speaker = speaker;
        }

        protected override void DoEntering(SolidMachine<TelephoneTrigger> machine)
        {
            _speaker.StartFastBeeps();
        }

        protected override void DoExiting(SolidMachine<TelephoneTrigger> machine)
        {
            _speaker.StopFastBeeps();
        }
    }
}