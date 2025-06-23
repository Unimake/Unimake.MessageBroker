using EBank.Solutions.Primitives.Security;
using Unimake.MessageBroker.Primitives.Model.Notifications;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.Messages.WhatsApp.PIX
{
    public class PIXNotificationTest(ITestOutputHelper output) : TestBase(output)
    {
        #region Private Methods

        private static string SignLink(string qrCode)
        {
            var claims = new List<(string Key, object Value)>
            {
                    ("minhaClaim", "Oi Claim"),
                    ("qrCode", qrCode)
                };

            var linkSigned = LinkSigner.SignLink("unimake", "PIX", claims, "123456");

            return linkSigned;
        }

        #endregion Private Methods

        #region Public Methods

        [Fact]
        public async Task NotifyPIX()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp, DebugScope.GetState().PublicKey);
            var copyAndPaste = "00020101021226860014BR.GOV.BCB.PIX2564qrpix.bradesco.com.br/qr/v2/92cff44af-f9a2-4f84-94a6-bb2611a0ded5520406546546424354041.005802BR5925UNIMAKE SOFTWARE***6304F43E";
            var linkSigned = SignLink(copyAndPaste);
            var response = await service.NotifyPIXCollectionAsync(new PIXNotification
            {
                CopyAndPaste = copyAndPaste,
                CompanyName = "Unimake",
                ContactPhone = "5544991538368",
                CustomerName = "Marcelo",
                Description = "Melhor churrasqueiro do mundo 🍖",
                IssuedDate = "31/12/2050",
                QueryString = linkSigned,
                To = DebugScope.GetState().ToPhoneDestination,
                Value = "R$ 250,00",
                Testing = true
            }, scope);

            DumpAsJson(response);
        }

        #endregion Public Methods
    }
}