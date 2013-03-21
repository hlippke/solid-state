namespace Solid.State
{
    /// <summary>
    /// A simple base class that can be used when creating state machine states. Since the interface is implemented
    /// with virtual methods in this class, subclasses can choose which methods to override.
    /// </summary>
    public abstract class SolidState : ISolidState
    {
        // Protected methods

        protected virtual void DoEntering(object context)
        {
            // No code
        }

        protected virtual void DoExiting(object context)
        {
            // No code
        }

        public void Entering(object context)
        {
            DoEntering(context);
        }

        public void Exiting(object context)
        {
            DoExiting(context);
        }
    }
}