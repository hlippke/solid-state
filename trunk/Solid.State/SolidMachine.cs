using System;
using System.Collections.Generic;

namespace Solid.State
{
    public partial class SolidMachine<TTrigger>
    {
        // Private variables

        internal Dictionary<Type, StateConfiguration> _stateConfigurations;

        // Constructor

        public SolidMachine()
        {
            _stateConfigurations = new Dictionary<Type, StateConfiguration>();
        }

        // Public methods

        public StateConfiguration State<TState>() where TState : SolidState
        {
            var type = typeof (TState);

            // Does a configuration for this state exist already?
            StateConfiguration configuration;
            if (_stateConfigurations.ContainsKey(typeof(TState)))
                configuration = _stateConfigurations[typeof(TState)];
            else
            {
                configuration = new StateConfiguration(type, this);
                _stateConfigurations.Add(type, configuration);
            }

            return configuration;
        }

        // Properties

        public Dictionary<Type, StateConfiguration> StateConfigurations
        {
            get { return _stateConfigurations; }
        }
    }
}