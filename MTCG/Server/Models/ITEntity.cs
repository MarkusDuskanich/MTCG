using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    public interface ITEntity {
        public Guid Id { get; set; }
        public int Version { get; set; }

    }
}
