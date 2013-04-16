using System.Collections.Generic;
using Solid.State.Tests.TestStates;

namespace Solid.State.Tests.Parallel
{
    public class ParallelMachine : SolidMachine<ParallelTrigger>
    {
        private List<string> _log;
        
        // Constructor

        public ParallelMachine()
        {
            _log = new List<string>();

            State<ParaState1>()
                .On(ParallelTrigger.State1Ended).GoesTo<ParaState2>();

            State<ParaState2>()
                .On(ParallelTrigger.State2Ended).ForksTo<ParaState3_1, ParaState3_2>()
                .On(ParallelTrigger.State2EndedDetour).GoesTo<ParaState3_3>();

            State<ParaState3_1>()
                .On(ParallelTrigger.State3_1Ended).GoesTo<ParaState4_1>();

            State<ParaState3_2>()
                .On(ParallelTrigger.State3_2Ended).GoesTo<ParaState4_2>();

            State<ParaState3_3>()
                .On(ParallelTrigger.State3_3Ended).GoesTo<ParaState6>();

            State<ParaState4_1>()
                .On(ParallelTrigger.State4_1Ended).GoesTo<ParaState5_1>();

            State<ParaState4_2>()
                .On(ParallelTrigger.State4_2Ended).ForksTo<ParaState5_2_1, ParaState5_2_2>();

            State<ParaState5_1>()
                .On(ParallelTrigger.State5_1Ended).JoinsTo<ParaState6>();

            State<ParaState5_2_1>()
                .On(ParallelTrigger.State5_2_1Ended).JoinsTo<ParaState6>();

            State<ParaState5_2_2>()
                .On(ParallelTrigger.State5_2_2Ended).JoinsTo<ParaState6>();

        }

        // Properties

        public List<string> Log
        {
            get { return _log; }
        }
    }

    public enum ParallelTrigger
    {
        State1Ended,
        State2Ended,
        State2EndedDetour,
        State3_1Ended,
        State3_2Ended,
        State3_3Ended,
        State4_1Ended,
        State4_2Ended,
        State5_1Ended,
        State5_2_1Ended,
        State5_2_2Ended
    }
}