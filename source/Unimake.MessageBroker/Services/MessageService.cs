﻿using System;
using System.Threading.Tasks;
using Unimake.AuthServer.Security.Scope;
using Unimake.MessageBroker.Client;
using Unimake.MessageBroker.Model;
using Unimake.MessageBroker.Primitives.Contract.Messages;
using Unimake.MessageBroker.Primitives.Enumerations;
using Unimake.MessageBroker.Primitives.Exceptions;
using Unimake.MessageBroker.Primitives.Model.Media;
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
        #region Public Properties

        public string InstanceName { get; }

        public MessagingService MessagingService { get; private set; }

        public string PublicKey { get; private set; }

        #endregion Public Properties

        #region Public Constructors

        public MessageService(MessagingService messagingService, string publicKey)
        {
            PublicKey = publicKey;
            MessagingService = messagingService;
        }

        public MessageService(MessagingService messagingService)
            : this(messagingService, "")
        {
        }

        public MessageService(string instanceName, MessagingService messagingService)
            : this(messagingService, "") => InstanceName = instanceName;

        #endregion Public Constructors

        #region Public Methods

        public async Task<MediaResult> DownloadMediaAsync(DownloadMedia request, AuthenticatedScope authenticatedScope)
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
            billetNotification.MessagingService = MessagingService;
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
            notification.MessagingService = MessagingService;
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

        public async Task<MessageResponse> SendAlertAsync(AlertNotification alert, AuthenticatedScope authenticatedScope)
        {
            alert.MessagingService = MessagingService;
            var apiClient = new APIClient(authenticatedScope, $"Messages/SendAlert");
            apiClient.QueryString.AddOrUpdateValue("instanceName",InstanceName);
            var response = await apiClient.PostAsync(alert);
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

        public async Task<MessageResponse> SendTextMessageAsync(IMessage message, AuthenticatedScope authenticatedScope)
        {
            var instanceName = (message is Message msg) ? $"/{msg.InstanceName}" : string.Empty;

            if(message.MessagingService == MessagingService.WhatsApp &&
                string.IsNullOrWhiteSpace(instanceName))
            {
                throw new ArgumentException($"A propriedade {nameof(msg.InstanceName)} deve ser informada para o serviço WhatsApp.");
            }

            message.MessagingService = MessagingService;
            var apiClient = new APIClient(authenticatedScope, $"Messages/Publish{instanceName}");
            var response = await apiClient.PostAsync(message);
            var json = await response.Content.ReadAsStringAsync();

            System.Diagnostics.Debug.WriteLine(json);

            if(response.IsSuccessStatusCode)
            {
                var result = DeserializeObject<MessageResponse>(json);
                result.StatusCode = (int)response.StatusCode;
                return result;
            }

            var errors = ExceptionObject.FromJson(json);
            System.Diagnostics.Debug.WriteLine(errors.Message);
            throw new Exception(errors.Message);
        }

        #endregion Public Methods
    }
}