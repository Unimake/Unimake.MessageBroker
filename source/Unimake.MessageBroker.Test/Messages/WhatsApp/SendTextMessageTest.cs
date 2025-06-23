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
                InstanceName = DebugScope.GetState().InstanceName,
                Text = $"Olá! Eu sou uma mensagem de teste 🌜☠️.{Environment.NewLine} Aqui, eu estou em uma nova linha.",
                To = DebugScope.GetState().ToPhoneDestination,
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
                InstanceName = DebugScope.GetState().InstanceName,
                // Se usar Files (abaixo), pode enviar o Text pelo Caption em Files, ou uma combinação de ambos. Text e Caption
                Text = $"Olá! Eu sou uma mensagem de teste 🌜☠️.{Environment.NewLine} Aqui, eu estou em uma nova linha.",
                To = DebugScope.GetState().ToPhoneDestination,
                Files = [
                    new UploadFile
                    {
                        Base64Content = "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",
                        Caption = "Veja esta imagem"
                    }]
            }, scope);

            DumpAsJson(response);
        }

        #endregion Public Methods
    }
}