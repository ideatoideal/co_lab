using NUnit.Framework;

namespace XiuxianDemoTests
{
    [TestFixture]
    public class SimpleTest
    {
        [Test]
        public void TestAddition()
        {
            // Arrange
            int a = 1;
            int b = 2;

            // Act
            int result = a + b;

            // Assert
            Assert.AreEqual(3, result);
        }
    }
}