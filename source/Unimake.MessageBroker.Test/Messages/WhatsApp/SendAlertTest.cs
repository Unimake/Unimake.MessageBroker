using Unimake.MessageBroker.Primitives.Model.Notifications;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.Messages.WhatsApp
{
    public class SendAlertTest : TestBase
    {
        #region Public Constructors

        public SendAlertTest(ITestOutputHelper output)
            : base(output)
        {
        }

        #endregion Public Constructors

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
                To = "5544991848774",
                Title = "Unimake Buzz"
            }, scope);

            DumpAsJson(response);
        }

        #endregion Public Methods
    }
}