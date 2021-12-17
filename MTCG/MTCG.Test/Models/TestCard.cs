using MTCG.DAL.ORM;
using MTCG.Http.Method;
using MTCG.Http.Status;
using MTCG.Models;
using Newtonsoft.Json;
using Npgsql;
using NUnit.Framework;
using System;
using System.Linq;

namespace MTCG.Test.Models {
    [TestFixture]
    public class TestCard {
        [Test]
        public void FireGoblinHasCardElementFire() {
            Card card = new() {
                Name = "FireGoblin"
            };
            Assert.AreEqual(CardElement.Fire, card.GetCardElement());
        }

        [Test]
        public void WaterGoblinHasCardElementWater() {
            Card card = new() {
                Name = "WaterGoblin"
            };
            Assert.AreEqual(CardElement.Water, card.GetCardElement());
        }

        [Test]
        public void KnightHasCardElementNormal() {
            Card card = new() {
                Name = "Knight"
            };
            Assert.AreEqual(CardElement.Normal, card.GetCardElement());
        }

        [Test]
        public void GoblinHasCardTypeGoblin() {
            Card card = new() {
                Name = "Goblin"
            };
            Assert.AreEqual(CardType.Goblin, card.GetCardType());
        }

        [Test]
        public void DragonHasCardTypeDragon() {
            Card card = new() {
                Name = "Dragon"
            };
            Assert.AreEqual(CardType.Dragon, card.GetCardType());
        }

        [Test]
        public void WizardHasCardTypeWizard() {
            Card card = new() {
                Name = "Wizard"
            };
            Assert.AreEqual(CardType.Wizard, card.GetCardType());
        }

        [Test]
        public void OrkHasCardTypeOrk() {
            Card card = new() {
                Name = "Ork"
            };
            Assert.AreEqual(CardType.Ork, card.GetCardType());
        }

        [Test]
        public void KnightHasCardTypeKnight() {
            Card card = new() {
                Name = "Knight"
            };
            Assert.AreEqual(CardType.Knight, card.GetCardType());
        }

        [Test]
        public void KrakenHasCardTypeKraken() {
            Card card = new() {
                Name = "Kraken"
            };
            Assert.AreEqual(CardType.Kraken, card.GetCardType());
        }

        [Test]
        public void ElfHasCardTypeElf() {
            Card card = new() {
                Name = "Elf"
            };
            Assert.AreEqual(CardType.Elf, card.GetCardType());
        }

        [Test]
        public void SpellHasCardTypeSpell() {
            Card card = new() {
                Name = "Spell"
            };
            Assert.AreEqual(CardType.Spell, card.GetCardType());
        }

        [Test]
        public void AbcHasCardTypeUnknown() {
            Card card = new() {
                Name = "Unknown"
            };
            Assert.AreEqual(CardType.Unknown, card.GetCardType());
        }


    }
}
