using MTCG.Endpoints.Battle;
using MTCG.Models;
using NUnit.Framework;

namespace MTCG.Test.Battle {

    [TestFixture]
    public class TestDamageCalculation {
        private Card _card1; 
        private Card _card2;

        [SetUp]
        public void Setup() {
            _card1 = new() { 
                Damage = 10
            };
            _card2 = new() {
                Damage = 10
            };
        }

        [Test]
        public void Card1HasEffectiveElement() {
            _card1.Name = "WaterSpell";
            _card2.Name = "FireSpell";
            CardBattle.CalculateDamage(_card1, _card2, out int damageCard1, out int damageCard2);
            Assert.AreEqual(_card1.Damage * 2, damageCard1);
            Assert.AreEqual(_card2.Damage / 2, damageCard2);
        }

        [Test]
        public void BothCardsElementsAreNotEffective() {
            _card1.Name = "WaterSpell";
            _card2.Name = "WaterSpell";
            CardBattle.CalculateDamage(_card1, _card2, out int damageCard1, out int damageCard2);
            Assert.AreEqual(_card1.Damage, damageCard1);
            Assert.AreEqual(_card2.Damage, damageCard2);
        } 

        [Test]
        public void ElementNotConsiderdIfBothMonsterCards() {
            _card1.Name = "Knight";
            _card2.Name = "Knight";
            CardBattle.CalculateDamage(_card1, _card2, out int damageCard1, out int damageCard2);
            Assert.AreEqual(_card1.Damage, damageCard1);
            Assert.AreEqual(_card2.Damage, damageCard2);
        }

        [Test]
        public void ElementsConsiderdIfMixedFight() {
            _card1.Name = "WaterKnight";
            _card2.Name = "FireSpell";
            CardBattle.CalculateDamage(_card1, _card2, out int damageCard1, out int damageCard2);
            Assert.AreEqual(_card1.Damage * 2, damageCard1);
            Assert.AreEqual(_card2.Damage / 2, damageCard2);
        }

        [Test]
        public void GoblinDoes0DamageAgainstDragon() {
            _card1.Name = "Goblin";
            _card2.Name = "Dragon";
            CardBattle.CalculateDamage(_card1, _card2, out int damageCard1, out int damageCard2);
            Assert.AreEqual(0, damageCard1);
            Assert.AreEqual(_card2.Damage, damageCard2);
        }

        [Test]
        public void OrkDoes0DamageAgainstWizard() {
            _card1.Name = "Ork";
            _card2.Name = "Wizard";
            CardBattle.CalculateDamage(_card1, _card2, out int damageCard1, out int damageCard2);
            Assert.AreEqual(0, damageCard1);
            Assert.AreEqual(_card2.Damage, damageCard2);
        }

        [Test]
        public void KnightDoes0DamageAgainstWaterSpell() {
            _card1.Name = "Knight";
            _card2.Name = "WaterSpell";
            CardBattle.CalculateDamage(_card1, _card2, out int damageCard1, out int damageCard2);
            Assert.AreEqual(0, damageCard1);
            Assert.AreEqual(_card2.Damage, damageCard2);
        }

        [Test]
        public void SpellDoes0DamageAgainstKraken() {
            _card1.Name = "WaterSpell";
            _card2.Name = "Kraken";
            CardBattle.CalculateDamage(_card1, _card2, out int damageCard1, out int damageCard2);
            Assert.AreEqual(0, damageCard1);
            Assert.AreEqual(_card2.Damage, damageCard2);
        }

        [Test]
        public void Dragon0DamageAgainstFireElf() {
            _card1.Name = "Dragon";
            _card2.Name = "FireElf";
            CardBattle.CalculateDamage(_card1, _card2, out int damageCard1, out int damageCard2);
            Assert.AreEqual(0, damageCard1);
            Assert.AreEqual(_card2.Damage, damageCard2);
        }
    }
}
