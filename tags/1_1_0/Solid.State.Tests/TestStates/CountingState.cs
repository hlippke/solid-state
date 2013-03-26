namespace Solid.State.Tests.TestStates
{
    public class CountingState : SolidState
    {
        // Methods (SolidState)

        protected override void DoEntering(object context)
        {
            // Increase the number on the state machines EnteringCount
            var machine = context as TestStateMachine;
            machine.EnteringCount++;

            // Also increase the number of times it has entered itself
            EnteringSelfCount++;
        }

        protected override void DoExiting(object context)
        {
            var machine = context as TestStateMachine;
            machine.ExitingCount++;
        }

        // Properties

        public int EnteringSelfCount { get; set; }
    }
}