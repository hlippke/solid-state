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

        protected override void DoEntering(object context)
        {
            // Write the number to console
            Console.WriteLine("Entering {0} : number = {1}", this.GetType().Name, _number);
        }

        protected override void DoExiting(object context)
        {
            Console.WriteLine("Exiting {0}", this.GetType().Name);
        }
    }
}