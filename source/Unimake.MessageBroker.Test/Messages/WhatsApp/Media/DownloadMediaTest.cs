using System.Diagnostics;
using Unimake.MessageBroker.Primitives.Request;
using Unimake.MessageBroker.Services;
using Unimake.MessageBroker.Test.Abstractions;
using Xunit;
using Xunit.Abstractions;

namespace Unimake.MessageBroker.Test.Messages.WhatsApp.Media
{
    public class DownloadMediaTest : TestBase
    {
        #region Public Constructors

        public DownloadMediaTest(ITestOutputHelper output)
                    : base(output)
        {
        }

        #endregion Public Constructors

        #region Public Methods

        [Fact]
        public async Task DownloadMedia()
        {
            using var scope = await CreateAuthenticatedScopeAsync();
            var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp);
            var response = await service.DownloadMediaAsync(new DownloadMediaRequest
            {
                MessagingService = Primitives.Enumerations.MessagingService.WhatsApp,
                MediaId = "1231894790804182",
                Testing = true
            }, scope);

            var fi = new FileInfo("image.jpg");
            File.WriteAllBytes(fi.FullName, response.Content);
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = fi.FullName,
                    UseShellExecute = true
                }
            };
            process.Start();
            File.Delete(fi.FullName);
        }

        [Fact]
        public async Task DownloadMediaException()
        {
            await Assert.ThrowsAsync<Exception>(async () =>
            {
                using var scope = await CreateAuthenticatedScopeAsync();
                var service = new MessageService(Primitives.Enumerations.MessagingService.WhatsApp);
                var response = await service.DownloadMediaAsync(new DownloadMediaRequest
                {
                    MessagingService = Primitives.Enumerations.MessagingService.WhatsApp,
                    MediaId = "aaaaabbbbb00000",
                    Testing = true
                }, scope);
            });
        }

        #endregion Public Methods
    }
}