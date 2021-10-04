using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Cards {
    public enum Element {
        Fire, Water, Normal
    }

    public enum Name {
        Spell, Goblin, Dragon, Wizard, Ork, Knight, Kraken, Elve
    }

    public class Card {

        public bool IsInDeck { get; set; } = false;
        public bool IsTradeOffer { get; set; } = false;

        public readonly Element Element;
        public readonly Name Name;
        public readonly int Damage;

        public Card(Element element, Name name, int damage, bool isInDeck = false, bool isTradeOffer = false) {
            Element = element;
            Name = name;
            Damage = damage;
            IsInDeck = isInDeck;
            IsTradeOffer = isTradeOffer;
        }
    }
}
