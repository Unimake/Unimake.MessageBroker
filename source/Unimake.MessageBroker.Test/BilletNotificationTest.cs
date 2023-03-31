using Unimake.AuthServer.Authentication;
using Unimake.MessageBroker.Primitives.Model.Notifications;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;
using AuthenticationService = Unimake.MessageBroker.Services.Security.AuthenticationService;

namespace Unimake.MessageBroker.Test
{
    public class BilletNotificationTest : TestBase
    {
        #region Public Constructors

        public BilletNotificationTest(ITestOutputHelper output)
            : base(output)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public async Task NotifyBillet()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService();
            var response = await service.NotifyBilletAsync(new BilletNotification
            {
                BarCode = "65465464646554456453456453456544565445645345654435",
                BilletNumber = "12345678",
                CompanyName = "Unimake",
                ContactPhone = "DDD<<telefone>>",
                CustomerName = "Marcelo",
                Description = "Descritivo 1 Descritivo 2 N",
                DueDate = "01/01/2022",
                QueryString = "code=<<Use o link signer para gerar um código válido>>",
                To = "DDD<<telefone>>",
                Value = "R$ 250,00"
            }, scope);
            DumpAsJson(response);
        }

        #endregion Public Methods
    }
}