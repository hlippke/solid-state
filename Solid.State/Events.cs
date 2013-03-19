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

        private Type _sourceState;
        private Type _targetState;

        // Constructor

        public TransitionedEventArgs(Type sourceState, Type targetState)
        {
            _sourceState = sourceState;
            _targetState = targetState;
        }

        // Properties

        public Type SourceState
        {
            get { return _sourceState; }
        }

        public Type TargetState
        {
            get { return _targetState; }
        }
    }
}