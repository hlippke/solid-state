namespace Solid.State
{
    public class Constants
    {
        // Exception messages

        public const int ExcMachineNotStartedId = 1000;
        public const string ExcMachineNotStartedMessage = "State machine is not started!";

        public const int ExcMultipleParallelPathsToStateId = 1001;
        public const string ExcMultipleParallelPathsToStateMessage =
            "There are multiple parallel paths to state {0}, please check your state machine configuration!";

        public const int ExcTransitionsBetweenStatePathsId = 1002;
        public const string ExcTransitionsBetweenStatePathsMessage = "Cannot create a transition from state {0} to state {1}, they're on different paths!";

        public const int ExcStateResolverReturnedNullId = 1003;
        public const string ExcStateResolverReturnedNullMessage = "State resolver returned null for type '{0}'!";

        public const int ExcNoStatesConfiguredId = 1004;
        public const string ExcNoStatesConfiguredMessage = "No states have been configured!";

        public const int ExcStatesHaveNoParameterlessConstructorId = 1005;
        public const string ExcStatesHaveNoParameterlessConstructorMessage =
            "One or more configured states have no parameterless constructor. Add such constructors or make sure that the StateResolver property is set!";

        public const int ExcCurrentStateWhenParallelId = 1006;
        public const string ExcCurrentStateWhenParallelMessage = "The CurrentState property cannot be used when the state machine contains parallel states!";

        public const int ExcStateInstantiationModeMustBeSetBeforeStartedId = 1007;
        public const string ExcStateInstantiationModeMustBeSetBeforeStartedMessage = "The StateInstantiationMode must be set before the state machine is started!";

        public const int ExcMultipleInitialStatesId = 1008;
        public const string ExcMultipleInitialStatesMessage = "The state machine cannot have multiple states configured as the initial state!";

        public const int ExcStateHasGuardedTransitionAlreadyId = 1009;
        public const string ExcStateHasGuardedTransitionAlreadyMessage =
            "State {0} has at least one guarded transition configured on trigger {1} already. A state cannot have both guardless and guarded transitions at the same time!";

        public const int ExcTriggerAlreadyConfiguredForStateId = 1010;
        public const string ExcTriggerAlreadyConfiguredForStateMessage = "Trigger {0} has already been configured for state {1}!";

        public const int ExcStateHasUnguardedTransitionAlreadyId = 1011;
        public const string ExcStateHasUnguardedTransitionAlreadyMessage =
            "State {0} has an unguarded transition for trigger {1}, you cannot add guarded transitions to this state as well!";

        public const int ExcInvalidTriggerId = 1012;
        public const string ExcInvalidTriggerMessage = "Trigger {0} is not valid for state {1}!";

        public const int ExcMultipleGuardsAreTrueId = 1013;
        public const string ExcMultipleGuardsAreTrueMessage =
            "State {0}, trigger {1} has multiple guard clauses that simultaneously evaulate to True!";

        public const int ExcNoGuardsAreTrueId = 1014;
        public const string ExcNoGuardsAreTrueMessage = "State {0}, trigger {1} has no guard clause that evaulate to True!";

        public const int ExcCannotGoBackWhenParallelId = 1015;
        public const string ExcCannotGoBackWhenParallelMessage = "Cannot go back when executing parallel states!";

        public const int ExcInvalidTriggerForMultipleStatesId = 1016;
        public const string ExcInvalidTriggerForMultipleStatesMessage = "Trigger {0} is not valid for any of the current states: {1}";
    }
}