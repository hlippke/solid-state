using System;

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

        // Methods

        public override void Entering(object context)
        {
            // Write the number to console
            Console.WriteLine("Entering {0} : number = {1}", this.GetType().Name, _number);
        }

        public override void Exiting(object context)
        {
            Console.WriteLine("Exiting {0}", this.GetType().Name);
        }
    }
}