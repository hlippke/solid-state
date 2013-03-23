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

            private readonly SolidMachine<TTrigger> _owningMachine;
            private readonly Type _stateType;
            private readonly List<TriggerConfiguration> _triggerConfigurations;
            
            private ISolidState _stateInstance;

            // Private methods

            private TriggerConfiguration GetConfigurationByTrigger(TTrigger trigger)
            {
                return _triggerConfigurations.FirstOrDefault(x => (x.Trigger.Equals(trigger)));
            }

            /// <summary>
            /// Enters a state, creating an instance of it if necessary.
            /// </summary>
            internal void Enter()
            {
                // Should a new instance be created?
                if ((_stateInstance == null) || (_owningMachine._instantiateStatePerTransition))
                    _stateInstance = _owningMachine.InstantiateState(_stateType);

                _stateInstance.Entering(_owningMachine.GetContext());
            }

            /// <summary>
            /// Exits the state that this configuration is linked to.
            /// </summary>
            internal void Exit()
            {
                if (_stateInstance != null)
                    _stateInstance.Exiting(_owningMachine.GetContext());
            }

            // Constructor

            public StateConfiguration(Type stateType, SolidMachine<TTrigger> owningMachine)
            {
                _stateType = stateType;
                _owningMachine = owningMachine;
                _triggerConfigurations = new List<TriggerConfiguration>();
            }

            // Methods

            public StateConfiguration IsInitialState()
            {
                if (_owningMachine._initialStateConfigured)
                    throw new SolidStateException(
                        "The state machine cannot have multiple states configured as the initial state!");

                _owningMachine.SetInitialState(this);

                return this;
            }

            /// <summary>
            /// Adds a guardless permitted transition to the state configuration.
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

            // Properties

            internal SolidMachine<TTrigger> OwningMachine
            {
                get { return _owningMachine; }
            }

            internal List<TriggerConfiguration> TriggerConfigurations
            {
                get { return _triggerConfigurations; }
            }

            internal Type StateType
            {
                get { return _stateType; }
            }

            internal ISolidState StateInstance
            {
                get { return _stateInstance; }
            }
        }
    }
}