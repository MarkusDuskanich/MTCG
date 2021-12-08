using MTCG.Client;
using MTCG.Http.Method;
using MTCG.Http.Status;
using MTCG.Server;
using NUnit.Framework;
using Npgsql;

namespace MTCG.Test.DAL {
    [SetUpFixture]
    public class DALTestSetup {
        public static NpgsqlConnection Connection { get; private set; }

        [OneTimeSetUp]
        public void OpenDBConnention() {
            Connection = new NpgsqlConnection("Host=localhost;Username=postgres;Password=postgres;Database=mtcgdb;IncludeErrorDetail=true");
            Connection.Open();
        }

        [OneTimeTearDown]
        public void CloseDBConnection() {
            Connection.Dispose();
        }
    }
}
