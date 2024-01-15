using EBank.Solutions.Primitives.Security;
using Unimake.MessageBroker.Primitives.Model.Notifications;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.Billet
{
    public class BilletNotificationTest : TestBase
    {
        #region Private Fields

        #endregion Private Fields

        #region Private Methods

        private string SignLink(long boletoId)
        {
            var claims = new List<(string Key, object Value)>
            {
                    ("minhaClaim", "Oi Claim"),
                    ("billet", boletoId)
                };

            var linkSigned = LinkSigner.SignLink("unimake", "billet", claims, PublicKey);

            return linkSigned;
        }

        #endregion Private Methods

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
            var service = new MessageService(PublicKey);
            var linkSigned = SignLink(123456);
            var response = await service.NotifyBilletAsync(new BilletNotification
            {
                BarCode = "46354546534546345463454365454365454365454365454",
                BilletNumber = "00001",
                CompanyName = "fulano",
                ContactPhone = "5511111111111",
                CustomerName = "Marcelo",
                Description = "Mensalidade R$ 250,00",
                DueDate = "10/12/2023",
                QueryString = linkSigned,
                To = "5511111111112",
                Value = "R$ 250,00",
                Testing = false
            }, scope);

            DumpAsJson(response);
        }

        #endregion Public Methods
    }
}