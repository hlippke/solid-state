using Solid.State;

namespace TelephoneSample.Telephone
{
    /// <summary>
    /// Abstract base class for states that belong to the TelephoneMachine. The context is cast
    /// to the correct type to simplify for inheriting states.
    /// </summary>
    public abstract class TelephoneState : ISolidState
    {
        // Protected methods

        protected virtual void DoEntering(SolidMachine<TelephoneTrigger> machine)
        {
            // No code
        }

        protected virtual void DoExiting(SolidMachine<TelephoneTrigger> machine)
        {
            // No code
        }

        // Methods

        public void Entering(object context)
        {
            // Redirect to protected methods and cast context
            DoEntering(context as SolidMachine<TelephoneTrigger>);
        }

        public void Exiting(object context)
        {
            // Redirect to protected methods and cast context
            DoExiting(context as SolidMachine<TelephoneTrigger>);
        }
    }
}