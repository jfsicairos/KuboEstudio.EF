
using KuboEstudio.EF.Entities;
using KuboEstudio.EF.Schema.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuboEstudio.EF.Nuget.UnitTest.Entities.Repository
{
    [Table("dbo", "Phones")]
    internal class Phone : Item
    {
        [PrimaryKey(), Identity]
        public int ID { get; set; }

        [DBColumn("PhoneNumber")]
        public string Number { get; set; }

        [ForeignKey("dbo", "People", "Identifier")]
        public int PersonId { get; set; }
    }
}
