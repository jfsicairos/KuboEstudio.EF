using KuboEstudio.EF.Azure.Entities;
using KuboEstudio.EF.Azure.Schema.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuboEstudio.EF.Nuget.UnitTest.Entities.AzureRepository
{
    [AzureTable("Phones")]
    internal class Phone : AzureItem
    {
        public int ID { get; set; }

        public string Number { get; set; }

        [AzureForeignKey("People", "Identifier")]
        public int PersonId { get; set; }
    }
}
