namespace Solid.State.Tests.TestStates
{
    public class CountingState : SolidState
    {
        protected override void DoEntering(object context)
        {
            // Increase the number on the state machines EnteringCount
            var machine = context as TestStateMachine;
            machine.EnteringCount++;
        }

        protected override void DoExiting(object context)
        {
            var machine = context as TestStateMachine;
            machine.ExitingCount++;
        }
    }
}