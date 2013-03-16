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

            // Private methods

            private TriggerConfiguration GetConfigurationByTrigger(TTrigger trigger)
            {
                return _triggerConfigurations.FirstOrDefault(x => (x.Trigger.Equals(trigger)));
            }

            // Constructor

            public StateConfiguration(Type stateType, SolidMachine<TTrigger> owningMachine)
            {
                _stateType = stateType;
                _owningMachine = owningMachine;
                _triggerConfigurations = new List<TriggerConfiguration>();
            }

            // Methods

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
                                "State {0} has at least one guarded transition configured on trigger {1} already. A state cannot have both guardless and guarded transitions at the same time.",
                                _stateType.Name, trigger));
                    else
                        throw new SolidStateException(string.Format(
                            "Trigger {0} has already been configured for state {1}", trigger, _stateType.Name));
                }

                var newConfiguration = new TriggerConfiguration(trigger, null, this);
                _triggerConfigurations.Add(newConfiguration);

                return newConfiguration;
            }

            public TriggerConfiguration On(TTrigger trigger, Func<bool> guardClause)
            {
                throw new NotImplementedException();
            }

            public StateConfiguration IsSubStateOf<TState>() where TState : SolidState
            {
                throw new NotImplementedException();
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

            public Type StateType
            {
                get { return _stateType; }
            }
        }
    }
}