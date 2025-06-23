using Newtonsoft.Json;
using Unimake.AuthServer.Security.Scope;
using Unimake.Primitives.Security.Credentials;
using Unimake.Primitives.UDebug;
using Xunit.Abstractions;
using static Newtonsoft.Json.JsonConvert;
using AuthenticationService = Unimake.MessageBroker.Services.Security.AuthenticationService;

namespace Unimake.MessageBroker.Test.Abstractions
{
    public abstract class TestBase : IDisposable
    {
        #region Private Fields

        private readonly ITestOutputHelper output;
        private JsonSerializerSettings _jsonSettings;

        #endregion Private Fields

        #region Private Properties

        private JsonSerializerSettings JsonSettings => _jsonSettings ??= _jsonSettings = new JsonSerializerSettings
        {
            Formatting = Formatting.Indented,
            MissingMemberHandling = MissingMemberHandling.Ignore,
            NullValueHandling = NullValueHandling.Include,
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
        };

        #endregion Private Properties

        #region Protected Fields



        #endregion Protected Fields

        #region Protected Properties

        
        #endregion Protected Properties

        #region Protected Methods

        protected async Task<AuthenticatedScope> CreateAuthenticatedScopeAsync() =>
            await new AuthenticationService().AuthenticateAsync(new AuthenticationToken
            {
                AppId = DebugScope.GetState().AppId,
                Secret = DebugScope.GetState().Secret
            });

        protected void StartDebugMode() =>
           DebugScope = new DebugScope<DebugStateObject>(new DebugStateObject
           {
               AuthServerUrl = "https://auth.sandbox.unimake.software/api/auth/",
               AnotherServerUrl = "https://umessenger.sandbox.unimake.software/api/v1/",
               State = new Scope.DebugState()
           });

        #endregion Protected Methods

        #region Public Fields

        public DebugScope<DebugStateObject> DebugScope;

        #endregion Public Fields

        #region Public Constructors

        public TestBase(ITestOutputHelper output)
        {
            this.output = output;
            StartDebugMode();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            DebugScope?.Dispose();
            GC.SuppressFinalize(this);
        }

        public void DumpAsJson(object value)
        {
            var text = SerializeObject(value, JsonSettings);
            output.WriteLine(text);
            System.Diagnostics.Debug.WriteLine(text, "MessageBrokerDebug");
        }

        #endregion Public Methods
    }
}