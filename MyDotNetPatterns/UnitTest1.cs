using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyDotNetPatterns.Lib.DALPattern.People;
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
            
            var people = PersonDAI.GetPeople();
            Assert.IsTrue(people.Any());
        }
    }
}