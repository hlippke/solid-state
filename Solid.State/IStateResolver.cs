using System;

namespace Solid.State
{
    /// <summary>
    /// Implements a resolver to instantiate states for the SolidMachine.
    /// </summary>
    public interface IStateResolver
    {
        // Methods

        ISolidState ResolveState(Type stateType);
    }
}