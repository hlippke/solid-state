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
                _targetStates = new[]
                    {
                        _owningStateConfiguration.OwningMachine.LinkToState(_owningStateConfiguration, typeof(TTargetState), _owningStateConfiguration.PathIndex)
                    };

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
                        _owningStateConfiguration.OwningMachine.LinkToState(_owningStateConfiguration, typeof(TTargetState1), _owningStateConfiguration.PathIndex),
                        _owningStateConfiguration.OwningMachine.LinkToState(_owningStateConfiguration, typeof(TTargetState2), _owningStateConfiguration.PathIndex+1)
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
                        _owningStateConfiguration.OwningMachine.LinkToState(_owningStateConfiguration, typeof(TTargetState1), _owningStateConfiguration.PathIndex),
                        _owningStateConfiguration.OwningMachine.LinkToState(_owningStateConfiguration, typeof(TTargetState2), _owningStateConfiguration.PathIndex+1),
                        _owningStateConfiguration.OwningMachine.LinkToState(_owningStateConfiguration, typeof(TTargetState3), _owningStateConfiguration.PathIndex+2)
                    };

                return GetStateConfiguration();
            }

            public StateConfiguration JoinsTo<TTargetState>() where TTargetState : ISolidState
            {
                var targetState = _owningStateConfiguration.OwningMachine.State<TTargetState>();

                // The target adopts the lowest path index of all the states that join at it
                if ((targetState.PathIndex<0) || (_owningStateConfiguration.PathIndex<targetState.PathIndex))
                    targetState.PathIndex = _owningStateConfiguration.PathIndex;

                IsJoin = true;
                targetState.TotalJoinCount++;

                _targetStates = new[] {targetState};

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

            public bool IsJoin { get; set; }
        }
    }
}