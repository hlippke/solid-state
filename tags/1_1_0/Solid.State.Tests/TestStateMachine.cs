using System;

namespace Solid.State.Tests
{
    public class TestStateMachine : SolidMachine<TelephoneTrigger>
    {
        // Properties

        public int EnteringCount { get; set; }

        public int ExitingCount { get; set; }

        public DateTime CurrentDate { get; set; }
    }
}