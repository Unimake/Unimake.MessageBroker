using Newtonsoft.Json;
using Unimake.AuthServer.Authentication;
using Unimake.AuthServer.Security.Scope;
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
        private static DebugScope<DebugStateObject> debugScope;

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

        #region Protected Methods

        protected static async Task<AuthenticatedScope> CreateAuthenticatedScopeAsync() =>
            await new AuthenticationService().AuthenticateAsync(new AuthenticationRequest
            {
                AppId = "<<appId>>",
                Secret = "<<secretId>>"
            });

#if DEBUG_UNIMAKE

        protected void StartServerDebugMode() => debugScope = new DebugScope<DebugStateObject>(new DebugStateObject
        {
            AuthServerUrl = "https://localhost:44386/api/auth/",
            AnotherServerUrl = "http://localhost:58295/api/v1/"
        });

#else
        protected void StartServerDebugMode() => debugScope = null;
#endif

        #endregion Protected Methods

        #region Public Constructors

        public TestBase(ITestOutputHelper output)
        {
            this.output = output;
            StartServerDebugMode();
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            debugScope?.Dispose();
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