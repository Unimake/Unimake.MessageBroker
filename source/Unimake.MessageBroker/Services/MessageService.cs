using System;
using System.Threading.Tasks;
using Unimake.AuthServer.Security.Scope;
using Unimake.MessageBroker.Client;
using Unimake.MessageBroker.Model;
using Unimake.MessageBroker.Primitives.Contract.Messages;
using Unimake.MessageBroker.Primitives.Exceptions;
using Unimake.MessageBroker.Primitives.Model.Messages;
using Unimake.MessageBroker.Primitives.Model.Notifications;
using Unimake.MessageBroker.Primitives.Request;
using static Newtonsoft.Json.JsonConvert;

namespace Unimake.MessageBroker.Services
{
    /// <summary>
    /// Serviço mensageiro
    /// </summary>
    public class MessageService
    {
        #region Public Properties

        public string PublicKey { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public MessageService(string publicKey)
        {
            PublicKey = publicKey;
        }

        public MessageService()
        {
        }

        #endregion Public Constructors

        #region Public Methods

        public async Task<MediaResult> DownloadMediaAsync(DownloadMediaRequest request, AuthenticatedScope authenticatedScope)
        {
            var apiClient = new APIClient(authenticatedScope, $"Messages/DownloadMedia", new System.Http.QueryString
            {
                {"MediaId" , request.MediaId} ,
                {"MessagingService"  , (int)request.MessagingService} ,
                {"Testing"  , request.Testing}
            });

            var response = await apiClient.GetAsync();

            if(response.IsSuccessStatusCode)
            {
                var mediaContent = await response.Content.ReadAsByteArrayAsync();
                return new MediaResult
                {
                    Content = mediaContent,
                    ContentType = response.Content.Headers.ContentType.ToString()
                };
            }

            var json = await response.Content.ReadAsStringAsync();
            var errors = ExceptionObject.FromJson(json);
            System.Diagnostics.Debug.WriteLine(errors.Message);
            throw new Exception(errors.Message);
        }

        /// <summary>
        /// Faz a notificação de um boleto recebido
        /// </summary>
        /// <param name="billetNotification">Dados do boleto para compor a mensagem</param>
        /// <param name="authenticatedScope">Escopo autenticado</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MessageResponse> NotifyBilletAsync(BilletNotification billetNotification, AuthenticatedScope authenticatedScope)
        {
            var apiClient = new APIClient(authenticatedScope, $"Messages/NotifyBillet", publicKey: PublicKey);
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

        /// <summary>
        /// Faz a notificação de uma cobrança PIX recebida
        /// </summary>
        /// <param name="notification">Dados da cobrança PIX para compor a mensagem</param>
        /// <param name="authenticatedScope">Escopo autenticado</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<MessageResponse> NotifyPIXCollectionAsync(PIXNotification notification, AuthenticatedScope authenticatedScope)
        {
            var apiClient = new APIClient(authenticatedScope, $"Messages/NotifyPIXCollection", publicKey: PublicKey);
            var response = await apiClient.PostAsync(notification);
            var json = await response.Content.ReadAsStringAsync();

            if(response.IsSuccessStatusCode)
            {
                return DeserializeObject<MessageResponse>(json);
            }

            var errors = ExceptionObject.FromJson(json);
            System.Diagnostics.Debug.WriteLine(errors.Message);
            throw new Exception(errors.Message);
        }

        public async Task<MessageResponse> SendTextMessageAsync(IMessage message, AuthenticatedScope authenticatedScope)
        {
            var apiClient = new APIClient(authenticatedScope, $"Messages/Publish");
            var response = await apiClient.PostAsync(message);
            var json = await response.Content.ReadAsStringAsync();
            System.Diagnostics.Debug.WriteLine(json);

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