using MTCG.Models.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {

    public enum CardElement {
        Water,
        Fire,
        Normal
    }

    public enum CardType {
        Goblin, 
        Dragon, 
        Wizard, 
        Ork, 
        Knight, 
        Kraken, 
        Elf,
        Spell,
        Unknown
    }


    [DataSource("cards")]
    public class Card : ITEntity {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = "";
        public int Damage { get; set; } = 0;
        public bool InDeck { get; set; } = false;
        public bool IsTradeOffer { get; set; } = false;
        [JsonIgnore]
        public int Version { get; set; } = 1;


        public CardType GetCardType() {
            var enumNames = Enum.GetNames(typeof(CardType));
            foreach (var item in enumNames) {
                if (Name.Contains(item))
                    return Enum.Parse<CardType>(item);
            }
            return CardType.Unknown;
        }

        public CardElement GetCardElement() {
            var enumNames = Enum.GetNames(typeof(CardElement));
            foreach (var item in enumNames) {
                if (Name.Contains(item))
                    return Enum.Parse<CardElement>(item);
            }
            return CardElement.Normal;
        }   

        public Card() { }

        public Card(Card origin) {
            Id = origin.Id;
            UserId = origin.UserId;
            Name = origin.Name;
            Damage = origin.Damage;
            InDeck = origin.InDeck;
            IsTradeOffer = origin.IsTradeOffer;
            Version = origin.Version;
        }
    }
}
