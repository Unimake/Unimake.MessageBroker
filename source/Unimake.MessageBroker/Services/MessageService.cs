using EBank.Solutions.Primitives.Exceptions;
using System;
using System.Threading.Tasks;
using Unimake.AuthServer.Security.Scope;
using Unimake.MessageBroker.Client;
using Unimake.MessageBroker.Primitives.Model.Messages;
using Unimake.MessageBroker.Primitives.Model.Notifications;
using static Newtonsoft.Json.JsonConvert;

namespace Unimake.MessageBroker.Services
{
    /// <summary>
    /// Serviço mensageiro
    /// </summary>
    public class MessageService
    {
        #region Public Methods

        /// <summary>
        /// Faz a notificação de um boleto recebido
        /// </summary>
        /// <param name="billetNotification">Dados do boleto para compor a mensagem</param>
        /// <param name="authenticatedScope">Escopo autenticado</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MessageResponse> NotifyBilletAsync(BilletNotification billetNotification, AuthenticatedScope authenticatedScope)
        {
            var apiClient = new APIClient(authenticatedScope, $"Messages/NotifyBillet");
            var response = await apiClient.PostAsync(billetNotification);
            var json = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                return DeserializeObject<MessageResponse>(json);
            }

            var errors = ExceptionObject.FromJson(json);
            System.Diagnostics.Debug.WriteLine(errors.Message);
            throw new Exception(errors.Message);
        }

        #endregion Public Methods
    }
}