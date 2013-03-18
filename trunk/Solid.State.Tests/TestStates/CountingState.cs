namespace Solid.State.Tests.TestStates
{
    public class CountingState : SolidState
    {
        public override void Entering(object context)
        {
            // Increase the number on the state machines EnteringCount
            var machine = context as TestStateMachine;
            machine.EnteringCount++;
        }

        public override void Exiting(object context)
        {
            var machine = context as TestStateMachine;
            machine.ExitingCount++;
        }
    }
}