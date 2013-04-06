namespace Solid.State.Tests.Parallel
{
    public class ParallelMachine : SolidMachine<ParallelTrigger>
    {
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