using Unimake.MessageBroker.Primitives.Model.Notifications;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.BugFix
{
    public class Bug157182(ITestOutputHelper output) : TestBase(output)
    {
        #region Public Methods

        [Fact]
        public async Task FixItAsync()
        {
            using var scope = await CreateAuthenticatedScopeAsync();

            var alertNotication = new AlertNotification
            {
                Testing = false,
                Text = "Mensagem de erro.",
                To = DebugScope.GetState().ToPhoneDestination,
                Title = "UNINFE - Erro no envio documentos eletrônicos"
            };

            var messageService = new MessageService(DebugScope.GetState().InstanceName, Primitives.Enumerations.MessagingService.WhatsApp);
            _ = await messageService.SendAlertAsync(alertNotication, scope);
        }

        #endregion Public Methods
    }
}