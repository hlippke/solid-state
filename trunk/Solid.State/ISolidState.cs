namespace Solid.State
{
    /// <summary>
    /// Represents a state implementation for the SolidMachine state machine.
    /// </summary>
    public interface ISolidState
    {
        void EnteringState();

        void ExitingState();
    }
}