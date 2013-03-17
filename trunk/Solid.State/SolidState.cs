namespace Solid.State
{
    /// <summary>
    /// A simple base class that can be used when creating state machine states. Since the interface is implemented
    /// with virtual methods in this class, subclasses can choose which methods to override.
    /// </summary>
    public abstract class SolidState : ISolidState
    {
        public virtual void Entering(object context)
        {
            // No code
        }

        public virtual void Exiting(object context)
        {
            // No code
        }
    }
}