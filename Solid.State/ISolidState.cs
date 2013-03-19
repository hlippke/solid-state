namespace Solid.State
{
    /// <summary>
    /// Represents a state implementation for the SolidMachine state machine.
    /// </summary>
    public interface ISolidState
    {
        // Methods

        void Entering(object context);

        void Exiting(object context);
    }
}