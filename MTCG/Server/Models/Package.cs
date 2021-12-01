using MTCG.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    [TEntity("packages")]
    public class Package : ITEntity{

        public Guid Id { get; set; }
        public string Name { get; set; } = "";
        public int Damage { get; set; } = 0;
        public int Version { get; set; } = 1;

        public Package() {

        }

        public Package(Package origin) {
            Id = origin.Id;
            Name = origin.Name;
            Damage = origin.Damage;
            Version = origin.Version;
        }

    }
}
