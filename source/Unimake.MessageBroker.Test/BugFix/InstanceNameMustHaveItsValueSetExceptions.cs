using Unimake.MessageBroker.Primitives.Model.Messages;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.BugFix
{
    public class InstanceNameMustHaveItsValueSetExceptions(ITestOutputHelper output) : TestBase(output)
    {
        #region Public Methods

        [Fact]
        public async Task InstanceNameMustHaveItsValueSetAsync()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.Discord);
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                MultiLineText = ["teste"],
                To = new Primitives.Model.Recipient
                {
                    Destination = "<<DISCORD WEBHOOK>>",
                }
            }, scope);
        }

        #endregion Public Methods
    }
}