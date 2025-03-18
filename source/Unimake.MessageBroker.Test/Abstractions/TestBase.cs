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

        #region Protected Properties

        protected static string InstanceName => "<<DEFINIR_INSTANCE_NAME>>";

        #endregion Protected Properties

        #region Protected Methods

        protected static async Task<AuthenticatedScope> CreateAuthenticatedScopeAsync() =>
            await new AuthenticationService().AuthenticateAsync(new AuthenticationToken
            {
                AppId = "<<AppId>>",
                Secret = "<<Secret>>"
            });

        protected static void StartServerDebugMode() =>
#if DEBUG_UNIMAKE
           debugScope = new DebugScope<DebugStateObject>(new DebugStateObject
           {
               AuthServerUrl = "https://auth.sandbox.unimake.software/api/auth/",
                AnotherServerUrl = "https://umessenger.sandbox.unimake.software/api/v1/"
           });

#else
            debugScope = null;
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