using EBank.Solutions.Primitives.Security;
using Unimake.MessageBroker.Primitives.Model.Notifications;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test
{
    public class BilletNotificationTest : TestBase
    {
        #region Private Fields

        // A chave gerada é uma chave aleatória gerada no site https://codebeautify.org/hmac-generator
        // Esta chave deve ser informada no cabeçalho  U-Public-Key para que a API saiba qual chave usar
        // A chave é livre, pública e você pode gerar como quiser.
        private const string PublicKey = "25e847391c25263a8d5758c0a416170d321dcfc2ca3148e109bf2675519c70fa";

        #endregion Private Fields

        #region Private Methods

        private string SignLink(long boletoId)
        {
            var claims = new List<(string Key, object Value)>
            {
                    ("minhaClaim", "Oi Claim"),
                    ("billet", boletoId)
                };

            var linkSigned = LinkSigner.SignLink("", "unimake", "billet", claims, PublicKey);

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
                BarCode = "65465464646554456453456453456544565445645345654435",
                BilletNumber = "12345678",
                CompanyName = "Unimake",
                ContactPhone = "DDD<<telefone>>",
                CustomerName = "Marcelo",
                Description = "Descritivo 1 Descritivo 2 N",
                DueDate = "01/01/2022",
                QueryString = linkSigned,
                To = "DDD<<telefone>>",
                Value = "R$ 250,00"
            }, scope);
            DumpAsJson(response);
        }

        #endregion Public Methods
    }
} 