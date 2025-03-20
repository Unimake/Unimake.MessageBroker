using System;
using System.Http;
using System.Net;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Unimake.AuthServer.Security.Scope;
using Unimake.Primitives.UDebug;

namespace Unimake.MessageBroker.Client
{
    internal class APIClient : IDisposable
    {
        #region Private Fields

        private readonly AuthenticatedScope authenticatedScope;

        private readonly HttpClient client;

        private QueryString _queryString;

        #endregion Private Fields

        #region Private Properties

        private static DebugStateObject DebugStateObject => DebugScope<DebugStateObject>.Instance?.ObjectState;

        #endregion Private Properties

        #region Private Constructors

        private APIClient()
        {
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.ServerCertificateValidationCallback += CertificateValidationCallback;

            client = new HttpClient
            {
#if DEBUG_UNIMAKE
                Timeout = TimeSpan.FromMinutes(20)
#endif
            };
        }

        #endregion Private Constructors

        #region Private Methods

        private bool CertificateValidationCallback(object sender, X509Certificate cert, X509Chain chain, SslPolicyErrors sslPolicyErrors) => true;

        private void EnsureAuthorization()
        {
            _ = client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", $"{authenticatedScope.Type} {authenticatedScope.Token}");

            if(!string.IsNullOrEmpty(PublicKey))
            {
                _ = client.DefaultRequestHeaders.Remove("U-Public-Key");
                client.DefaultRequestHeaders.Add("U-Public-Key", PublicKey);
            }
        }

        private async Task<HttpResponseMessage> PostAsync(string json)
        {
            EnsureAuthorization();
            return await client.PostAsync(PrepareURI(), new StringContent(json, Encoding.UTF8, "application/json"));
        }

        private string PrepareURI() => $"{DebugStateObject?.AnotherServerUrl ?? $"https://unimake.app/umessenger/api/v1/"}{Action}{ToQueryString()}";

        private string ToQueryString()
        {
            if(QueryString == null)
            {
                return "";
            }

            return QueryString.ToString(urlEncodeValue: false);
        }

        #endregion Private Methods

        #region Public Properties

        public string Action { get; }

        /// <summary>
        /// Chave pública utilizada para analisar o token para download de boletos, quando emitidos pelo e-Bank
        /// </summary>
        public string PublicKey { get; set; }

        public QueryString QueryString => _queryString ?? (_queryString = new QueryString());

        #endregion Public Properties

        #region Public Constructors

        public APIClient(AuthenticatedScope scope, string action, QueryString queryString = null, string publicKey = null)
            : this()
        {
            authenticatedScope = scope ?? throw new ArgumentNullException(nameof(scope));
            Action = action.Trim('/');
            _queryString = queryString;
            PublicKey = publicKey;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            client.Dispose();
            ServicePointManager.ServerCertificateValidationCallback -= CertificateValidationCallback;
        }

        public async Task<HttpResponseMessage> GetAsync()
        {
            EnsureAuthorization();
            return await client.GetAsync(PrepareURI());
        }

        public async Task<HttpResponseMessage> PostAsync(object param) => await PostAsync(Newtonsoft.Json.JsonConvert.SerializeObject(param));

        #endregion Public Methods
    }
}