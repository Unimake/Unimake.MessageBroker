using System;
using System.Http;
using System.Net.Http;
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

        private readonly HttpClient client = new HttpClient();

        private QueryString _queryString;

        #endregion Private Fields

        #region Private Properties

        private static DebugStateObject debugStateObject => DebugScope<DebugStateObject>.Instance?.ObjectState;

        #endregion Private Properties

        #region Private Methods

        private void EnsureAuthorization()
        {
            client.DefaultRequestHeaders.Remove("Authorization");
            client.DefaultRequestHeaders.Add("Authorization", $"{authenticatedScope.Type} {authenticatedScope.Token}");

            if(!string.IsNullOrEmpty(PublicKey))
            {
                client.DefaultRequestHeaders.Remove("U-Public-Key");
                client.DefaultRequestHeaders.Add("U-Public-Key", PublicKey);
            }
        }

        private async Task<HttpResponseMessage> PostAsync(string json)
        {
            EnsureAuthorization();
            return await client.PostAsync(PrepareURI(), new StringContent(json, Encoding.UTF8, "application/json"));
        }

        private string PrepareURI()
        {
            return $"{debugStateObject?.AnotherServerUrl ?? $"https://unimake.app/messenger/api/v1/"}{Action}{ToQueryString()}";
        }

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

        public QueryString QueryString { get => _queryString ?? (_queryString = new QueryString()); }

        #endregion Public Properties

        #region Public Constructors

        public APIClient(AuthenticatedScope scope, string action, QueryString queryString = null, string publicKey = null)
        {
            authenticatedScope = scope ?? throw new ArgumentNullException(nameof(scope));
            Action = action;
            _queryString = queryString;
            PublicKey = publicKey;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Dispose()
        {
            client.Dispose();
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