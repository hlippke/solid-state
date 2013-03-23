using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid.State
{
    public partial class SolidMachine<TTrigger>
    {
        // Variables

        private readonly object _queueLockObject = new object();
        private readonly Dictionary<Type, StateConfiguration> _stateConfigurations;
        private readonly List<Action> _transitionQueue; 

        private StateConfiguration _initialState;
        private StateConfiguration _currentState;
        
        private bool _initialStateConfigured;
        private bool _isStarted;

        private object _context;
        private IStateResolver _stateResolver;

        private Action<Type, TTrigger> _invalidTriggerHandler;

        private bool _stateResolverRequired;
        private bool _isProcessingQueue;
        private bool _instantiateStatePerTransition;

        // Private methods

        private void HandleMachineStarted()
        {
            if (!_isStarted)
                throw new SolidStateException("State machine is not started!");
        }

        /// <summary>
        /// Performs a transition by exiting the current state and entering
        /// the next. This method should never be called directly, instead use
        /// the GotoState method to use the transition queue which will make
        /// sure that the transitions happens in the correct order.
        /// </summary>
        /// <param name="state"></param>
        private void ExecuteTransition(StateConfiguration state)
        {
            Type previousState = null;
            Type currentState = null;

            // Are we leaving a state?
            if (_currentState != null)
            {
                previousState = _currentState.StateType;
                _currentState.Exit();
            }

            _currentState = state;

            // Are we entering a new state?
            if (_currentState != null)
            {
                currentState = _currentState.StateType;
                _currentState.Enter();
            }

            // Raise an event about the transition
            OnTransitioned(new TransitionedEventArgs(previousState, currentState));
        }

        /// <summary>
        /// Loops through the transition queue until it is empty, executing the queued
        /// calls to the ExecuteTransition method.
        /// </summary>
        private void ProcessTransitionQueue()
        {
            if (_isProcessingQueue)
                return;

            try
            {
                _isProcessingQueue = true;

                do
                {
                    Action nextAction = null;

                    // Lock queue during readout of next action
                    lock (_queueLockObject)
                    {
                        if (_transitionQueue.Count == 0)
                            return;

                        nextAction = _transitionQueue[0];
                        _transitionQueue.RemoveAt(0);
                    }

                    if (nextAction != null)
                        nextAction();
                } while (true);
            }
            finally
            {
                _isProcessingQueue = false;
            }
        }

        /// <summary>
        /// Queues up a transition to a new state.
        /// </summary>
        /// <param name="state"></param>
        private void GotoState(StateConfiguration state)
        {
            // Add to queue
            var processQueueNeeded = false;

            lock (_queueLockObject)
            {
                var localState = state;
                _transitionQueue.Add(() => ExecuteTransition(localState));
            }

            // Do we need to process the queue? Do it outside lock
            ProcessTransitionQueue();
        }

        /// <summary>
        /// Sets the initial state of the state machine.
        /// </summary>
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
            {
                var instance = _stateResolver.ResolveState(stateType);
                if (instance == null)
                    throw new SolidStateException(string.Format("State resolver returned null for type '{0}'!",
                                                                stateType.Name));
                return instance;
            }
            else
                return (ISolidState) Activator.CreateInstance(stateType);
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

        /// <summary>
        /// Returns a list of all triggers that are valid to use on the current state.
        /// </summary>
        /// <returns></returns>
        private List<TTrigger> GetValidTriggers()
        {
            if (!_isStarted || (_currentState == null))
                return new List<TTrigger>();

            // Return a distinct list (no duplicates) of triggers
            return _currentState.TriggerConfigurations.Select(x => x.Trigger).Distinct().ToList();
        }

        // Protected methods

        /// <summary>
        /// Raises the Transitioned event.
        /// </summary>
        /// <param name="eventArgs"></param>
        protected virtual void OnTransitioned(TransitionedEventArgs eventArgs)
        {
            if (Transitioned != null)
                Transitioned(this, eventArgs);
        }

        // Constructor

        public SolidMachine()
        {
            _stateConfigurations = new Dictionary<Type, StateConfiguration>();
            _transitionQueue = new List<Action>();
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

        // Events

        /// <summary>
        /// Raised when the state machine has transitioned from one state to another.
        /// </summary>
        public event TransitionedEventHandler Transitioned;

        // Public methods

        /// <summary>
        /// Defines a state that should be configured.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
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

            // If there are states that has no parameterless constructor, we must have set the StateResolver property.
            if (_stateResolverRequired && (_stateResolver == null))
                throw new SolidStateException(
                    "One or more configured states has no parameterless constructor. Add such constructors or make sure that the StateResolver property is set!");

            _isStarted = true;

            // Transition to initial state
            GotoState(_initialState);
        }

        /// <summary>
        /// Sets the invalid trigger handler that will be called when a trigger is used
        /// that isn't valid for the current state. If no handler is specified, an
        /// exception will be thrown.
        /// </summary>
        public void OnInvalidTrigger(Action<Type, TTrigger> invalidTriggerHandler)
        {
            _invalidTriggerHandler = invalidTriggerHandler;
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
            {
                // Do we have a handler for the situation?
                if (_invalidTriggerHandler == null)
                    throw new SolidStateException(string.Format("Trigger {0} is not valid for state {1}!", trigger,
                                                            _currentState.StateType.Name));
                // Let the handler decide what to do
                _invalidTriggerHandler(_currentState.StateType, trigger);
            }
            else
            {
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

                    // Queue up the transition
                    GotoState(matchingTrigger.TargetState);
                }
            }
        }

        // Properties

        /// <summary>
        /// The type of the machines initial state.
        /// </summary>
        public Type InitialState
        {
            get { return _initialState.StateType; }
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
        /// A list of the triggers that are valid to use on the current state.
        /// If the machine hasn't been started yet, this list is empty.
        /// </summary>
        public List<TTrigger> ValidTriggers
        {
            get { return GetValidTriggers(); }
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

        /// <summary>
        /// Controls whether state class instances should be Singletons or if they should be instantiated
        /// for each transition. Default value = false.
        /// </summary>
        public bool InstantiateStatePerTransition
        {
            get { return _instantiateStatePerTransition; }
            set { _instantiateStatePerTransition = value; }
        }
    }
}