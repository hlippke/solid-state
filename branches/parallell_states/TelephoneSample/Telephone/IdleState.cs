using Solid.State;

namespace TelephoneSample.Telephone
{
    public class IdleState : TelephoneState
    {
        // Private variables

        private ISimpleLog _log;

        // Constructor

        public IdleState(ISimpleLog log)
        {
            _log = log;
        }

        // Protected methods

        protected override void DoEntering(SolidMachine<TelephoneTrigger> machine)
        {
            // Log that we're here
            _log.Write("Telephone is idle\r\n");
        }
    }
}