using Unimake.MessageBroker.Primitives.Model.Messages;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test
{
    public class SendTextMessageTest : TestBase
    {
        #region Public Constructors

        public SendTextMessageTest(ITestOutputHelper output)
            : base(output)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public async Task SendTextMessage()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService();
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                Text = "Olá! Eu sou uma mensagem de teste 🌜☠️",
                To = new Primitives.Model.Recipient
                {
                    Destination = "5544991848774"
                }
            }, scope);

            DumpAsJson(response);   
        }

        #endregion Public Methods
    }
}