using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.State.Tests.TestStates;

namespace Solid.State.Tests
{
    public class ParallelTests
    {
        [TestMethod]
        public void ParallelStates()
        {
            var sm = new SolidMachine<ParallelTrigger>();

            sm.State<ParaState1>()
                .On(ParallelTrigger.State1Ended).GoesTo<ParaState2>();

            sm.State<ParaState2>()
                .On(ParallelTrigger.State2Ended).ForksTo<ParaState3_1>().ForksTo<ParaState3_2>();

            sm.State<ParaState3_1>()
                .On(ParallelTrigger.State3_1Ended).GoesTo<ParaState4_1>();

            sm.State<ParaState3_2>()
                .On(ParallelTrigger.State3_2Ended).GoesTo<ParaState4_2>();

            sm.State<ParaState4_1>()
                .On(ParallelTrigger.State4_1Ended).GoesTo<ParaState5_1>();

            sm.State<ParaState4_2>()
                .On(ParallelTrigger.State4_2Ended).ForksTo<ParaState5_2_1>().ForksTo<ParaState5_2_2>();

            sm.State<ParaState5_1>()
                .On(ParallelTrigger.State5_1Ended).JoinsTo<ParaState6>();

            sm.State<ParaState5_2_1>()
                .On(ParallelTrigger.State5_2_1Ended).JoinsTo<ParaState6>();

            sm.State<ParaState5_2_2>()
                .On(ParallelTrigger.State5_2_2Ended).JoinsTo<ParaState6>();
        }
 
    }
}