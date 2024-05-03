using SimpleTGBot;
using static SimpleTGBot.TelegramBot;
namespace TestProject1
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void TestWriteRecipe()
        {
            var test_1 = "Files\\tests.txt";
            Assert.That(WriteRecipe(1,2, test_1), Is.EqualTo("11111"));
            Assert.That(WriteRecipe(2, 4, test_1), Is.EqualTo("rrrrr"));
            Assert.That(WriteRecipe(4, 9, test_1), Is.EqualTo("222"));
            Assert.That(WriteRecipe(1, 10, test_1), Is.EqualTo(""));
        }
    }
}