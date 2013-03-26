using System;

namespace Solid.State.Tests.TestStates
{
    public abstract class StepStateBase : ISolidState
    {
        // Protected methods

        protected virtual void DoEntering(SolidMachine<int> machine)
        {
            // Just fire the trigger based on the type name (e.g. StepState1 -> fire trigger 1)
            var typeName = GetType().Name;
            var trigger = Convert.ToInt32(typeName.Substring(typeName.Length - 1, 1));

            if (trigger < 4)
                machine.Trigger(trigger);
        }

        protected virtual void DoExiting(SolidMachine<int> machine)
        {
        }

        // Methods (ISolidState)

        public void Entering(object context)
        {
            DoEntering(context as SolidMachine<int>);
        }

        public void Exiting(object context)
        {
            DoExiting(context as SolidMachine<int>);
        }
    }

    public class StepState1 : StepStateBase
    {
    }

    public class StepState2 : StepStateBase
    {
    }
    
    public class StepState3 : StepStateBase
    {
    }

    public class StepState4 : StepStateBase
    {
    }

}