using Unimake.MessageBroker.Primitives.Model.Media;
using Unimake.MessageBroker.Primitives.Model.Messages;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.Messages.WhatsApp
{
    public class SendTextMessageTest(ITestOutputHelper output) : TestBase(output)
    {
        #region Public Methods

        [Fact]
        public async Task SendTextMessage()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp);
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                InstanceName = InstanceName,
                Text = $"Olá! Eu sou uma mensagem de teste 🌜☠️.{Environment.NewLine} Aqui, eu estou em uma nova linha.",
                To = new Primitives.Model.Recipient
                {
                    Destination = "<<DESTINATION>>"
                }
            }, scope);

            DumpAsJson(response);
        }

        [Fact]
        [Trait("feat", "171507")]
        public async Task SendTextMessageWithFiles()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp);
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                InstanceName = InstanceName,
                // Se usar Files (abaixo), pode enviar o Text pelo Caption em Files, ou uma combinação de ambos. Text e Caption
                Text = $"Olá! Eu sou uma mensagem de teste 🌜☠️.{Environment.NewLine} Aqui, eu estou em uma nova linha.",
                To = new Primitives.Model.Recipient
                {
                    Destination = "<<DESTINATION>>"
                },
                Files = [
                    new UploadFile
                    {
                        Base64Content = Convert.ToBase64String(File.ReadAllBytes(@"D:\Temp\erro_osmair.png")),
                        Caption = "Aconteceu este erro no Osmair"
                    }]
            }, scope);

            DumpAsJson(response);
        }

        #endregion Public Methods
    }
}