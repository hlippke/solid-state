using System;

namespace Solid.State
{
    /// <summary>
    /// Base exception for the Solid.State framework.
    /// </summary>
    public class SolidStateException : Exception
    {
        // Constructors

        public SolidStateException(int id, string message) : base(message)
        {
            Id = id;
        }

        public SolidStateException(int id, string message, Exception innerException) : base(message, innerException)
        {
            Id = id;
        }

        // Properties

        public int Id { get; private set; }
    }
}