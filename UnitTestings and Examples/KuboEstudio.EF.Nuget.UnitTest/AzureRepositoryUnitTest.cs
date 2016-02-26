using KuboEstudio.EF.Azure;
using KuboEstudio.EF.Azure.Entities;
using KuboEstudio.EF.Enums;
using KuboEstudio.EF.Interfaces;
using KuboEstudio.EF.Azure.Resources;
using KuboEstudio.EF.Azure.Schema.Attributes;
using KuboEstudio.EF.Nuget.UnitTest.Entities.AzureRepository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KuboEstudio.EF.Entities;

namespace KuboEstudio.EF.Nuget.UnitTest
{
    [TestClass]
    public class AzureRepositoryUnitTest
    {
        private void CleanTables()
        {
            using (IUnitOfWork unitOfWork = new AzureUnitOfWork("TestAzureStorage"))
            {
                using (IRepository<Person> repo = new AzureRepository<Person>(unitOfWork))
                {
                    foreach (var item in repo.All)
                        repo.Delete(item);
                }

                using (IRepository<Phone> repo = new AzureRepository<Phone>(unitOfWork))
                {
                    foreach (var item in repo.All)
                        repo.Delete(item);
                }
            }
        }

        [TestMethod]
        public void Save()
        {
            CleanTables();

            Person person = new Person
            {
                PartitionKey = "Person",
                RowKey = "1",
                Identifier = 2,
                FirstName = "Person",
                LastName = "One",
                Address = "123 ST, City, STATE 00000",
                Phones = new List<Phone>() {
                    new Phone {
                        PartitionKey = "Phone",
                        RowKey = "1",
                        ID = 1,
                        Number = "123-456-7890" 
                    }
                },
                Supervisor = new Person
                {
                    PartitionKey = "Person",
                    RowKey = "2",
                    Identifier = 1,
                    FirstName = "Supervisor",
                    LastName = "One"
                }
            };

            using (IUnitOfWork unitOfWork = new AzureUnitOfWork("TestAzureStorage"))
            using (IRepository<Person> repo = new AzureRepository<Person>(unitOfWork))
            {
                Assert.AreEqual<bool>(true, repo.Save(person));
            }
        }

        [TestMethod]
        public void Find()
        {
            using (IUnitOfWork unitOfWork = new AzureUnitOfWork("TestAzureStorage"))
            using (IRepository<Person> repo = new AzureRepository<Person>(unitOfWork))
            {
                Person person = repo.Find(new object[] { "Person", "1" });

                Assert.AreNotEqual(null, person);
                Assert.AreEqual<int>(2, person.Identifier);
                Assert.AreEqual<string>("Person", person.FirstName);
                Assert.AreEqual<string>("One", person.LastName);
                Assert.AreEqual<string>("123 ST, City, STATE 00000", person.Address);
                Assert.AreEqual<int?>(1, person.SupervisorId);

                person.AzureInclude(unitOfWork, p => p.Supervisor);
                Assert.AreNotEqual(null, person.Supervisor);
                Assert.AreEqual<int>(1, person.Supervisor.Identifier);
                Assert.AreEqual<string>("Supervisor", person.Supervisor.FirstName);
                Assert.AreEqual<string>("One", person.Supervisor.LastName);
                Assert.AreEqual<string>(null, person.Supervisor.Address);

                person.Supervisor.AzureInclude(unitOfWork, s => s.Subordinated);
                Assert.AreNotEqual(null, person.Supervisor.Subordinated);
                Assert.AreEqual<int>(1, person.Supervisor.Subordinated.Count);
                Assert.AreEqual<int>(2, person.Supervisor.Subordinated.First().Identifier);
                Assert.AreEqual<string>("Person", person.Supervisor.Subordinated.First().FirstName);
                Assert.AreEqual<string>("One", person.Supervisor.Subordinated.First().LastName);
                Assert.AreEqual<string>("123 ST, City, STATE 00000", person.Supervisor.Subordinated.First().Address);

                person = repo.FindWithIncluding(new object[] { "Person", "1" }, p => p.Phones);
                Assert.AreNotEqual(null, person.Phones);
                Assert.AreEqual<int>(1, person.Phones.Count);
                Assert.AreEqual<int>(1, person.Phones.First().ID);
                Assert.AreEqual<string>("123-456-7890", person.Phones.First().Number);
                Assert.AreEqual<int>(2, person.Phones.First().PersonId);
            }
        }

        [TestMethod]
        public void All()
        {
            using (IUnitOfWork unitOfWork = new AzureUnitOfWork("TestAzureStorage"))
            using (IRepository<Person> repo = new AzureRepository<Person>(unitOfWork))
            {
                Assert.AreEqual<int>(2, repo.All.Count);
            }
        }

        [TestMethod]
        public void Count()
        {
            using (IUnitOfWork unitOfWork = new AzureUnitOfWork("TestAzureStorage"))
            using (IRepository<Person> repo = new AzureRepository<Person>(unitOfWork))
            {
                int total = repo.Count(new Restriction("LastName", Condition.Eq, "One"));
                Assert.AreEqual<int>(2, total);
            }
        }

        [TestMethod]
        public void Where()
        {
            using (IUnitOfWork unitOfWork = new AzureUnitOfWork("TestAzureStorage"))
            using (IRepository<Person> repo = new AzureRepository<Person>(unitOfWork))
            {
                List<Person> people = repo.Where(new Restriction("FirstName", Condition.Eq, "Person"), new Restriction("LastName", Condition.Eq, "Two"));

                Assert.AreNotEqual(null, people);
                Assert.AreEqual<int>(0, people.Count);

                people = repo.Where(new Restriction("FirstName", Condition.Eq, "Person"), new Restriction("LastName", Condition.Eq, "One"));

                Assert.AreNotEqual(null, people);
                Assert.AreEqual<int>(1, people.Count);

                Assert.AreEqual<int>(2, people.First().Identifier);
                Assert.AreEqual<string>("Person", people.First().FirstName);
                Assert.AreEqual<string>("One", people.First().LastName);
                Assert.AreEqual<string>("123 ST, City, STATE 00000", people.First().Address);
            }
        }

        [TestMethod]
        public void Delete()
        {
            using (IUnitOfWork unitOfWork = new AzureUnitOfWork("TestAzureStorage"))
            using (IRepository<Person> repo = new AzureRepository<Person>(unitOfWork))
            {
                Person person = repo.Find(new AzurePrimaryKey("Person", "2" ));
                Assert.AreEqual<bool>(true, repo.Delete(person));
            }
        }

        [TestMethod]
        public void SaveInMemory()
        {
            CleanTables();

            Person person1 = new Person
            {
                PartitionKey = "Person",
                RowKey = "1",
                Identifier = 2,
                FirstName = "Person",
                LastName = "One",
                Address = "123 ST, City, STATE 00000",
                Phones = new List<Phone>() {
                    new Phone { 
                        PartitionKey = "Phone",
                        RowKey = "1",
                        ID = 1,
                        Number = "123-456-7890" 
                    }
                }
            };

            Person person2 = new Person
            {
                PartitionKey = "Person",
                RowKey = "2",
                Identifier = 1,
                FirstName = "Person",
                LastName = "Two"
            };

            using (IUnitOfWork unitOfWork = new AzureUnitOfWork("TestAzureStorage"))
            using (IRepository<Person> repo = new AzureRepository<Person>(unitOfWork))
            {
                repo.SaveInMemory(person1);
                repo.SaveInMemory(person2);

                List<ChangesOnMemory> errors;
                unitOfWork.Commit(out errors);

                Assert.AreEqual<int>(0, errors.Count);
            }
        }

        [TestMethod]
        public void DeleteOnMemory()
        {
            using (IUnitOfWork unitOfWork = new AzureUnitOfWork("TestAzureStorage"))
            using (IRepository<Person> repo = new AzureRepository<Person>(unitOfWork))
            {
                //This use only the value of the RowKey
                Person person1 = repo.Find(1);

                repo.DeleteInMemory(person1);

                //This use only the value of the RowKey
                repo.DeleteInMemory(2);

                List<ChangesOnMemory> errors;
                unitOfWork.Commit(out errors);

                Assert.AreEqual<int>(0, errors.Count);
            }

            CleanTables();
        }
    }
}
