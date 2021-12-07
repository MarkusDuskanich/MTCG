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
            PlayRounds();
            UpdatePlayerData();
        }

        private void PlayRounds() {
            while (_rounds < 100 && _deckUser1.Any() && _deckUser2.Any()) {
                Log.Add(ResolveRound());
                _rounds++;
            }
        }

        private void UpdatePlayerData() {

            //update wins, losses, games played
            var uow = new UnitOfWork();

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
            var cardUser1 = _deckUser1[_rng.Next(_deckUser1.Count)];
            var cardUser2 = _deckUser2[_rng.Next(_deckUser2.Count)];
            string result;

            CalculateDamage(cardUser1, cardUser2, out int damageCard1, out int damageCard2);
            if (damageCard1 > damageCard2) {
                //user1 wins round
                result = $"{cardUser1.Name} wins";
                _deckUser1.Add(cardUser2);
                _deckUser2.Remove(cardUser2);

            } else if (damageCard1 < damageCard2) {
                //user2 wins round
                result = $"{cardUser2.Name} wins";
                _deckUser2.Add(cardUser1);
                _deckUser1.Remove(cardUser1);
            } else {
                result = $"Draw";
            }

            string setup = $"{_user1.Name}: {cardUser1.Name} ({cardUser1.Damage} Damage) vs {_user2.Name}: {cardUser2.Name} ({cardUser2.Damage} Damage)" +
                $" --> {cardUser1.Damage} VS {cardUser2.Damage} --> {damageCard1} VS {damageCard2} --> ";

            return setup + result;
        }


        public static void CalculateDamage(Card card1, Card card2, out int damageCard1, out int damageCard2) {
            if (card1.GetCardType() == CardType.Goblin && card2.GetCardType() == CardType.Dragon) {
                damageCard1 = 0;
                damageCard2 = card2.Damage;

            } else if (card2.GetCardType() == CardType.Goblin && card1.GetCardType() == CardType.Dragon) {
                damageCard1 = card1.Damage;
                damageCard2 = 0;

            } else if (card1.GetCardType() == CardType.Ork && card2.GetCardType() == CardType.Wizard) {
                damageCard1 = 0;
                damageCard2 = card2.Damage;

            } else if (card2.GetCardType() == CardType.Ork && card1.GetCardType() == CardType.Wizard) {
                damageCard1 = card1.Damage;
                damageCard2 = 0;

            } else if (card1.GetCardType() == CardType.Knight && card2.GetCardType() == CardType.Spell && card2.GetCardElement() == CardElement.Water) {
                damageCard1 = 0;
                damageCard2 = card2.Damage;

            } else if (card2.GetCardType() == CardType.Knight && card1.GetCardType() == CardType.Spell && card1.GetCardElement() == CardElement.Water) {
                damageCard1 = card1.Damage;
                damageCard2 = 0;

            } else if (card1.GetCardType() == CardType.Kraken && card2.GetCardType() == CardType.Spell) {
                damageCard1 = card1.Damage;
                damageCard2 = 0;

            } else if (card2.GetCardType() == CardType.Kraken && card1.GetCardType() == CardType.Spell) {
                damageCard1 = 0;
                damageCard2 = card2.Damage;

            } else if (card1.GetCardType() == CardType.Dragon && card2.GetCardType() == CardType.Elf && card2.GetCardElement() == CardElement.Fire) {
                damageCard1 = 0;
                damageCard2 = card2.Damage;

            } else if (card2.GetCardType() == CardType.Dragon && card1.GetCardType() == CardType.Elf && card1.GetCardElement() == CardElement.Fire) {
                damageCard1 = card1.Damage;
                damageCard2 = 0;

            } else {
                if(card1.GetCardType() == CardType.Spell || card2.GetCardType() == CardType.Spell) {
                    ElementalDamage(card1, card2, out damageCard1, out damageCard2);

                } else {
                    damageCard1 = card1.Damage;
                    damageCard2 = card2.Damage;
                }
            }
        }

        private static void ElementalDamage(Card card1, Card card2, out int damageCard1, out int damageCard2) {
            if (card1.GetCardElement() == CardElement.Water && card2.GetCardElement() == CardElement.Fire) {
                damageCard1 = card1.Damage * 2;
                damageCard2 = card2.Damage / 2;

            } else if (card2.GetCardElement() == CardElement.Water && card1.GetCardElement() == CardElement.Fire) {
                damageCard1 = card1.Damage / 2;
                damageCard2 = card2.Damage * 2;

            } else if (card1.GetCardElement() == CardElement.Fire && card2.GetCardElement() == CardElement.Normal) {
                damageCard1 = card1.Damage * 2;
                damageCard2 = card2.Damage / 2;

            } else if (card2.GetCardElement() == CardElement.Fire && card1.GetCardElement() == CardElement.Normal) {
                damageCard1 = card1.Damage / 2;
                damageCard2 = card2.Damage * 2;

            } else if (card1.GetCardElement() == CardElement.Normal && card2.GetCardElement() == CardElement.Water) {
                damageCard1 = card1.Damage * 2;
                damageCard2 = card2.Damage / 2;

            } else if (card2.GetCardElement() == CardElement.Normal && card1.GetCardElement() == CardElement.Water) {
                damageCard1 = card1.Damage / 2;
                damageCard2 = card2.Damage * 2;

            } else {
                damageCard1 = card1.Damage;
                damageCard2 = card2.Damage;
            }
        }
    }
}