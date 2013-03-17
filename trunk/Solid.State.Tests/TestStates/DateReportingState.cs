using System;

namespace Solid.State.Tests.TestStates
{
    /// <summary>
    /// A test state that reports the current date back to the state machine in its Entering method.
    /// </summary>
    public class DateReportingState : SolidState
    {
        public override void Entering(object context)
        {
            if (context is TestStateMachine)
            {
                var machine = context as TestStateMachine;
                machine.CurrentDate = DateTime.Now.Date;
            }
            else if (context is DateHolder)
            {
                var dateHolder = context as DateHolder;
                dateHolder.CurrentDate = DateTime.Now.Date;
            }
        }
    }

    public class DateHolder
    {
        public DateTime CurrentDate { get; set; }
    }
}