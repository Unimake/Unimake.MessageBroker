using Unimake.MessageBroker.Primitives.Model.Messages;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.Messages.WhatsApp
{
    public class SendTextMessageTest : TestBase
    {
        #region Public Constructors

        public SendTextMessageTest(ITestOutputHelper output)
            : base(output)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public async Task SendTextMessage()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp);
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                InstanceName = "<<instanceName>>",
                Text = $"Olá! Eu sou uma mensagem de teste 🌜☠️.{Environment.NewLine} Aqui, eu estou em uma nova linha.",
                To = new Primitives.Model.Recipient
                {
                    Destination = "<<whats>>"
                }
            }, scope);

            DumpAsJson(response);
        }

        #endregion Public Methods
    }
}