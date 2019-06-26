using net_platform.Calculation;
using NUnit.Framework;

namespace Tests
{
    [TestFixture]
    public class Tests
    {
        private readonly CalculationEngine _calculationEngine;
        
        public Tests()
        {
            _calculationEngine = new CalculationEngine();
        }
        [Test]
        public void Test1()
        {
            var result = _calculationEngine.testFunction(1, 2);
            Assert.AreEqual(3, result, 0);
        }
    }
}