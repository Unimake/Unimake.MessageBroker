namespace Unimake.MessageBroker.Model
{
    /// <summary>
    /// Retorna os dados da media
    /// </summary>
    public struct MediaResult
    {
        #region Public Properties

        /// <summary>
        /// Conteúdo da media
        /// </summary>
        public byte[] Content { get; set; }

        /// <summary>
        /// Tipo de conteúdo
        /// </summary>
        public string ContentType { get; set; }

        #endregion Public Properties
    }
}