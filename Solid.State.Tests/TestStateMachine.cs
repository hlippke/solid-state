using System;

namespace Solid.State.Tests
{
    public class TestStateMachine : SolidMachine<TelephoneTrigger>
    {
        // Properties

        public DateTime CurrentDate { get; set; }
    }
}