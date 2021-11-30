using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    public class Package {

        public int CardNumber { get; set; }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public int Version { get; set; }

        public Package() {

        }

        public Package(Package origin) {
            CardNumber = origin.CardNumber;
            Id = origin.Id;
            Name = origin.Name;
            Damage = origin.Damage;
            Version = origin.Version;
        }

    }
}
