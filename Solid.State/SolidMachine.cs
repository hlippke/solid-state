using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid.State
{
    public partial class SolidMachine<TTrigger>
    {
        // Variables

        private StateConfiguration _initialState;
        private StateConfiguration _currentState;
        
        private bool _initialStateConfigured;
        private bool _isStarted;

        private object _context;
        private IStateResolver _stateResolver;

        internal Dictionary<Type, StateConfiguration> _stateConfigurations;
        private bool _stateResolverRequired;

        // Private methods

        private void HandleMachineStarted()
        {
            if (!_isStarted)
                throw new SolidStateException("State machine is not started!");
        }

        private void GotoState(StateConfiguration state)
        {
            // Are we leaving a state?
            if (_currentState != null)
                _currentState.Exit();

            _currentState = state;

            // Are we entering a new state?
            if (_currentState != null)
                _currentState.Enter();
        }

        private void SetInitialState(StateConfiguration initialStateConfiguration)
        {
            _initialState = initialStateConfiguration;
            _initialStateConfigured = true;
        }

        /// <summary>
        /// Creates an instance of a specified state type, either through .NET activation
        /// or through a defined state resolver.
        /// </summary>
        /// <param name="stateType"></param>
        /// <returns></returns>
        private ISolidState InstantiateState(Type stateType)
        {
            // Do we have a state resolver?
            if (_stateResolver != null)
                return _stateResolver.ResolveState(stateType);

            // No, use standard .NET activator
            return (SolidState) Activator.CreateInstance(stateType);
        }

        /// <summary>
        /// Checks if a state resolver will be required on state machine startup.
        /// </summary>
        /// <param name="stateType"></param>
        private void HandleResolverRequired(Type stateType)
        {
            // A state resolver is required if a configured state has no parameterless constructor
            _stateResolverRequired = (stateType.GetConstructor(Type.EmptyTypes) == null);
        }

        /// <summary>
        /// Gets the object that should be used as state context.
        /// </summary>
        /// <returns></returns>
        private object GetContext()
        {
            return _context ?? this;
        }

        // Constructor

        public SolidMachine()
        {
            _stateConfigurations = new Dictionary<Type, StateConfiguration>();
        }

        public SolidMachine(object context) : this()
        {
            _context = context;
        }

        public SolidMachine(object context, IStateResolver stateResolver) : this(context)
        {
            _context = context;
            _stateResolver = stateResolver;
        }

        // Public methods

        public StateConfiguration State<TState>() where TState : ISolidState
        {
            var type = typeof (TState);
            // Does the state have a parameterless constructor? Otherwise a state resolver is required
            HandleResolverRequired(type);

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
            if (_initialState == null)
                throw new SolidStateException("No states have been configured!");

            // Is a state resolver required without us having one?
            if (_stateResolverRequired && (_stateResolver == null))
                throw new SolidStateException(
                    "One or more configured states has no parameterless constructor. Add such constructors or make sure that the StateResolver property is set!");

            _isStarted = true;

            // Transition to initial state
            GotoState(_initialState);
        }

        /// <summary>
        /// Notifies the state machine that a transition should be triggered.
        /// </summary>
        /// <param name="trigger"></param>
        public void Trigger(TTrigger trigger)
        {
            HandleMachineStarted();

            var triggers = _currentState.TriggerConfigurations.Where(x => x.Trigger.Equals(trigger)).ToList();
            if (triggers.Count == 0)
                throw new SolidStateException(string.Format("Trigger {0} is not valid for state {1}!", trigger,
                                                            _currentState.StateType.Name));

            // Is it a single, unguarded trigger?
            if (triggers[0].GuardClause == null)
                GotoState(triggers[0].TargetState);
            else
            {
                TriggerConfiguration matchingTrigger = null;

                foreach (var tr in triggers)
                {
                    if (tr.GuardClause())
                    {
                        if (matchingTrigger != null)
                            throw new SolidStateException(string.Format(
                                "State {0}, trigger {1} has multiple guard clauses that simultaneously evaulate to True!",
                                _currentState.StateType.Name, trigger));
                        matchingTrigger = tr;
                    }
                }

                // Did we find a matching trigger?
                if (matchingTrigger == null)
                    throw new SolidStateException(string.Format(
                        "State {0}, trigger {1} has no guard clause that evaulate to True!",
                        _currentState.StateType.Name, trigger));

                GotoState(matchingTrigger.TargetState);
            }
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

        /// <summary>
        /// Returns the state that the state machine is currently in.
        /// </summary>
        public ISolidState CurrentState
        {
            get
            {
                if (_currentState == null)
                    return null;
                else
                    return _currentState.StateInstance;
            }
        }

        /// <summary>
        /// An arbitrary object that will be passed on to the states in their entry and exit methods.
        /// If no context is defined, the state machine instance will be used as context.
        /// </summary>
        public object Context
        {
            get { return _context; }
            set { _context = value; }
        }

        /// <summary>
        /// The resolver for state machine states. If this is not specified the standard
        /// .NET activator is used and all states must then have parameterless constructors.
        /// </summary>
        public IStateResolver StateResolver
        {
            get { return _stateResolver; }
            set { _stateResolver = value; }
        }
    }
}