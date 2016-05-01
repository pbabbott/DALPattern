using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyDotNetPatterns.Lib.DALPattern.Employees;
using System.Configuration;
using System.Linq;

namespace MyDotNetPatterns
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            
            var employees = EmployeeDAI.GetEmployees();

            Assert.IsTrue(employees.Any());
        }
    }
}