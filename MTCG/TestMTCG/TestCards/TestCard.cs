using NUnit.Framework;
using MTCG.Cards;

namespace TestMTCG.TestCards {
    public class TestCard {
        [SetUp]
        public void Setup() {
        }

        [Test]
        public void TestMakeCard() {
            Card card = new(Element.Fire, Name.Dragon, 10);
            Assert.IsTrue(Name.Dragon == card.Name);
            Assert.IsTrue(Element.Fire == card.Element);
            Assert.IsTrue(card.Damage == 10);
        }
    }
}