﻿using MTCG.Models.Attributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    [DataSource("tradeoffers")]
    public class TradeOffer : ITEntity{
        public Guid Id { get; set; }
        public Guid CardId { get; set; }
        public Guid UserId { get; set; }
        public bool MustBeSpell { get; set; } = false;
        public string Element { get; set; } = "";
        public int MinDamage { get; set; } = 0;
        [JsonIgnore]
        public int Version { get; set; } = 1;

        public TradeOffer() {

        }

        public TradeOffer(TradeOffer origin) {
            Id = origin.Id;
            CardId = origin.Id;
            UserId = origin.UserId;
            MustBeSpell = origin.MustBeSpell;
            Element = origin.Element;
            MinDamage = origin.MinDamage;
            Version = origin.Version;
        }
    }
}
