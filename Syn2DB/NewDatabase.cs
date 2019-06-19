using FirebirdSql.Data.FirebirdClient;

namespace Syn2DB
{
    public sealed class NewDatabase
    {
        // Singleton Class
        private static NewDatabase instance = null;
        private static readonly object padlock = new object();
        private FbConnection connection = null;

        #region Constructor
        public NewDatabase() { }

        public static NewDatabase Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new NewDatabase();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public FbConnection Connection
        {
            get
            {
                this.connection = new FbConnection(ConnectionUtils.NewDBConStr);
                return this.connection;
            }
        }
    }
}
