using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid.State
{
    public partial class SolidMachine<TTrigger>
    {
        public class StateConfiguration
        {
            // Private variables

            private readonly SolidMachine<TTrigger> _machine;
            private readonly Type _stateType;
            private readonly List<TriggerConfiguration> _triggerConfigurations;
            
            private ISolidState _stateInstance;
            private int _totalJoinCount;
            private int _joinCounter;

            // Private methods

            private TriggerConfiguration GetConfigurationByTrigger(TTrigger trigger)
            {
                return _triggerConfigurations.FirstOrDefault(x => (x.Trigger.Equals(trigger)));
            }

            // Internal methods

            /// <summary>
            /// Enters a state, creating an instance of it if necessary.
            /// </summary>
            internal void Enter()
            {
                // Reset the join counter
                _joinCounter = _totalJoinCount;

                // Should a new instance be created?
                if ((_stateInstance == null) || (_machine.StateInstantiationMode == StateInstantiationMode.PerTransition))
                    _stateInstance = _machine.InstantiateState(_stateType);

                _stateInstance.Entering(_machine.GetContext());
            }

            /// <summary>
            /// Exits the state that this configuration is linked to.
            /// </summary>
            internal void Exit()
            {
                if (_stateInstance != null)
                {
                    _stateInstance.Exiting(_machine.GetContext());

                    // If we're instantiating per transition, we release the reference to the instance
                    if (_machine._stateInstantiationMode == StateInstantiationMode.PerTransition)
                        _stateInstance = null;
                }
            }

            // Internal properties

            /// <summary>
            /// The machine that owns this state configuration.
            /// </summary>
            internal SolidMachine<TTrigger> Machine
            {
                get { return _machine; }
            }

            internal IEnumerable<TriggerConfiguration> TriggerConfigurations
            {
                get { return _triggerConfigurations; }
            }

            internal Type StateType
            {
                get { return _stateType; }
            }

            /// <summary>
            /// The state instance that this belongs to this state configuration.
            /// </summary>
            internal ISolidState StateInstance
            {
                get { return _stateInstance; }
            }

            /// <summary>
            /// The index of the path that this state belongs to. New paths are created by
            /// forking the flow, and paths cease to exist when joins are performed.
            /// </summary>
            internal int PathIndex { get; set; }

            /// <summary>
            /// The total number of joins that converge on this state. It is used to determine
            /// when the state should actually be entered.
            /// </summary>
            internal int TotalJoinCount
            {
                get { return _totalJoinCount; }
                set
                {
                    _totalJoinCount = value;
                    _joinCounter = _totalJoinCount;
                }
            }

            /// <summary>
            /// Counts down as join transitions use this state as their target. When the counter
            /// has reached 0, the state is entered.
            /// </summary>
            internal int JoinCounter
            {
                get { return _joinCounter; }
                set { _joinCounter = value; }
            }

            // Constructor

            public StateConfiguration(Type stateType, SolidMachine<TTrigger> machine)
            {
                _stateType = stateType;
                _machine = machine;
                _triggerConfigurations = new List<TriggerConfiguration>();
            }

            // Methods

            public StateConfiguration IsInitialState()
            {
                if (_machine._initialStateConfigured)
                    throw new SolidStateException(
                        "The state machine cannot have multiple states configured as the initial state!");

                _machine.SetInitialState(this);

                return this;
            }

            /// <summary>
            /// Adds a guardless transition to the state configuration.
            /// </summary>
            /// <param name="trigger">The trigger that this state should accept.</param>
            /// <returns></returns>
            public TriggerConfiguration On(TTrigger trigger)
            {
                var existingConfig = GetConfigurationByTrigger(trigger);
                if (existingConfig != null)
                {
                    // Does the existing configuration have a guard clause?
                    if (existingConfig.GuardClause != null)
                        throw new SolidStateException(
                            string.Format(
                                "State {0} has at least one guarded transition configured on trigger {1} already. A state cannot have both guardless and guarded transitions at the same time!",
                                _stateType.Name, trigger));
                    else
                        throw new SolidStateException(string.Format(
                            "Trigger {0} has already been configured for state {1}!", trigger, _stateType.Name));
                }

                var newConfiguration = new TriggerConfiguration(trigger, null, this);
                _triggerConfigurations.Add(newConfiguration);

                return newConfiguration;
            }

            /// <summary>
            /// Adds a guarded transition to this state configuration.
            /// </summary>
            public TriggerConfiguration On(TTrigger trigger, Func<bool> guardClause)
            {
                if (guardClause == null) throw new ArgumentNullException("guardClause");

                var existingConfig = GetConfigurationByTrigger(trigger);
                if (existingConfig != null)
                {
                    // It's OK that there are multiple configurations of the same trigger, as long as they all have guard clauses
                    if (existingConfig.GuardClause == null)
                        throw new SolidStateException(
                            string.Format(
                                "State {0} has an unguarded transition for trigger {1}, you cannot add guarded transitions to this state as well!",
                                _stateType.Name, trigger));
                }

                var newConfiguration = new TriggerConfiguration(trigger, guardClause, this);
                _triggerConfigurations.Add(newConfiguration);

                return newConfiguration;
            }
        }
    }
}