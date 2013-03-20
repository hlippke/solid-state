using Solid.State;
using TelephoneSample.TelephoneParts;

namespace TelephoneSample.Telephone
{
    public class RingingState : TelephoneState
    {
        // Private variables

        private Bell _bell;

        // Constructor

        public RingingState(Bell bell)
        {
            _bell = bell;
        }

        protected override void DoEntering(SolidMachine<TelephoneTrigger> machine)
        {
            _bell.StartRinging();
        }

        protected override void DoExiting(SolidMachine<TelephoneTrigger> machine)
        {
            _bell.StopRinging();
        }
    }
}