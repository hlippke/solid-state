using System;

namespace Solid.State
{
    public partial class SolidMachine<TTrigger>
    {
        public class TriggerConfiguration
        {
            // Private variables

            private readonly StateConfiguration _owningStateConfiguration;
            private readonly TTrigger _trigger;
            
            private StateConfiguration _targetState;
            private Func<bool> _guardClause; 

            // Constructor

            public TriggerConfiguration(TTrigger trigger, StateConfiguration owningStateConfiguration)
            {
                _trigger = trigger;
                _owningStateConfiguration = owningStateConfiguration;
            }

            public TriggerConfiguration(TTrigger trigger, Func<bool> guardClause, StateConfiguration owningStateConfiguration)
            {
                _guardClause = guardClause;
                _trigger = trigger;
                _owningStateConfiguration = owningStateConfiguration;
            }

            // Methods

            public StateConfiguration GoesTo<TTargetState>() where TTargetState : SolidState
            {
                _targetState = _owningStateConfiguration.OwningMachine.State<TTargetState>();

                // Return the correct StateConfiguration
                var machine = _owningStateConfiguration.OwningMachine;
                return machine.StateConfigurations[_owningStateConfiguration.StateType];
            }

            // Properties

            internal TTrigger Trigger
            {
                get { return _trigger; }
            }

            internal StateConfiguration TargetState
            {
                get { return _targetState; }
            }

            internal Func<bool> GuardClause
            {
                get { return _guardClause; }
            }
        }
    }
}