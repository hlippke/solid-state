using System;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Solid.State.Tests.TestStates;

namespace Solid.State.Tests.Parallel
{
    [TestClass]
    public class ParallelTests
    {
        // Test methods

        [TestMethod]
        public void TwoCurrentStates()
        {
            var sm = new SolidMachine<ParallelTrigger>();
            sm.State<ParaState1>()
                .On(ParallelTrigger.State1Ended).ForksTo<ParaState3_1, ParaState3_2>();
            sm.Start();

            sm.Trigger(ParallelTrigger.State1Ended);
            Assert.IsTrue(sm.IsInState<ParaState3_1>() && sm.IsInState<ParaState3_2>(), "Not in both states!");
            Assert.IsTrue(sm.CurrentStates.Length == 2, "CurrentStates.Length != 2");
        }

        [TestMethod]
        public void WaitForAllJoins()
        {
            var sm = new SolidMachine<ParallelTrigger>();
            sm.State<ParaState1>()
                .On(ParallelTrigger.State1Ended).ForksTo<ParaState3_1, ParaState3_2>();

            sm.State<ParaState3_1>()
                .On(ParallelTrigger.State3_1Ended).JoinsTo<ParaState6>();
            sm.State<ParaState3_2>()
                .On(ParallelTrigger.State3_2Ended).JoinsTo<ParaState6>();

            sm.Start();

            sm.Trigger(ParallelTrigger.State1Ended);
            Assert.IsTrue(sm.IsInState<ParaState3_1>() && sm.IsInState<ParaState3_2>(), "Not in both states!");

            sm.Trigger(ParallelTrigger.State3_1Ended);

            Assert.IsTrue(!sm.IsInState<ParaState6>(), "Is in ParaState6 before all paths have completed!");

            sm.Trigger(ParallelTrigger.State3_2Ended);

            Assert.IsTrue(sm.IsInState<ParaState6>(), "Is NOT in ParaState6 after paths have converged!");
        }

        /// <summary>
        /// Makes sure that an exception is thrown if a trigger matches none of the current states.
        /// </summary>
        [TestMethod]
        public void InvalidParallelTrigger()
        {
            var sm = new SolidMachine<ParallelTrigger>();
            sm.State<ParaState1>()
                .On(ParallelTrigger.State1Ended).ForksTo<ParaState3_1, ParaState3_2>();

            sm.State<ParaState3_1>()
                .On(ParallelTrigger.State3_1Ended).JoinsTo<ParaState6>();
            sm.State<ParaState3_2>()
                .On(ParallelTrigger.State3_2Ended).JoinsTo<ParaState6>();

            sm.Start();

            // OK trigger
            sm.Trigger(ParallelTrigger.State1Ended);

            try
            {
                sm.Trigger(ParallelTrigger.State5_2_2Ended);
                Assert.Fail("Accepted invalid trigger!");
            }
            catch (Exception ex)
            {
                Assert.IsTrue(ex is SolidStateException, string.Format("Unexpected exception: {0}", ex));
            }
        }

        /// <summary>
        /// Tests that it's not OK to transition between states that are on different paths.
        /// </summary>
        [TestMethod]
        public void TransitionBetweenPaths()
        {
            try
            {
                var sm = new ParallelMachine();

                sm.State<ParaState1>()
                    .On(ParallelTrigger.State1Ended).GoesTo<ParaState2>();

                sm.State<ParaState2>()
                    .On(ParallelTrigger.State2Ended).ForksTo<ParaState3_1, ParaState3_2>()
                    .On(ParallelTrigger.State2EndedDetour).GoesTo<ParaState3_3>();

                sm.State<ParaState3_1>()
                    .On(ParallelTrigger.State3_1Ended).GoesTo<ParaState4_1>();

                sm.State<ParaState3_2>()
                    .On(ParallelTrigger.State3_2Ended).GoesTo<ParaState3_1>();

                Assert.Fail("Configuration of cross-path transition succeeded!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(ex is SolidStateException, string.Format("Unexpected exception: {0}", ex));
            }
        }

        [TestMethod]
        public void CurrentStateUnavailableWhenParallelStates()
        {
            try
            {
                var sm = new ParallelMachine();
                sm.Start();

                sm.Trigger(ParallelTrigger.State1Ended);
                sm.Trigger(ParallelTrigger.State2Ended);

                Console.WriteLine("CurrentState : {0}", sm.CurrentState);

                Assert.Fail("Could use CurrentState property despite of parallel states!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Assert.IsTrue(ex is SolidStateException, string.Format("Unexpected exception: {0}", ex));
            }
            
        }

        [TestMethod]
        public void FullParallelState()
        {
            var machine = new ParallelMachine();

            machine.Start();

            // Wait for it to reach final state (with a timeout)
            var tick = Environment.TickCount;
            while (!machine.IsInState<ParaState6>() && (Environment.TickCount < (tick + 1500))) ;

            // According to the timing of all the states, the following states should be on the same log position always:
            Assert.IsTrue(machine.Log.Count >= 7, "Not enough log entries, timeout?");
            Assert.IsTrue(machine.Log[4].Contains(typeof (ParaState4_2).Name),
                          string.Format("State {0} not in expected position", typeof (ParaState4_2).Name));
            Assert.IsTrue(machine.Log[5].Contains(typeof(ParaState4_1).Name),
                          string.Format("State {0} not in expected position", typeof(ParaState4_1).Name));
            Assert.IsTrue(machine.Log[6].Contains(typeof(ParaState5_1).Name),
                          string.Format("State {0} not in expected position", typeof(ParaState5_1).Name));
        }
    }
}