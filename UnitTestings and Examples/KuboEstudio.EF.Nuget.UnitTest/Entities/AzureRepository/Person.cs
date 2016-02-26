using KuboEstudio.EF.Azure.Entities;
using KuboEstudio.EF.Azure.Schema.Attributes;
using KuboEstudio.EF.Schema.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuboEstudio.EF.Nuget.UnitTest.Entities.AzureRepository
{
    [AzureTable("People")]
    internal class Person : AzureItem
    {
        [AzureColumn("ID")]
        public int Identifier { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Address { get; set; }

        [NotAzureColumn]
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        [AzureForeignKey("People", "Identifier")]
        public int? SupervisorId { get; set; }

        [AzureForeignProperty]
        [CascadeOnSave]
        public Person Supervisor { get; set; }

        [AzureChildForeignProperty]
        [CascadeAll]
        public List<Phone> Phones { get; set; }

        [AzureChildForeignProperty]
        [CascadeOnDelete]
        public List<Person> Subordinated { get; set; }
    }
}
