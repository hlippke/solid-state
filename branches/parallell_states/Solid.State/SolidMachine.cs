using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid.State
{
    public partial class SolidMachine<TTrigger>
    {
        // Variables

        private const int DEFAULT_STATEHISTORY_TRIM_THRESHOLD = 100;
        private const int MIN_STATEHISTORY_TRIM_THRESHOLD = 10;
        private const double STATEHISTORY_TRIM_PERCENTAGE = 0.1; // Trim 10% of state history

        private readonly object _queueLockObject = new object();
        private readonly object _stateHistoryLockObject = new object();

        private readonly TriggerHandler _triggerHandler;
        private readonly Dictionary<Type, StateConfiguration> _stateConfigurations;
        private readonly List<Action> _transitionQueue;
        private readonly List<StateConfiguration> _stateHistory;

        private StateConfiguration _initialState;
        private List<StateConfiguration> _currentStates;
        
        private bool _initialStateConfigured;
        private bool _isStarted;

        private object _context;
        private IStateResolver _stateResolver;

        private Action<Type, TTrigger> _invalidTriggerHandler;

        private bool _stateResolverRequired;
        private bool _isProcessingQueue;
        
        private StateInstantiationMode _stateInstantiationMode;
        private int _stateHistoryTrimThreshold;

        // Private methods

        /// <summary>
        /// Throws an exception if the state machine hasn't been started yet.
        /// </summary>
        private void ThrowOnNotStarted()
        {
            if (!_isStarted)
                throw new SolidStateException("State machine is not started!");
        }

        /// <summary>
        /// Throws an exception with a specified message if the state machine is started.
        /// </summary>
        /// <param name="message"></param>
        private void ThrowOnStarted(string message)
        {
            if (_isStarted)
                throw new SolidStateException(message);
        }

        /// <summary>
        /// Enters a new state (if there is one) and raises the OnTransitioned event.
        /// </summary>
        private void EnterNewStates(Type previousStateType, StateConfiguration[] states, bool isJoin)
        {
            foreach (var state in states)
            {
                // If this state is already the current state, there is a configuration error in the state machine
                if (_currentStates.Contains(state))
                    throw new SolidStateException(
                        string.Format(
                            "There are multiple parallel paths to state {0}, please check your state machine configuration!",
                            state.StateType.Name));

                var isOkToEnterNow = true;
                if (isJoin)
                {
                    state.JoinCounter--;
                    isOkToEnterNow = (state.JoinCounter == 0);
                }

                if (isOkToEnterNow)
                {
                    if (state != null)
                        _currentStates.Add(state);

                    // Are we entering a new state?
                    if (state != null)
                        state.Enter();

                    // Raise an event about the transition (where TargetState can be null)
                    OnTransitioned(new TransitionedEventArgs(previousStateType,
                                                             (state == null) ? null : state.StateInstance));
                }
            }
        }

        /// <summary>
        /// Exits the current state and returns the Type of it.
        /// </summary>
        /// <returns></returns>
        private Type ExitState(StateConfiguration state, bool addToHistory)
        {
            if (state == null)
                return null;
            else
            {
                state.Exit();

                // Record it in the history (but only if we're not running in parallel)
                if (addToHistory && (_currentStates.Count == 1))
                    AddStateToHistory(state);

                // Remove from current states
                _currentStates.Remove(state);

                return state.StateType;
            }
        }

        /// <summary>
        /// Handles the processing of a trigger, calculating if it is valid and which target state we should go to.
        /// </summary>
        /// <param name="trigger"></param>
        private void DoTrigger(TTrigger trigger)
        {
            var triggerHandled = false;

            // Make a copy of the current states since it will be manipulated
            var states = new List<StateConfiguration>(_currentStates);

            foreach (var state in states)
            {
                // Find all trigger configurations with a matching trigger
                var triggers = state.TriggerConfigurations.Where(x => x.Trigger.Equals(trigger)).ToList();

                // No trigger configs found?
                if (triggers.Count == 0)
                {
                    // Do we have a handler for the situation? If there is only one current state, raise an exception
                    if (_invalidTriggerHandler == null)
                    {
                        if (state.PathIndex < 0)
                            throw new SolidStateException(string.Format("Trigger {0} is not valid for state {1}!",
                                                                        trigger,
                                                                        state.StateType.Name));
                    }
                    else
                        // Let the handler decide what to do
                        _invalidTriggerHandler(state.StateType, trigger);
                }
                else
                {
                    // Is it a single, unguarded trigger?
                    if (triggers[0].GuardClause == null)
                    {
                        var previousStateType = ExitState(state, addToHistory: true);
                        triggerHandled = true;
                        EnterNewStates(previousStateType, triggers[0].TargetStates, triggers[0].IsJoin);
                    }
                    else
                    {
                        // First exit the current state, it may affect the evaluation of the guard clauses
                        var previousStateType = ExitState(state, addToHistory: true);

                        TriggerConfiguration matchingTrigger = null;
                        foreach (var tr in triggers)
                        {
                            if (tr.GuardClause())
                            {
                                if (matchingTrigger != null)
                                    throw new SolidStateException(string.Format(
                                        "State {0}, trigger {1} has multiple guard clauses that simultaneously evaulate to True!",
                                        previousStateType.Name, trigger));
                                matchingTrigger = tr;
                            }
                        }

                        // Did we find a matching trigger and are not forked?
                        if (matchingTrigger == null)
                        {
                            if (state.PathIndex < 0)
                                throw new SolidStateException(string.Format(
                                    "State {0}, trigger {1} has no guard clause that evaulate to True!",
                                    previousStateType.Name, trigger));
                        }
                        else
                        {
                            triggerHandled = true;
                            // Queue up the transition
                            EnterNewStates(previousStateType, matchingTrigger.TargetStates, matchingTrigger.IsJoin);
                        }
                    }
                }
            }

            // Was the trigger unhandled and there is no external handler?
            if (!triggerHandled && (_invalidTriggerHandler == null))
                throw new SolidStateException(
                    string.Format("Trigger {0} is not valid for any of the current states: {1}", trigger,
                                  string.Join(", ", _currentStates.Select(x => x.StateType.Name))));
        }

        private void AddStateToHistory(StateConfiguration state)
        {
            lock (_stateHistoryLockObject)
            {
                if (state != null)
                    _stateHistory.Insert(0, state);

                // Time to trim it?
                if (_stateHistory.Count > _stateHistoryTrimThreshold)
                {
                    var trimValue = (int)(_stateHistoryTrimThreshold * (1.0 - STATEHISTORY_TRIM_PERCENTAGE));
                    while (_stateHistory.Count > trimValue)
                        _stateHistory.RemoveAt(trimValue);
                }
            }
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

        private StateConfiguration LinkToState(StateConfiguration sourceState, Type targetStateType, int pathIndex)
        {
            // Does the state have a parameterless constructor? Otherwise a state resolver is required
            HandleResolverRequired(targetStateType);

            // Does a configuration for this state exist already?
            StateConfiguration configuration;
            if (_stateConfigurations.ContainsKey(targetStateType))
            {
                configuration = _stateConfigurations[targetStateType];

                // If the target state doesn't belong to any path yet, it adopts the source state's index
                if (configuration.PathIndex < 0)
                    configuration.PathIndex = sourceState == null ? 0 : sourceState.PathIndex;

                if ((sourceState != null) && (sourceState.PathIndex != configuration.PathIndex))
                    throw new SolidStateException(
                        string.Format(
                            "Cannot create a transition from state {0} to state {1}, they're on different paths!",
                            sourceState.StateType.Name, targetStateType.Name));
            }
            else
            {
                configuration = new StateConfiguration(targetStateType, this);
                configuration.PathIndex = pathIndex;

                // If this is the first state that is added, it becomes the initial state
                if (_stateConfigurations.Count == 0)
                    _initialState = configuration;

                _stateConfigurations.Add(targetStateType, configuration);
            }

            return configuration;

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
            if (!_isStarted || (_currentStates == null))
                return new List<TTrigger>();

            // Return a distinct list (no duplicates) of triggers for all current states
            return _currentStates.SelectMany(x => x.TriggerConfigurations).Select(x => x.Trigger).Distinct().ToList();
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

        // Constructors

        public SolidMachine()
        {
            _triggerHandler = new TriggerHandler();
            _stateConfigurations = new Dictionary<Type, StateConfiguration>();
            _transitionQueue = new List<Action>();
            _stateHistory = new List<StateConfiguration>();
            _currentStates = new List<StateConfiguration>();

            _stateHistoryTrimThreshold = DEFAULT_STATEHISTORY_TRIM_THRESHOLD;
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
        /// Defines a state that should be configured. Since this method belongs to the state
        /// machine, it it not possible to determine the correct path index for the state, so
        /// it's set to -1 = undefined.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        public StateConfiguration State<TState>() where TState : ISolidState
        {
            return LinkToState(null, typeof (TState), -1);
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

            // Enter the initial state
            EnterNewStates(null, new[] {_initialState}, isJoin: false);
        }

        /// <summary>
        /// Stops the state machine by exiting the current state without entering a new one.
        /// </summary>
        public void Stop()
        {
            // Ignore this call if the machine hasn't been started yet
            if (!_isStarted)
                return;

            try
            {
                // Empty the queue
                lock (_queueLockObject)
                    _transitionQueue.Clear();

                // Exit the current states and raise an event about each
                var states = new List<StateConfiguration>(_currentStates);
                foreach (var state in states)
                {
                    var previousStateType = ExitState(state, false);
                    OnTransitioned(new TransitionedEventArgs(previousStateType, null));
                }
            }
            finally
            {
                _isStarted = false;
            }
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
        /// Adds a request to fire a trigger to the processing queue.
        /// </summary>
        /// <param name="trigger"></param>
        public void Trigger(TTrigger trigger)
        {
            ThrowOnNotStarted();

            // Queue it up
            lock (_queueLockObject)
            {
                var localTrigger = trigger;
                _transitionQueue.Add(() => _triggerHandler.Handle(localTrigger));
            }

            ProcessTransitionQueue();
        }

        /// <summary>
        /// Moves the state machine back to the previous state, ignoring valid triggers, guard clauses etc.
        /// </summary>
        public void GoBack()
        {
            if (_currentStates.Count > 1)
                throw new SolidStateException("Cannot go back when running parallel states!");

            StateConfiguration targetState;
            Type previousStateType;

            lock (_stateHistoryLockObject)
            {
                // If the history is empty, we just ignore the call
                if (_stateHistory.Count == 0)
                    return;

                previousStateType = ExitState(_currentStates[0], addToHistory: false);

                targetState = _stateHistory[0];
                _stateHistory.RemoveAt(0);
            }

            if (targetState != null)
                EnterNewStates(previousStateType, new[] {targetState}, isJoin: false);
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
        /// Returns an array of the states the machine is currently in.
        /// </summary>
        public ISolidState[] CurrentStates
        {
            get { return _currentStates.Select(x => x.StateInstance).ToArray(); }
        }

        /// <summary>
        /// Returns true if any of the current states are of the specified type.
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <returns></returns>
        public bool IsInState<TState>() where TState : ISolidState
        {
            return _currentStates.Any(x => x.StateInstance.GetType() == typeof (TState));
        }

        /// <summary>
        /// A list of the triggers that are valid to use on any of the current states.
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
        /// for each transition. Default value is Singleton.
        /// </summary>
        public StateInstantiationMode StateInstantiationMode
        {
            get { return _stateInstantiationMode; }
            set
            {
                // This is only OK to change 
                ThrowOnStarted("The StateInstantiationMode must be set before the state machine is started!");
                _stateInstantiationMode = value;
            }
        }

        /// <summary>
        /// Returns an array of the states the state machine has been in. The last state is at index 0.
        /// The current state is not part of the list.
        /// </summary>
        public Type[] StateHistory
        {
            get
            {
                lock (_stateHistoryLockObject)
                    return _stateHistory.Select(x => x.StateType).ToArray();
            }
        }

        /// <summary>
        /// The number of entries that will be kept in the state history before an automatic
        /// trim is performed.
        /// </summary>
        public int StateHistoryTrimThreshold
        {
            get { return _stateHistoryTrimThreshold; }
            set
            {
                // Can't set a too low value
                if (value < MIN_STATEHISTORY_TRIM_THRESHOLD)
                    value = MIN_STATEHISTORY_TRIM_THRESHOLD;

                _stateHistoryTrimThreshold = value;
                
                // If the new value is lower we may need a trim right away
                AddStateToHistory(null);
            }
        }
    }

    /// <summary>
    /// Enumeration of possible state instantiation modes.
    /// </summary>
    public enum StateInstantiationMode
    {
        /// <summary>
        /// The state class is instantiated the first time it is used. All following
        /// transitions into that state will use the same instance.
        /// </summary>
        Singleton,

        /// <summary>
        /// The target state of a transition is instantiated on each transition.
        /// </summary>
        PerTransition
    }
}