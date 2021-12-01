using MTCG.Models.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MTCG.Models {
    [TEntity("Define this tag for every descendant")]
    public interface ITEntity {
        public Guid Id { get; set; }
        public int Version { get; set; }

    }
}
