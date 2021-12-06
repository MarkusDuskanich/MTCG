using MTCG.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace MTCG.Endpoints.Battle {
    public class BattleHandler {

        private readonly ConcurrentQueue<User> _registeredUsers = new();

        private readonly Thread t_doBattle = null;

        private readonly List<BattleLog> _logs = new();
        private readonly object _logsLock = new();

        public static BattleHandler Instance { get; } = new();

        private BattleHandler() {
            t_doBattle = new(DoBattle) { IsBackground = true };
            t_doBattle.Start();
        }

        private void DoBattle() {
            User user1 = null;
            User user2 = null;
            int battleStartAttempts = 0;
            while (true) {
                if (_registeredUsers.TryDequeue(out var user)) {
                    if (user1 == null) {
                        user1 = user;
                        battleStartAttempts = 0;
                    } else {
                        user2 = user;
                    }
                } else {
                    battleStartAttempts++;
                }

                if (user1 != null && user2 != null) {
                    var res = new CardBattle(user1, user2).Log;
                    lock (_logsLock) {
                        res.User = user1;
                        _logs.Add(res);
                        _logs.Add(new(res) { User = user2 });
                    }
                    user1 = null;
                    user2 = null;
                    battleStartAttempts = 0;
                }

                if (battleStartAttempts >= 800) {
                    user1 = null;
                    user2 = null;
                    battleStartAttempts = 0;
                }

                Thread.Sleep(10);
            }
        }

        public void RegisterForBattle(User user) {
            _registeredUsers.Enqueue(user);
        }

        public BattleLog GetBattleLog(User user) {
            int fetchAttempts = 0;
            while (fetchAttempts < 1000) {
                lock (_logsLock) {
                    foreach (var item in _logs) {
                        if (item.User == user) {
                            _logs.Remove(item);

                            if (_registeredUsers.Contains(user))
                                return null;

                            return item;
                        }
                    }
                }
                fetchAttempts++;
                Thread.Sleep(10);
            }
            return null;
        }
    }
}
