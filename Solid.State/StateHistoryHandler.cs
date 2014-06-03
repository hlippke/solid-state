using System;
using System.Collections.Generic;
using System.Linq;

namespace Solid.State
{
    public partial class SolidMachine<TTrigger>
    {
        /// <summary>
        /// Handles the state history of the state machine, keeping
        /// track of all visited states and performing back operations.
        /// </summary>
        private class StateHistoryHandler
        {
            // Private variables

            private const int DEFAULT_STATEHISTORY_TRIM_THRESHOLD = 100;

            private readonly object _stateHistoryLockObject = new object();
            private readonly SolidMachine<TTrigger> _machine;
            private readonly List<StateConfiguration> _stateHistory;
            
            private int _trimThreshold;

            // Constructor

            public StateHistoryHandler(SolidMachine<TTrigger> machine)
            {
                _machine = machine;
                _stateHistory = new List<StateConfiguration>();

                _trimThreshold = DEFAULT_STATEHISTORY_TRIM_THRESHOLD;
            }

            // Public methods

            /// <summary>
            /// Adds a state to the history, performing trimming if necessary.
            /// </summary>
            /// <param name="state"></param>
            public void Add(StateConfiguration state)
            {
                lock (_stateHistoryLockObject)
                {
                    if (state != null)
                        _stateHistory.Insert(0, state);

                    // Time to trim it?
                    if (_stateHistory.Count > _machine.StateHistoryTrimThreshold)
                    {
                        var trimValue = (int)(_machine.StateHistoryTrimThreshold * (1.0 - STATEHISTORY_TRIM_PERCENTAGE));
                        while (_stateHistory.Count > trimValue)
                            _stateHistory.RemoveAt(trimValue);
                    }
                }
            }

            /// <summary>
            /// Goes back to the previous state without the use of triggers. Can only be used
            /// when there are no parallel states.
            /// </summary>
            public void GoBack()
            {
                // Get a list of the current states
                var states = _machine.CurrentStateConfigurations.ToList();

                if (states.Count > 1)
                    throw new SolidStateException(Constants.ExcCannotGoBackWhenParallelId,
                                                  Constants.ExcCannotGoBackWhenParallelMessage);

                StateConfiguration targetState;
                Type previousStateType;

                lock (_stateHistoryLockObject)
                {
                    // If the history is empty, we just ignore the call
                    if (_stateHistory.Count == 0)
                        return;

                    // Exit the current state
                    previousStateType = _machine.ExitState(states[0], addToHistory: false);

                    targetState = _stateHistory[0];
                    _stateHistory.RemoveAt(0);
                }

                if (targetState != null)
                    _machine.EnterNewStates(previousStateType, new[] { targetState }, isJoin: false);
            }

            // Properties
            
            public Type[] GetStateTypes()
            {
                return _stateHistory.Select(x => x.StateType).ToArray();
            }

            public int TrimThreshold
            {
                get { return _trimThreshold; }
                set
                {
                    // Can't set a too low value
                    if (value < MIN_STATEHISTORY_TRIM_THRESHOLD)
                        value = MIN_STATEHISTORY_TRIM_THRESHOLD;

                    _trimThreshold = value;

                    // If the new value is lower we may need a trim right away
                    Add(null);
                }
            }

        }
    }
}