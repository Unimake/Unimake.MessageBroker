using Unimake.MessageBroker.Primitives.Model.Media;
using Unimake.MessageBroker.Primitives.Model.Messages;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.BugFix
{
    public class Bug175465(ITestOutputHelper output) : TestBase(output)
    {
        #region Public Methods

        [Fact]
        public async Task Fix_Request_Entity_Too_Large()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp);
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                InstanceName = DebugScope.GetState().InstanceName,
                Text = $"Olá! Eu sou uma mensagem de teste 🌜☠️.{Environment.NewLine} Aqui, eu estou em uma nova linha.",
                To = DebugScope.GetState().ToPhoneDestination,
                Files = [
                    new UploadFile
                    {
                        // Este arquivo tem 4mb
                        Base64Content = Convert.ToBase64String(File.ReadAllBytes(@"D:\Temp\ANEXO I - Leiaute e Regra de Validação - NF-e e NFC-e.pdf")),
                        Caption = "Veja este arquivo"
                    }]
            }, scope);

            DumpAsJson(response);
        }

        #endregion Public Methods
    }
}