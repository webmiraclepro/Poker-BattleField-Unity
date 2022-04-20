using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CardDeck : MonoBehaviour 
{
    [SerializeField]
    private GameObject _cardPrefab;

    public readonly List<Card> CardList =  new List<Card>();                            

    public void InstanatiateDeck()
    {
        AssetBundle cardBundle = BundleSingleton.Instance.LoadBundle("cards");
        string[] nameArray = cardBundle.GetAllAssetNames();
        ShuffleArray(nameArray);

        for (int i = 0; i < nameArray.Length; ++i)
        {
            GameObject cardInstance = (GameObject)Instantiate(_cardPrefab, transform.position, transform.rotation);
            Card card = cardInstance.GetComponent<Card>();
            card.SourceAssetBundlePath = "cards";
            card.TexturePath = nameArray[i];
            CardList.Add(card);
        }
    }

    public static void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--) 
        {
            int r = Random.Range(0, i);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }
}