using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.State.Tests.Parallel;

namespace Solid.State.Tests.TestStates
{
    public abstract class ParaState : ISolidState
    {
        // Private variables

        private Timer _timer;

        /// <summary>
        /// Must be overridden to provide a trigger that should be fired to exit this state.
        /// </summary>
        protected abstract ParallelTrigger GetExitTrigger();

        /// <summary>
        /// Must be overridden to provide a duration that the state should last.
        /// </summary>
        /// <returns></returns>
        protected abstract int GetDuration();

        // Methods (ISolidState)

        public void Entering(object context)
        {
            var machine = context as ParallelMachine;
            if (machine != null)
                machine.Log.Add(string.Format("Entering {0}", this.GetType().Name));

            _timer = new Timer((callbackState) =>
                {
                    (context as SolidMachine<ParallelTrigger>).Trigger(GetExitTrigger());
                }, null, GetDuration(), Timeout.Infinite);
        }

        public void Exiting(object context)
        {
            _timer.Dispose();
        }
    }

    public class ParaState1 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State1Ended;
        }

        protected override int GetDuration()
        {
            return 100;
        }
    }

    public class ParaState2 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State2Ended;
        }

        protected override int GetDuration()
        {
            return 100;
        }
    }

    public class ParaState3_1 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State3_1Ended;
        }

        protected override int GetDuration()
        {
            return 300;
        }
    }

    public class ParaState3_2 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State3_2Ended;
        }

        protected override int GetDuration()
        {
            return 200;
        }
    }

    public class ParaState3_3 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State3_3Ended;
        }

        protected override int GetDuration()
        {
            return 100;
        }
    }

    public class ParaState4_1 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State4_1Ended;
        }

        protected override int GetDuration()
        {
            return 100;
        }
    }

    public class ParaState4_2 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State4_2Ended;
        }

        protected override int GetDuration()
        {
            return 300;
        }
    }

    public class ParaState5_1 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State5_1Ended;
        }

        protected override int GetDuration()
        {
            return 300;
        }
    }

    public class ParaState5_2_1 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State5_2_1Ended;
        }

        protected override int GetDuration()
        {
            return 300;
        }
    }

    public class ParaState5_2_2 : ParaState
    {
        protected override ParallelTrigger GetExitTrigger()
        {
            return ParallelTrigger.State5_2_2Ended;
        }

        protected override int GetDuration()
        {
            return 100;
        }
    }

    public class ParaState6 : ISolidState
    {
        public void Entering(object context)
        {
            var machine = context as ParallelMachine;
        }

        public void Exiting(object context)
        {
        }
    }
}