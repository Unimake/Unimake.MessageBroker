using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Unimake.AuthServer.Security.Scope;
using Unimake.Primitives.Security.Credentials;

namespace Unimake.MessageBroker.Services.Security
{
    /// <summary>
    /// Serviço de autenticação
    /// </summary>
    public sealed class AuthenticationService
    {
        #region Public Constructors

        /// <summary>
        /// Instancia um novo objeto
        /// </summary>
        public AuthenticationService()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Realiza a autenticação e retorna o escopo autenticado, se válido
        /// </summary>
        /// <param name="appId">Identificador da aplicação</param>
        /// <param name="secret">Segredo</param>
        /// <returns></returns>
        public AuthenticatedScope Authenticate(string appId, string secret)
        {
            return new AuthenticatedScope(new AuthenticationToken
            {
                AppId = appId,
                Secret = secret
            });
        }

        /// <inheritdoc cref="IAuthenticationService.AuthenticateAsync(AuthenticationToken)"/>
        [ComVisible(false)]
        public async Task<AuthenticatedScope> AuthenticateAsync(AuthenticationToken credentials)
        {
            await Task.CompletedTask;
            return new AuthenticatedScope(credentials);
        }

        #endregion Public Methods
    }
}