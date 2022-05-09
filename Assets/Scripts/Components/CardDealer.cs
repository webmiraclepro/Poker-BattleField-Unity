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
        
        public IEnumerator Initialize() 
        {
            foreach(GameObject slotObj in GameObject.FindGameObjectsWithTag("CardSlot"))
            {
                slotObj.GetComponent<CardSlot>().Clear();
            }
            _cardDeck.CardList.Clear();

            yield return _cardDeck.InstanatiateDeck(_Initialize());
        }

        private IEnumerator _Initialize()
        {
            for (int i = 0; i < _cardDeck.CardList.Count; ++i)
            {
                _stackCardSlot.AddCard(_cardDeck.CardList[i]);
                yield return new WaitForSeconds(0.001f);
            }

            yield return new WaitForSeconds(1f);

            PokerGame.SetPlayerStatus(PokerGame.CARD_DEALER_READY, true);
        }

        public IEnumerator DealHoleCards(string[] holeCards, IEnumerator onSuccess)
        {
            foreach(GameObject pObj in GameObject.FindGameObjectsWithTag("Player"))
            {
                PokerPlayer pp = pObj.GetComponent<PokerPlayer>();
                string[] cards = holeCards[pp.ID].Split(new char[] { ' ' });

                Card card1 = _stackCardSlot.TopCard();
                card1.FaceValue = cards[0];
                pp.HoleCardSlots[0].AddCard(card1);
                yield return new WaitForSeconds(0.1f);

                Card card2 = _stackCardSlot.TopCard();
                card2.FaceValue = cards[1];
                pp.HoleCardSlots[1].AddCard(card2);
                yield return new WaitForSeconds(0.1f);
            }

            yield return new WaitForSeconds(1f);

            PokerGame.SetPlayerStatus(PokerGame.DEALT_HOLE_CARDS, true);

            yield return onSuccess;
        }

        public IEnumerator DealFlopCards(string[] flopCards)
        {
            for (int i = 0; i < flopCards.Length; i++)
            {
                Card card = _stackCardSlot.TopCard();
                card.FaceValue = flopCards[i];

                _flopCardSlots[i].AddCard(card);

                yield return new WaitForSeconds(0.5f);
            }

            yield return new WaitForSeconds(0.5f);
        }

        public IEnumerator DealTurnCard(string turnCard)
        {
            Card card = _stackCardSlot.TopCard();
            card.FaceValue = turnCard;
            _turnCardSlot.AddCard(card);

            yield return new WaitForSeconds(1f);
        }

        public IEnumerator DealRiverCard(string riverCard)
        {
            Card card = _stackCardSlot.TopCard();
            card.FaceValue = riverCard;
            _riverCardSlot.AddCard(card);

            yield return new WaitForSeconds(1f);
        }

        public IEnumerator ShuffleCards()
        {
            int halfLength = _stackCardSlot.CardList.Count / 2;
            int fullLength = _stackCardSlot.CardList.Count;

            for (int i = 0; i < halfLength; ++i)
            {
                _leftHandCardSlot.AddCard(_stackCardSlot.TopCard());
            }
            
            yield return new WaitForSeconds(0.1f);
            
            for (int i = 0; i < halfLength; ++i)
            {
                _rightHandCardSlot.AddCard(_stackCardSlot.TopCard());
            }
            
            yield return new WaitForSeconds(0.1f);

            _stackCardSlot.AddCard(_stackCardSlot.TopCard());
            
            yield return new WaitForSeconds(0.01f);
            
            for (int i = 0; i < fullLength; ++i)
            {
                _stackCardSlot.AddCard((i % 2 == 0 ? _rightHandCardSlot : _leftHandCardSlot).TopCard());
                yield return new WaitForSeconds(0.001f);
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
}