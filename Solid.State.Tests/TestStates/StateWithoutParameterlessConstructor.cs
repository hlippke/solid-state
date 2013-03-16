namespace Solid.State.Tests.TestStates
{
    public class StateWithoutParameterlessConstructor : SolidState
    {
        private int _number;

        // Constructor

        public StateWithoutParameterlessConstructor(int number)
        {
            _number = number;
        }
    }
}