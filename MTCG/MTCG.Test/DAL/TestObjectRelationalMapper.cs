using MTCG.DAL.ORM;
using MTCG.Http.Method;
using MTCG.Http.Status;
using MTCG.Models;
using Newtonsoft.Json;
using Npgsql;
using NUnit.Framework;
using System;
using System.Linq;

namespace MTCG.Test.DAL {
    [TestFixture]
    public class TestObjectRelationalMapper {

        private ObjectRelationalMapper _orm;
        private User _testUser;

        [SetUp]
        public void SetUp() {
            _orm = new(DALTestSetup.Connection);
            _testUser = new() {
                Id = Guid.NewGuid(),
                UserName = "TestUser1",
                Password = "1234"
            };
        }

        [TearDown]
        public void TearDown() {
            string query = $"DELETE FROM users WHERE id = @id";
            using NpgsqlCommand command = new NpgsqlCommand(query, DALTestSetup.Connection);
            command.Parameters.AddWithValue("@id", _testUser.Id);
            command.ExecuteNonQuery();
        }

        [Test]
        public void InsertingObjectDoesNotThrow() {
            Assert.DoesNotThrow(() => _orm.Insert(_testUser));
        }

        [Test]
        public void UpdatingObjectDoesNotThrow() {
            _orm.Insert(_testUser);
            _testUser.Bio = "Unit tester";
            Assert.DoesNotThrow(() => _orm.Update(_testUser));
        }

        [Test]
        public void SelectingObjectDoesNotThrow() {
            Assert.DoesNotThrow(() => _orm.GetAll<User>());
        }

        [Test]
        public void DeletingObjectDoesNotThrow() {
            _orm.Insert(_testUser);
            Assert.DoesNotThrow(() => _orm.Delete(_testUser));
        }

        [Test]
        public void SelectingReturnsObjects() {
            _orm.Insert(_testUser);
            var res = _orm.GetAll<User>();
            Assert.IsTrue(res.Any());
        }

        [Test]
        public void InsertedObjectCanBeSelected() {
            _orm.Insert(_testUser);
            var res = _orm.GetAll<User>().Where(user => user.Id == _testUser.Id).ToList();
            Assert.AreEqual(1, res.Count);
        }

        [Test]
        public void InsertingAndSelectingObjectDoesNotChangeData() {
            _orm.Insert(_testUser);
            var res = _orm.GetAll<User>().Where(user => user.Id == _testUser.Id).ToList()[0];
            Assert.AreEqual(_testUser.UserName, res.UserName);
            Assert.AreEqual(_testUser.Name, res.Name);
            Assert.AreEqual(_testUser.Password, res.Password);
        }

        [Test]
        public void DeletedObjectCanNotBeSelected() {
            _orm.Insert(_testUser);
            _orm.Delete(_testUser);
            var res = _orm.GetAll<User>().Where(user => user.Id == _testUser.Id).ToList();
            Assert.AreEqual(0, res.Count);
        }

        [Test]
        public void UpdatesToObjectCanBePersisted() {
            _orm.Insert(_testUser);
            _testUser.Bio = "Unit tester";
            _orm.Update(_testUser);
            var res = _orm.GetAll<User>().Where(user => user.Id == _testUser.Id).ToList();
            Assert.AreEqual("Unit tester", res[0].Bio);
        }

        [Test]
        public void UpdatesToObjectChangesVerionNumber() {
            Assert.AreEqual(1, _testUser.Version);
            _orm.Insert(_testUser);
            _testUser.Bio = "Unit tester";
            _orm.Update(_testUser);
            var res = _orm.GetAll<User>().Where(user => user.Id == _testUser.Id).ToList();
            Assert.AreEqual(2, res[0].Version);
        }
    }
}