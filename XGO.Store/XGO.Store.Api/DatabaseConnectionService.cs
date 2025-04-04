namespace XGO.Store.Api
{
    public class DatabaseConnectionService
    {
        #region Fields
        IConfiguration _configuration;
        #endregion

        #region Properties

        #endregion

        #region Constructors

        public DatabaseConnectionService(IConfiguration configuration)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }
        #endregion

        #region Methods
        public string GetConnectionString()
        {
            return "";
        }
        #endregion
    }
}
