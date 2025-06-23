using Unimake.MessageBroker.Primitives.Model.Notifications;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.Messages.WhatsApp
{
    public class SendAlertTest(ITestOutputHelper output) : TestBase(output)
    {

        #region Public Methods

        [Fact]
        public async Task SendAlert()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp);
            var response = await service.SendAlertAsync(new AlertNotification
            {
                Testing = true,
                Text = "Ao infinito e além. 🚀",
                To = DebugScope.GetState().ToPhoneDestination,
                Title = "Unimake Buzz"
            }, scope);

            DumpAsJson(response);
        }

        #endregion Public Methods

    }
}