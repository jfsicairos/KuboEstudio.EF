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
    public class StoreProcedureUnitTest
    {
        [TestMethod]
        public void OutputParameters()
        {

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE PROCEDURE Test_SP 
                                    @OutputParameter VARCHAR(80) OUTPUT
                                    AS
                                    BEGIN
                                        SET @OutputParameter = 'Test Value'
                                    END");

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("@OutputParameter", null, System.Data.DbType.String, ParamType.Out));

            try
            {
                StoreProcedure.ExecuteNonQuery("Test_SP", parameters.ToArray());
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Assert.AreEqual<string>("Test Value", parameters[0].Value.ToString());
        }

        [TestMethod]
        public void InputOutputParameters()
        {

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE PROCEDURE Test_SP 
                                    @OutputParameter VARCHAR(80) OUTPUT
                                    AS
                                    BEGIN
                                        SET @OutputParameter = ISNULL(@OutputParameter, '') + ' - Test Value'
                                    END");

            List<Parameter> parameters = new List<Parameter>();
            parameters.Add(new Parameter("@OutputParameter", "Input Value", System.Data.DbType.String, ParamType.InOut));

            try
            {
                StoreProcedure.ExecuteNonQuery("Test_SP", parameters.ToArray());
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Assert.AreEqual<string>("Input Value - Test Value", parameters[0].Value.ToString());
        }

        [TestMethod]
        public void ExecuteScalar()
        {
            string result1 = null;
            string result2 = null;

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE PROCEDURE Test_SP 
                                    AS
                                    BEGIN
                                        SELECT 'Test Scalar'
                                    END");

            try
            {
                result1 = StoreProcedure.ExecuteScalar<string>("Test_SP");
            }
            catch { }

            try
            {
                result2 = (string)StoreProcedure.ExecuteScalar("Test_SP");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
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
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE PROCEDURE Test_SP 
                                    AS
                                    BEGIN
                                        SELECT 'Test Table 1' AS Value

                                        SELECT 'Test Table 2' AS Value
                                    END");

            try
            {
                result = StoreProcedure.ExecuteDataSet("Test_SP");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
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
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE PROCEDURE Test_SP 
                                    AS
                                    BEGIN
                                        SELECT 'Test Table' AS Value
                                    END");

            try
            {
                result = StoreProcedure.ExecuteDataTable("Test_SP");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
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
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE PROCEDURE Test_SP 
                                    AS
                                    BEGIN
                                        SELECT 1 AS [Key], 'One' AS [Value]
                                        UNION SELECT 2, 'Two'
                                    END");

            try
            {
                result = StoreProcedure.ExecuteDictionary<Dictionary<int, string>>("Test_SP");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
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
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE PROCEDURE Test_SP 
                                    AS
                                    BEGIN
                                        SELECT 1 AS ID, 'Value 1-1' AS FirstValue, 'Value 1-2' AS SecondValue
                                        UNION SELECT 2, 'Value 2-1', 'Value 2-2'
                                    END");

            try
            {
                result = StoreProcedure.Execute<UnitTestClass>("Test_SP");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
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
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE PROCEDURE Test_SP 
                                    AS
                                    BEGIN
                                        SELECT 3 AS ID, 'Value 3-1' AS FirstValue, 'Value 3-2' AS ValueTwo
                                    END");

            try
            {
                result = StoreProcedure.ExecuteSingle<UnitTestClass>("Test_SP");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP PROCEDURE Test_SP");
            }
            catch { }

            Assert.AreNotEqual(null, result);

            Assert.AreEqual<int>(3, result.ID);
            Assert.AreEqual<string>("Value 3-1", result.FirstValue);
            Assert.AreEqual<string>("Value 3-2", result.SecondValue);
        }
    }
}
