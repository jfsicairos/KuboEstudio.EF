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
    public class FunctionUnitTest
    {
        [TestMethod]
        public void InputParameters()
        {
            string result = null;

            try
            {
                Query.ExecuteNonQuery(@"DROP FUNCTION Test_Fn");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE FUNCTION Test_Fn (@Input1 VARCHAR(MAX), @Input2 VARCHAR(MAX))
                                    RETURNS VARCHAR(MAX) 
                                    AS
                                    BEGIN
                                        RETURN ISNULL(@Input1, '') + ' - ' + ISNULL(@Input2, '')
                                    END");

            try
            {
                result = Function.SelectScalar<string>("Test_Fn", new object[] { "Input Value 1", "Input Value 2" });
            }
            catch { }

            Assert.AreEqual<string>("Input Value 1 - Input Value 2", result);
        }

        [TestMethod]
        public void SelectScalar()
        {
            string result1 = null;
            string result2 = null;

            try
            {
                Query.ExecuteNonQuery(@"DROP FUNCTION Test_Fn");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE FUNCTION Test_Fn()
                                    RETURNS VARCHAR(MAX) 
                                    AS
                                    BEGIN
                                        RETURN 'Test Scalar'
                                    END");

            try
            {
                result1 = Function.SelectScalar<string>("Test_Fn");
            }
            catch { }

            try
            {
                result2 = (string)Function.SelectScalar("Test_Fn");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP FUNCTION Test_Fn");
            }
            catch { }

            Assert.AreEqual<string>("Test Scalar", result1);
            Assert.AreEqual<string>("Test Scalar", result2);
        }

        [TestMethod]
        public void SelectTable()
        {
            DataTable result = null;

            try
            {
                Query.ExecuteNonQuery(@"DROP FUNCTION Test_Fn");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE FUNCTION Test_Fn()
                                    RETURNS TABLE 
                                    AS
                                    RETURN (
                                        SELECT 'Test Table 1' AS Value
                                        UNION SELECT 'Test Table 2' AS Value
                                    )");

            try
            {
                result = Function.SelectTable("Test_Fn");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP FUNCTION Test_Fn");
            }
            catch { }

            Assert.AreEqual<string>("Test Table 1", result.Rows[0][0].ToString());
            Assert.AreEqual<string>("Test Table 2", result.Rows[1][0].ToString());
        }

        [TestMethod]
        public void Select()
        {
            List<UnitTestClass> result = new List<UnitTestClass>();

            try
            {
                Query.ExecuteNonQuery(@"DROP FUNCTION Test_Fn");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE FUNCTION Test_Fn()
                                    RETURNS TABLE 
                                    AS
                                    RETURN (
                                        SELECT 1 AS ID, 'Value 1-1' AS FirstValue, 'Value 1-2' AS SecondValue
                                        UNION SELECT 2, 'Value 2-1', 'Value 2-2'
                                    )");

            try
            {
                result = Function.Select<UnitTestClass>("Test_Fn");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP FUNCTION Test_Fn");
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
        public void SelectSingle()
        {
            UnitTestClass result = null;

            try
            {
                Query.ExecuteNonQuery(@"DROP FUNCTION Test_Fn");
            }
            catch { }

            Query.ExecuteNonQuery(@"CREATE FUNCTION Test_Fn()
                                    RETURNS TABLE 
                                    AS
                                    RETURN (
                                        SELECT 3 AS ID, 'Value 3-1' AS FirstValue, 'Value 3-2' AS ValueTwo
                                    )");

            try
            {
                result = Function.SelectSingle<UnitTestClass>("Test_Fn");
            }
            catch { }

            try
            {
                Query.ExecuteNonQuery(@"DROP FUNCTION Test_Fn");
            }
            catch { }

            Assert.AreNotEqual(null, result);

            Assert.AreEqual<int>(3, result.ID);
            Assert.AreEqual<string>("Value 3-1", result.FirstValue);
            Assert.AreEqual<string>("Value 3-2", result.SecondValue);
        }
    }
}
