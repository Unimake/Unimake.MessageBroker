using Unimake.MessageBroker.Primitives.Model;

namespace Unimake.MessageBroker.Test.Scope
{
    /// <summary>
    /// Estrutura que armazena o estado de depuração, incluindo credenciais e destinatário.
    /// </summary>
    internal struct DebugState
    {
        #region Public Fields

        /// <summary>
        /// Identificador do aplicativo obtido das variáveis de ambiente.
        /// </summary>
        public string AppId = Environment.GetEnvironmentVariable($"UNIMAKE_APPKEY");
        /// <summary>
        /// Representa o nome da instância do Message Broker, obtido das variáveis de ambiente.
        /// </summary>
        public string InstanceName = Environment.GetEnvironmentVariable($"{nameof(MessageBroker)}_{nameof(InstanceName)}");
        /// <summary>
        /// Representa a chave pública do Message Broker, obtida das variáveis de ambiente.
        /// </summary>
        public string PublicKey = Environment.GetEnvironmentVariable($"{nameof(MessageBroker)}_{nameof(PublicKey)}");
        /// <summary>
        /// Segredo do aplicativo obtido das variáveis de ambiente.
        /// </summary>
        public string Secret = Environment.GetEnvironmentVariable($"UNIMAKE_SECRET");
        /// <summary>
        /// Destinatário padrão para mensagens do Discord.
        /// </summary>
        public Recipient ToDiscordDestination = new() { Destination = "<<ENDPOINT DO CANAL DISCORD>>" };
        /// <summary>
        /// Destinatário padrão para testes.
        /// </summary>
        public Recipient ToPhoneDestination = new() { Destination = "+5544991848774" };

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Inicializa uma nova instância da estrutura <see cref="DebugState"/>.
        /// </summary>
        public DebugState()
        {
        }

        #endregion Public Constructors
    }
}