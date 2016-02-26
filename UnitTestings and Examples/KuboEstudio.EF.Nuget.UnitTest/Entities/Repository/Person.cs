using KuboEstudio.EF.Entities;
using KuboEstudio.EF.Schema.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuboEstudio.EF.Nuget.UnitTest.Entities.Repository
{
    [Table("dbo", "People")]
    internal class Person : Item
    {
        [PrimaryKey(defaultValue: 0), Identity]
        [DBColumn("ID")]
        public int Identifier { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DBColumn("Address1")]
        public string Address { get; set; }

        [NotDBColumn]
        public string FullName
        {
            get
            {
                return String.Format("{0} {1}", this.FirstName, this.LastName);
            }
        }

        [ForeignKey("dbo", "People", "Identifier")]
        public int? SupervisorId { get; set; }

        [ForeignProperty("SupervisorId")]
        [CascadeOnSave]
        public Person Supervisor { get; set; }

        [ChildForeignProperty]
        [CascadeAll]
        public List<Phone> Phones { get; set; }

        [ChildForeignProperty]
        [CascadeOnDelete]
        public List<Person> Subordinated { get; set; }
    }
}
