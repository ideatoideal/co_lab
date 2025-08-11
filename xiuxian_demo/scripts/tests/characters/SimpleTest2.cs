public class SimpleTest2
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestAddition()
    {
        int result = 1 + 1;
        Assert.AreEqual(2, result);
    }
}