using System;
using System.Linq;
using Unimake.MessageBroker.Primitives.Contract.Messages;
using Unimake.MessageBroker.Primitives.Model.Notifications;

namespace Unimake.MessageBroker.Validators
{
    /// <summary>
    /// Validações de mensagens para WhatsApp
    /// </summary>
    public static class MessageValidator
    {
        #region Public Methods

        /// <summary>
        /// Valida a mensagem
        /// </summary>
        /// <param name="message"></param>
        public static void Validate(this IMessage message)
        {
            if(message is null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if(!(message.Template?.Components.IsNullOrEmpty() ?? true))
            {
                var first = message.Template.Components.FirstOrDefault(w => w.Type == Primitives.Enumerations.ComponentType.LinkButton);

                if(first != null &&
                    !first.Index.HasValue)
                {
                    throw new ArgumentOutOfRangeException($"O componente do tipo '{first.Type}' exige que a propriedade '{nameof(first.Index)}' seja informada.", default(Exception));
                }
            }
        }

        public static void Validate(this BilletNotification billetNotification)
        {
        }

        #endregion Public Methods
    }
}