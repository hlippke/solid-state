using System;

namespace Solid.State
{
    /// <summary>
    /// Base exception for the Solid.State framework.
    /// </summary>
    public class SolidStateException : Exception
    {
        public SolidStateException()
        {
        }

        public SolidStateException(string message) : base(message)
        {
        }

        public SolidStateException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}