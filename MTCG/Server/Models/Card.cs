using MTCG.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    [TEntity("cards")]
    public class Card : ITEntity {

        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string Name { get; set; } = "";
        public int Damage { get; set; } = 0;
        public bool InDeck { get; set; } = false;
        public bool IsTradeOffer { get; set; } = false;
        public int Version { get; set; } = 1;

        //write a method to get element and type from name

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
