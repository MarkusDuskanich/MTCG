using MTCG.DAL;
using MTCG.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MTCG.Endpoints.Battle {
    public class CardBattle {
        private readonly User _user1;
        private readonly User _user2;
        private readonly List<Card> _deckUser1;
        private readonly List<Card> _deckUser2;

        private readonly Random _rng = new();

        public BattleLog Log { get; private set; } = new();
        private int _rounds = 0; 

        public CardBattle(User user1, User user2) {
            _user1 = user1;
            _user2 = user2;
            using var uow = new UnitOfWork();
            _deckUser1 = uow.CardRepository.Get(card => card.UserId == _user1.Id);
            _deckUser2 = uow.CardRepository.Get(card => card.UserId == _user2.Id);
            Battle();
        }

        private void Battle() {
            while(_rounds < 100 && _deckUser1.Any() && _deckUser2.Any()) {
                Log.Add(ResolveRound());
                _rounds++;
            }

            //update wins, losses, games played
            using var uow = new UnitOfWork();

            _user1.GamesPlayed++;
            _user2.GamesPlayed++;

            if (_rounds >= 100 && _deckUser1.Any() && _deckUser2.Any()) {
                //draw
                Log.Add("Draw");

            } else if (_deckUser1.Any()) {
                //user1 wins
                Log.Add($"{_user1.Name} Wins");
                _user1.Wins++;
                _user2.Losses++;
            } else {
                //user2 wins
                Log.Add($"{_user1.Name} Wins");
                _user1.Losses++;
                _user2.Wins++;
            }
            Log.Add($"{_rounds} rounds played");

            uow.UserRepository.Update(_user1);
            uow.UserRepository.Update(_user2);

            uow.Save();
        }


        private string ResolveRound() {
            //battle logic here
            var cardUser1 = _deckUser1[_rng.Next(_deckUser1.Count)];
            var cardUser2 = _deckUser2[_rng.Next(_deckUser2.Count)];
            string result;

            //write a damage calculate function
            if (cardUser1.Damage > cardUser2.Damage) {
                //user1 wins round
                result = $"{cardUser1.Name} wins";
                _deckUser1.Add(cardUser2);
                _deckUser2.Remove(cardUser2);

            }else if(cardUser1.Damage < cardUser2.Damage) {
                //user2 wins round
                result = $"{cardUser2.Name} wins";
                _deckUser2.Add(cardUser1);
                _deckUser1.Remove(cardUser1);
            } else
                result = $"Draw";

            //fix updated damage
            string setup = $"{_user1.Name}: {cardUser1.Name} ({cardUser1.Damage} Damage) vs {_user2.Name}: {cardUser2.Name} ({cardUser2.Damage} Damage)" +
                $"--> {cardUser1.Damage} VS {cardUser2.Damage} --> {cardUser1.Damage} VS {cardUser2.Damage} --> ";

            return setup + result;
        }
    }
}