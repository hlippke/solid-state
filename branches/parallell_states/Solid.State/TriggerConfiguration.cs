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

            private readonly Func<bool> _guardClause;

            private StateConfiguration[] _targetStates;
            private bool _isJoin;

            // Private methods

            private StateConfiguration GetStateConfiguration()
            {
                var machine = _owningStateConfiguration.OwningMachine;
                return machine._stateConfigurations[_owningStateConfiguration.StateType];
            }

            // Constructor

            public TriggerConfiguration(TTrigger trigger, StateConfiguration owningStateConfiguration)
            {
                _trigger = trigger;
                _owningStateConfiguration = owningStateConfiguration;
            }

            public TriggerConfiguration(TTrigger trigger, Func<bool> guardClause,
                                        StateConfiguration owningStateConfiguration)
            {
                _guardClause = guardClause;
                _trigger = trigger;
                _owningStateConfiguration = owningStateConfiguration;
            }

            // Methods

            /// <summary>
            /// Specifies the target state for the specified trigger configuration.
            /// </summary>
            public StateConfiguration GoesTo<TTargetState>() where TTargetState : ISolidState
            {
                _targetStates = new[] {_owningStateConfiguration.OwningMachine.State<TTargetState>()};

                // Return the correct StateConfiguration
                return GetStateConfiguration();
            }

            /// <summary>
            /// Specifies two parallel target states for the specified trigger configuration.
            /// </summary>
            public StateConfiguration ForksTo<TTargetState1, TTargetState2>() where TTargetState1 : ISolidState
                where TTargetState2 : ISolidState
            {
                _targetStates = new[]
                    {
                        _owningStateConfiguration.OwningMachine.State<TTargetState1>(),
                        _owningStateConfiguration.OwningMachine.State<TTargetState2>()
                    };

                return GetStateConfiguration();
            }

            /// <summary>
            /// Specifies three parallel target states for the specified trigger configuration.
            /// </summary>
            public StateConfiguration ForksTo<TTargetState1, TTargetState2, TTargetState3>()
                where TTargetState1 : ISolidState where TTargetState2 : ISolidState where TTargetState3 : ISolidState
            {
                _targetStates = new[]
                    {
                        _owningStateConfiguration.OwningMachine.State<TTargetState1>(),
                        _owningStateConfiguration.OwningMachine.State<TTargetState2>(),
                        _owningStateConfiguration.OwningMachine.State<TTargetState3>()
                    };

                return GetStateConfiguration();
            }

            public StateConfiguration JoinsTo<TTargetState>() where TTargetState : ISolidState
            {
                var targetState = _owningStateConfiguration.OwningMachine.State<TTargetState>();
                _targetStates = new[] {targetState};

                // Mark this configuration as a join
                _isJoin = true;

                // Increase the target join count
                targetState.TotalJoinCount++;

                return GetStateConfiguration();
            }

            // Properties

            internal TTrigger Trigger
            {
                get { return _trigger; }
            }

            internal StateConfiguration[] TargetStates
            {
                get { return _targetStates; }
            }

            internal Func<bool> GuardClause
            {
                get { return _guardClause; }
            }

            internal bool IsJoin
            {
                get { return _isJoin; }
            }
        }
    }
}