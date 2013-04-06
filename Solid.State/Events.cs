using System;

namespace Solid.State
{
    /// <summary>
    /// Delegate for the SolidMachine.Transitioned event.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public delegate void TransitionedEventHandler(object sender, TransitionedEventArgs e);

    /// <summary>
    /// EventArgs for the SolidMachine.Transitioned event.
    /// </summary>
    public class TransitionedEventArgs : EventArgs
    {
        // Private variables

        private readonly Type _sourceStateType;
        private readonly ISolidState _targetState;

        // Constructor

        public TransitionedEventArgs(Type sourceStateType, ISolidState targetState)
        {
            _sourceStateType = sourceStateType;
            _targetState = targetState;
        }

        // Properties

        /// <summary>
        /// The source state of the transition.
        /// </summary>
        public Type SourceStateType
        {
            get { return _sourceStateType; }
        }

        /// <summary>
        /// The target state of the transition.
        /// </summary>
        public ISolidState TargetState
        {
            get { return _targetState; }
        }
    }
}