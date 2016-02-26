using KuboEstudio.EF.Entities;
using KuboEstudio.EF.Enums;
using KuboEstudio.EF.Resources;
using KuboEstudio.EF.Schema.Attributes;
using KuboEstudio.EF.Nuget.UnitTest.Entities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KuboEstudio.EF.Nuget.UnitTest
{
    [TestClass]
    public class QueryUnitTest
    {
        [TestMethod]
        public void InputParameters()
        {
            string result1 = null;
            string result2 = null;

            Parameter param1 = new Parameter("@Input1", "Input Value 1", System.Data.DbType.String, ParamType.In);
            Parameter param2 = new Parameter("@Input2", "Input Value 2", System.Data.DbType.String, ParamType.In);

            try
            {
                result1 = Query.ExecuteScalar<string>(@"SELECT ISNULL(@Input1, '') + ' - ' + ISNULL(@Input2, '')", new Parameter[] { param1, param2 });
            }
            catch { }

            try
            {
                result2 = Query.ExecuteScalar<string>(@"SELECT ISNULL(@Input1, '') + ' - ' + ISNULL(@Input2, '')", param1, param2);
            }
            catch { }

            Assert.AreEqual<string>("Input Value 1 - Input Value 2", result1);
            Assert.AreEqual<string>("Input Value 1 - Input Value 2", result2);
        }

        [TestMethod]
        public void ExecuteScalar()
        {
            string result1 = null;
            string result2 = null;

            try
            {
                result1 = Query.ExecuteScalar<string>("SELECT 'Test Scalar'");
            }
            catch { }

            try
            {
                result2 = (string)Query.ExecuteScalar("SELECT 'Test Scalar'");
            }
            catch { }

            Assert.AreEqual<string>("Test Scalar", result1);
            Assert.AreEqual<string>("Test Scalar", result2);
        }

        [TestMethod]
        public void ExecuteDataSet()
        {
            DataSet result = null;

            try
            {
                result = Query.ExecuteDataSet(@"SELECT 'Test Table 1' AS Value
                                                SELECT 'Test Table 2' AS Value");
            }
            catch { }

            Assert.AreEqual<string>("Test Table 1", result.Tables[0].Rows[0][0].ToString());
            Assert.AreEqual<string>("Test Table 2", result.Tables[1].Rows[0][0].ToString());
        }

        [TestMethod]
        public void ExecuteDataTable()
        {
            DataTable result = null;

            try
            {
                result = Query.ExecuteDataTable("SELECT 'Test Table' AS Value");
            }
            catch { }

            Assert.AreEqual<string>("Test Table", result.Rows[0][0].ToString());
        }

        [TestMethod]
        public void ExecuteDictionary()
        {
            Dictionary<int, string> result = new Dictionary<int, string>();

            try
            {
                result = Query.ExecuteDictionary<Dictionary<int, string>>(@"SELECT 1 AS [Key], 'One' AS [Value]
                                                                            UNION SELECT 2, 'Two'");
            }
            catch { }

            Assert.AreEqual<int>(2, result.Keys.Count);

            Assert.AreEqual<int>(1, result.Keys.ToArray()[0]);
            Assert.AreEqual<int>(2, result.Keys.ToArray()[1]);

            Assert.AreEqual<string>("One", result[1]);
            Assert.AreEqual<string>("Two", result[2]);
        }

        [TestMethod]
        public void Execute()
        {
            List<UnitTestClass> result = new List<UnitTestClass>();

            try
            {
                result = Query.Execute<UnitTestClass>(@"SELECT 1 AS ID, 'Value 1-1' AS FirstValue, 'Value 1-2' AS SecondValue
                                                        UNION SELECT 2, 'Value 2-1', 'Value 2-2'");
            }
            catch { }

            Assert.AreEqual<int>(2, result.Count);

            Assert.AreEqual<int>(1, result[0].ID);
            Assert.AreEqual<string>("Value 1-1", result[0].FirstValue);
            Assert.AreEqual<string>("Value 1-2", result[0].SecondValue);

            Assert.AreEqual<int>(2, result[1].ID);
            Assert.AreEqual<string>("Value 2-1", result[1].FirstValue);
            Assert.AreEqual<string>("Value 2-2", result[1].SecondValue);
        }

        [TestMethod]
        public void ExecuteSingle()
        {
            UnitTestClass result = null;

            try
            {
                result = Query.ExecuteSingle<UnitTestClass>("SELECT 3 AS ID, 'Value 3-1' AS FirstValue, 'Value 3-2' AS ValueTwo");
            }
            catch { }

            Assert.AreNotEqual(null, result);

            Assert.AreEqual<int>(3, result.ID);
            Assert.AreEqual<string>("Value 3-1", result.FirstValue);
            Assert.AreEqual<string>("Value 3-2", result.SecondValue);
        }
    }
}
