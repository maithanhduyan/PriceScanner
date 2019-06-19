using FirebirdSql.Data.FirebirdClient;


namespace Syn2DB
{
    public sealed class OldDatabase
    {
        // Singleton Class
        private static OldDatabase instance = null;
        private static readonly object padlock = new object();
        private FbConnection connection = null;

        #region Constructor
        public OldDatabase()
        {
        }

        public static OldDatabase Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new OldDatabase();
                    }
                    return instance;
                }
            }
        }
        #endregion

        public FbConnection Connection
        {
            get {
                this.connection = new FbConnection(ConnectionUtils.OldDBConStr);
                return this.connection;
            }
        }

    }
}
