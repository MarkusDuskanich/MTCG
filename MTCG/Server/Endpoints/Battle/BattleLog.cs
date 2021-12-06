using MTCG.Models;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace MTCG.Endpoints.Battle {
    public class BattleLog {
        public User User { get; set; }

        private readonly List<string> _log = new();

        public void Add(string message) {
            _log.Add(message);
        }

        public BattleLog(BattleLog origin) {
            _log = origin._log;
        }
        public BattleLog() { }

        public override string ToString() {
            return JsonConvert.SerializeObject(_log); 
        }
    }
}