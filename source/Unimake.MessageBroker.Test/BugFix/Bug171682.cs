using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unimake.MessageBroker.Primitives.Model.Media;
using Unimake.MessageBroker.Primitives.Model.Messages;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.BugFix
{
    public class Bug171682(ITestOutputHelper output) : TestBase(output)
    {
        [Fact]
        [Trait("bug", "171682")]
        public async Task FixItAsync()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp);
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                InstanceName = InstanceName,
                Text = $"Olá! Eu sou uma mensagem de teste 🌜☠️.{Environment.NewLine} Aqui, eu estou em uma nova linha.",
                To = new Primitives.Model.Recipient
                {
                    Destination = "5544991285862"
                },
                Files = [
                    new UploadFile
                    {
                        Base64Content = Convert.ToBase64String(File.ReadAllBytes(@"D:\Temp\imagem_1.png")),
                        Caption = "Veja esta imagem"
                    },
                    new UploadFile
                    {
                        Base64Content = Convert.ToBase64String(File.ReadAllBytes(@"D:\Temp\imagem_2.png")),
                        Caption = "Veja esta imagem"
                    }]
            }, scope);

            DumpAsJson(response);
        }

        [Fact]
        [Trait("bug", "171682")]
        public async Task FixCaptionNullAsync()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp);
            var response = await service.SendTextMessageAsync(new TextMessage
            {
                InstanceName = InstanceName,
                Text = $"Olá! Eu sou uma mensagem de teste 🌜☠️.{Environment.NewLine} Aqui, eu estou em uma nova linha.",
                To = new Primitives.Model.Recipient
                {
                    Destination = "5544991285862"
                },
                Files = [
                    new UploadFile
                    {
                        Base64Content = Convert.ToBase64String(File.ReadAllBytes(@"D:\Temp\imagem_1.png"))
                    },
                    new UploadFile
                    {
                        Base64Content = Convert.ToBase64String(File.ReadAllBytes(@"D:\Temp\imagem_2.png")),
                        Caption = "Veja esta imagem"
                    }]
            }, scope);

            DumpAsJson(response);
        }
    }
}
