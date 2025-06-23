using Unimake.MessageBroker.Test.Scope;
using Unimake.Primitives.UDebug;

namespace Unimake
{
    internal static class DebugStateObjectExtensions
    {
        #region Public Methods

        public static DebugState GetState(this DebugScope<DebugStateObject> state) => (DebugState)state.ObjectState.State;

        #endregion Public Methods
    }
}