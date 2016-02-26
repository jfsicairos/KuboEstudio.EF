using KuboEstudio.EF.Entities;
using KuboEstudio.EF.Schema.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuboEstudio.EF.Nuget.UnitTest.Entities
{
    internal class UnitTestClass
    {
        public int ID { get; set; }

        public string FirstValue { get; set; }

        [DBColumn("ValueTwo")]
        public string SecondValue { get; set; }
    }
}
