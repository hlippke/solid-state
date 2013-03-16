using System;
using System.Collections.Generic;

namespace Solid.State
{
    public partial class SolidMachine<TTrigger>
    {
        // Variables

        private StateConfiguration _initialState;
        private bool _initialStateConfigured;

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

                // If this is the first state that is added, it becomes the initial state
                if (_stateConfigurations.Count == 0)
                    _initialState = configuration;

                _stateConfigurations.Add(type, configuration);
            }

            return configuration;
        }

        /// <summary>
        /// Starts the state machine by going to the initial state.
        /// </summary>
        public void Start()
        {
            throw new NotImplementedException();
        }

        // Properties

        /// <summary>
        /// Indicates if the initial state of the state machine has been configured yet.
        /// It can only be configured once.
        /// </summary>
        internal bool InitialStateConfigured
        {
            get { return _initialStateConfigured; }
        }

        internal void SetInitialState(StateConfiguration initialStateConfiguration)
        {
            _initialState = initialStateConfiguration;
            _initialStateConfigured = true;
        }

        /// <summary>
        /// The type that is the initial state.
        /// </summary>
        internal Type InitialState
        {
            get { return _initialState.StateType; }
        }

        public Dictionary<Type, StateConfiguration> StateConfigurations
        {
            get { return _stateConfigurations; }
        }
    }
}