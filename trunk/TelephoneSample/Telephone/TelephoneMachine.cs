using Solid.State;

namespace TelephoneSample.Telephone
{
    /// <summary>
    /// A specialized state machine representing a telephone. This state machine uses an int as the trigger,
    /// which means that constants can be used as triggers.
    /// </summary>
    public class TelephoneMachine : SolidMachine<int>
    {
    }
}