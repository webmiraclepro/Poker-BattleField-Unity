using UnityEngine;
using System.Collections;
using System.IO;
using PokerBattleField;

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
        private CardSlot[] _holeCardSlots;

        private CardSlot[] _dealtCardSlots;

        private const float CardStackDelay = .0001f;

        public int DealInProgress { get; set; }

        private void Awake()
        {
            _cardDeck.InstanatiateDeck();
        }

        public void StartInitialDealing(System.Action cb)
        {
            StartCoroutine(DealInitialCards(cb));
        }

        private void MoveCardSlotToCardSlot(CardSlot sourceCardSlot, CardSlot targerCardSlot) 
        {
            Card card;
            
            while ((card = sourceCardSlot.TopCard()) != null)
            {
                targerCardSlot.AddCard(card);
            }
        }
        
        private IEnumerator DealInitialCards(System.Action Callback) 
        {
            DealInProgress++;
            
            for (int i = 0; i < _cardDeck.CardList.Count; ++i)
            {
                _stackCardSlot.AddCard(_cardDeck.CardList[i]);
                yield return new WaitForSeconds(CardStackDelay);
            }

            yield return new WaitForSeconds(1f);
            yield return StartCoroutine(ShuffleCoroutine());

            yield return new WaitForSeconds(1f);
            foreach(GameObject player in GameObject.FindGameObjectsWithTag("Player"))
            {
                foreach (CardSlot hole in player.GetComponent<PokerPlayer>().HoleCardSlots)
                {
                    hole.AddCard(_stackCardSlot.TopCard());
                    yield return new WaitForSeconds(.5f);
                }
            }

            yield return new WaitForSeconds(2f);
            yield return StartCoroutine(DealCommunityCardSlots());

            DealInProgress--;
            Callback();
        }

        public IEnumerator GatherAllCardsCoroutine()
        {
            DealInProgress++;

            MoveCardSlotToCardSlot(_rightHandCardSlot, _stackCardSlot);
            MoveCardSlotToCardSlot(_leftHandCardSlot, _stackCardSlot);

            yield return new WaitForSeconds(.1f);

            DealInProgress--;
        }

        public IEnumerator ShuffleCoroutine()
        {
            DealInProgress++;

            yield return StartCoroutine(GatherAllCardsCoroutine());

            int halfLength = _stackCardSlot.CardList.Count / 2;
            int fullLength = _stackCardSlot.CardList.Count;

            for (int i = 0; i < halfLength; ++i)
            {
                _leftHandCardSlot.AddCard(_stackCardSlot.TopCard());
            }
            
            yield return new WaitForSeconds(.01f);  
            
            for (int i = 0; i < halfLength; ++i)
            {
                _rightHandCardSlot.AddCard(_stackCardSlot.TopCard());
            }
            
            yield return new WaitForSeconds(.01f);  

            _stackCardSlot.AddCard(_stackCardSlot.TopCard());//jic odd
            
            yield return new WaitForSeconds(CardStackDelay);
            
            for (int i = 0; i < fullLength; ++i)
            {
                if (i % 2 == 0)
                {
                    _stackCardSlot.AddCard(_rightHandCardSlot.TopCard());
                }
                else
                {
                    _stackCardSlot.AddCard(_leftHandCardSlot.TopCard());
                }
                yield return new WaitForSeconds(CardStackDelay);
            }

            yield return new WaitForSeconds(.01f);

            DealInProgress--;
        }

        public IEnumerator CutDeckCoroutine()
        {
            DealInProgress++;
            
            int halfLength = _cardDeck.CardList.Count / 2;
            int thirdLength = _cardDeck.CardList.Count / 3;
            int randomLength = Random.Range(thirdLength, halfLength);

            for (int i = 0; i < randomLength; ++i)
            {
                _leftHandCardSlot.AddCard(_stackCardSlot.TopCard());
            }

            yield return new WaitForSeconds(.5f);   
            
            for (int i = 0; i < (_cardDeck.CardList.Count - randomLength); ++i)
            {
                _rightHandCardSlot.AddCard(_stackCardSlot.TopCard());
            }

            yield return new WaitForSeconds(.1f);
            
            for (int i = 0; i < randomLength; ++i)
            {
                _stackCardSlot.AddCard(_leftHandCardSlot.TopCard());
                yield return new WaitForSeconds(CardStackDelay);
            }
            
            yield return new WaitForSeconds(.5f);
            
            for (int i = 0; i < (_cardDeck.CardList.Count - randomLength); ++i)
            {
                _stackCardSlot.AddCard(_rightHandCardSlot.TopCard());
                yield return new WaitForSeconds(CardStackDelay);
            }
            
            DealInProgress--;
        }

        public IEnumerator DealCommunityCardSlots()
        {
            DealInProgress++;

            foreach (CardSlot slot in _holeCardSlots)
            {
                slot.AddCard(_stackCardSlot.TopCard());
                yield return new WaitForSeconds(.5f);
            }

            yield return new WaitForSeconds(.1f);

            DealInProgress--;
        }

        void FlipCardSlotUp(CardSlot mCardSlot)
        {
            int number = Random.Range(0,2);
            float y = number == 1 ? 0f : -180f;
            float z = mCardSlot.GetComponent<Transform>().rotation.eulerAngles.z;
            Quaternion rot = transform.localRotation;
            rot.eulerAngles = new Vector3(90f, y, z);
            Transform tTemp = mCardSlot.GetComponent<Transform>();
            tTemp.rotation = rot;
            mCardSlot.TopCard().TargetTransform.rotation = rot;
        }

        void FlipCardSlotDown(CardSlot mCardSlot)
        {
            float y = mCardSlot.GetComponent<Transform>().rotation.eulerAngles.y;
            float z = mCardSlot.GetComponent<Transform>().rotation.eulerAngles.z;
            Quaternion rot = transform.localRotation;
            rot.eulerAngles = new Vector3 (270f, y, z);
            Transform tTemp = mCardSlot.GetComponent<Transform>();
            tTemp.rotation = rot;
        }
    }
}