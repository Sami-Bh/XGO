using Microsoft.Datasync.Client;
using XGOMobile.Data;

namespace XGOMobile.Services.Models
{
    internal class RemoteService
    {
        #region Fields

        /// <summary>
        /// Reference to the client used for datasync operations.
        /// </summary>
        private DatasyncClient _client = null;

        /// <summary>
        /// When set to true, the client and table and both initialized.
        /// </summary>
        private bool _initialized = false;

        /// <summary>
        /// When using authentication, the token requestor to use.
        /// </summary>
        public Func<Task<AuthenticationToken>> TokenRequestor;

        /// <summary>
        /// Used for locking the initialization block to ensure only one initialization happens.
        /// </summary>
        private readonly SemaphoreSlim _asyncLock = new(1, 1);
        #endregion

        #region Properties

        #endregion

        #region Constructors
        /// <summary>
        /// Creates a new <see cref="RemoteService"/> with no authentication.
        /// </summary>
        public RemoteService()
        {
            TokenRequestor = null; // no authentication
        }

        /// <summary>
        /// Creates a new <see cref="RemoteService"/> with authentication.
        /// </summary>
        public RemoteService(Func<Task<AuthenticationToken>> tokenRequestor)
        {
            TokenRequestor = tokenRequestor;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Initialize the connection to the remote table.
        /// </summary>
        /// <returns></returns>
        private async Task InitializeAsync()
        {
            // Short circuit, in case we are already initialized.
            if (_initialized)
            {
                return;
            }

            try
            {
                // Wait to get the async initialization lock
                await _asyncLock.WaitAsync();
                if (_initialized)
                {
                    // This will also execute the async lock.
                    return;
                }

                var options = new DatasyncClientOptions
                {
                    HttpPipeline = new HttpMessageHandler[] { new LoggingHandler() }
                };

                // Initialize the client.
                _client = TokenRequestor == null
                    ? new DatasyncClient(Constants.ServiceUri, options)
                    : new DatasyncClient(Constants.ServiceUri, new GenericAuthenticationProvider(TokenRequestor), options);

                //_table = _client.GetRemoteTable<TodoItem>();

                // Set _initialied to true to prevent duplication of locking.
                _initialized = true;
            }
            catch (Exception)
            {
                // Re-throw the exception.
                throw;
            }
            finally
            {
                _asyncLock.Release();
            }
        }

        public async Task<string> GetCategory()
        {
            await InitializeAsync();
            
            return "";
        }
        #endregion
    }
}
