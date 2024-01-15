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

        private static DebugScope<DebugStateObject> debugScope;
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

        protected const string PublicKey = "dDd3IXolQypGLUphTmRSZ1VqWG4ycjV1OHgvQT9EKEc=";

        #endregion Protected Fields

        #region Protected Methods

        protected static async Task<AuthenticatedScope> CreateAuthenticatedScopeAsync() =>
            await new AuthenticationService().AuthenticateAsync(new AuthenticationRequest
            {
                AppId = "<<AppId>>",
                Secret = "<<Secret>>"
            });

        protected void StartServerDebugMode()
        {
#if DEBUG_UNIMAKE
            debugScope = new DebugScope<DebugStateObject>(new DebugStateObject
            {
                AuthServerUrl = "http://homolog.unimake.software:54469/api/auth/",
                AnotherServerUrl = "http://192.168.1.56:58295/api/v1/"
            });

#else
            debugScope = null;
#endif
        }

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