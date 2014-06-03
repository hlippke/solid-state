using System.Collections.Generic;
using System.Linq;

namespace Solid.State
{
    public partial class SolidMachine<TTrigger>
    {
        /// <summary>
        /// Handles logic concerning how a fired trigger affects the
        /// state machine.
        /// </summary>
        public class TriggerHandler
        {
            // Private variables

            private readonly SolidMachine<TTrigger> _machine;

            // Constructor

            public TriggerHandler(SolidMachine<TTrigger> machine)
            {
                _machine = machine;
            }

            // Public methods

            public void Handle(TTrigger trigger)
            {
                var triggerHandled = false;

                List<StateConfiguration> states;
                lock (_machine._queueLockObject)
                    states = new List<StateConfiguration>(_machine.CurrentStateConfigurations);
                
                foreach (var state in states)
                {
                    // Find all trigger configurations with a matching trigger
                    var triggers = state.TriggerConfigurations.Where(x => x.Trigger.Equals(trigger)).ToList();

                    // No trigger configs found?
                    if (triggers.Count == 0)
                    {
                        // Do we have a handler for the situation? If we're on the main execution path we throw an exception
                        if (_machine.InvalidTriggerHandler == null)
                        {
                            if (state.PathIndex < 0)
                                throw new SolidStateException(
                                    Constants.ExcInvalidTriggerId,
                                    string.Format(Constants.ExcInvalidTriggerMessage, trigger, state.StateType.Name));
                        }
                        else
                            // Let the handler decide what to do
                            _machine.InvalidTriggerHandler(state.StateType, trigger);
                    }
                    else
                    {
                        // Is it a single, unguarded trigger? Then exit and enter immediately
                        if (triggers[0].GuardClause == null)
                        {
                            var previousStateType = _machine.ExitState(state, addToHistory: true);
                            triggerHandled = true;

                            _machine.EnterNewStates(previousStateType, triggers[0].TargetStates, triggers[0].IsJoin);
                        }
                        else
                        {
                            // First exit the current state, it may affect the evaluation of the guard clauses
                            var previousStateType = _machine.ExitState(state, addToHistory: true);

                            TriggerConfiguration matchingTrigger = null;
                            foreach (var tr in triggers)
                            {
                                if (tr.GuardClause())
                                {
                                    if (matchingTrigger != null)
                                        throw new SolidStateException(
                                            Constants.ExcMultipleGuardsAreTrueId,
                                            string.Format(Constants.ExcMultipleGuardsAreTrueMessage, previousStateType.Name, trigger));
                                    matchingTrigger = tr;
                                }
                            }

                            // Did we find a matching trigger?
                            if (matchingTrigger == null)
                            {
                                if (state.PathIndex < 0)
                                    throw new SolidStateException(
                                        Constants.ExcNoGuardsAreTrueId,
                                        string.Format(Constants.ExcNoGuardsAreTrueMessage, previousStateType.Name, trigger));
                            }
                            else
                            {
                                triggerHandled = true;
                                // Queue up the transition
                                _machine.EnterNewStates(previousStateType, matchingTrigger.TargetStates, matchingTrigger.IsJoin);
                            }
                        }
                    }
                }

                // Was the trigger unhandled and there is no external handler?
                if (!triggerHandled && (_machine.InvalidTriggerHandler == null))
                    throw new SolidStateException(
                        Constants.ExcInvalidTriggerForMultipleStatesId,
                        string.Format(Constants.ExcInvalidTriggerForMultipleStatesMessage, trigger,
                        string.Join(", ", states.Select(x => x.StateType.Name))));
            }
        }
    }
}
