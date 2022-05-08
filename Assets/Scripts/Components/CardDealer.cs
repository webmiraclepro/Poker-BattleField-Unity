using UnityEngine;
using System.Collections;
using System.IO;
using PokerBattleField;
using System.Threading.Tasks;

namespace PokerBattleField
{
    public class CardDealer : MonoBehaviour 
    {
        [SerializeField]
        private CardDeck _cardDeck; 

        [SerializeField]
        private CardSlot _stackCardSlot;        

        [SerializeField]
        private CardSlot _rightHandCardSlot;

        [SerializeField]
        private CardSlot _leftHandCardSlot;

        [SerializeField]
        private CardSlot[] _flopCardSlots;

        [SerializeField]
        private CardSlot _turnCardSlot;

        [SerializeField]
        private CardSlot _riverCardSlot;
        
        public async Task Initialize() 
        {
            foreach(GameObject slotObj in GameObject.FindGameObjectsWithTag("CardSlot"))
            {
                slotObj.GetComponent<CardSlot>().Clear();
            }
            _cardDeck.CardList.Clear();

            await _cardDeck.InstanatiateDeck();

            for (int i = 0; i < _cardDeck.CardList.Count; ++i)
            {
                _stackCardSlot.AddCard(_cardDeck.CardList[i]);
                await Task.Delay(1);
            }

            await Task.Delay(1000);

            PokerGame.SetPlayerStatus(PokerGame.CARD_DEALER_READY, true);

            
        }

        public async Task DealHoleCards(string[] holeCards)
        {
            foreach(GameObject pObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                PokerPlayer pp = pObj.GetComponent<PokerPlayer>();
                string[] cards = holeCards[pp.ID].Split(new char[] { ' ' });

                Card card1 = _stackCardSlot.TopCard();
                card1.FaceValue = cards[0];
                pp.HoleCardSlots[0].AddCard(card1);
                await Task.Delay(100);

                Card card2 = _stackCardSlot.TopCard();
                card2.FaceValue = cards[1];
                pp.HoleCardSlots[1].AddCard(card2);
                await Task.Delay(100);
            }

            await Task.Delay(1000);

            PokerGame.SetPlayerStatus(PokerGame.DEALT_HOLE_CARDS, true);
        }

        public async Task DealFlopCards(string[] flopCards)
        {
            for (int i = 0; i < flopCards.Length; i++)
            {
                Card card = _stackCardSlot.TopCard();
                card.FaceValue = flopCards[i];

                _flopCardSlots[i].AddCard(card);

                await Task.Delay(500);
            }

            await Task.Delay(500);
        }

        public async Task DealTurnCard(string turnCard)
        {
            Card card = _stackCardSlot.TopCard();
            card.FaceValue = turnCard;
            _turnCardSlot.AddCard(card);

            await Task.Delay(1000);
        }

        public async Task DealRiverCard(string riverCard)
        {
            Card card = _stackCardSlot.TopCard();
            card.FaceValue = riverCard;
            _riverCardSlot.AddCard(card);

            await Task.Delay(1000);
        }

        public async Task ShuffleCards()
        {
            int halfLength = _stackCardSlot.CardList.Count / 2;
            int fullLength = _stackCardSlot.CardList.Count;

            for (int i = 0; i < halfLength; ++i)
            {
                _leftHandCardSlot.AddCard(_stackCardSlot.TopCard());
            }
            
            await Task.Delay(100);
            
            for (int i = 0; i < halfLength; ++i)
            {
                _rightHandCardSlot.AddCard(_stackCardSlot.TopCard());
            }
            
            await Task.Delay(100);

            _stackCardSlot.AddCard(_stackCardSlot.TopCard());
            
            await Task.Delay(1);
            
            for (int i = 0; i < fullLength; ++i)
            {
                _stackCardSlot.AddCard((i % 2 == 0 ? _rightHandCardSlot : _leftHandCardSlot).TopCard());
                await Task.Delay(1);
            }

            await Task.Delay(100);
        }
    }
}