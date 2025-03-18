using Unimake.MessageBroker.Primitives.Model.Messages;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.Messages.Discord
{
    public class SendTextMessageTest(ITestOutputHelper output) : TestBase(output)
    {
        #region Public Methods

        [Fact]
        public async Task SendMultiLineTextMessage()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.Discord);
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                MultiLineText =
                [
                    "Olá! Eu sou uma mensagem de testes 🌜☠️",
                    "Eu vim do pacote GITHUB",
                    "Eu fui testado com múltiplas linhas"
                ],
                To = new Primitives.Model.Recipient
                {
                    Destination = "<<Destination>>"
                }
            }, scope);

            DumpAsJson(response);
        }

        [Fact]
        public async Task SendTextMessage()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.Discord);
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                Text = "Olá! Eu sou uma mensagem de testes 🌜☠️. \n Eu vim do pacote GITHUB e fui testado com uma linha com quebras",
                To = new Primitives.Model.Recipient
                {
                    Destination = "<<Destination>>"
                }
            }, scope);

            DumpAsJson(response);
        }

        #endregion Public Methods
    }
}