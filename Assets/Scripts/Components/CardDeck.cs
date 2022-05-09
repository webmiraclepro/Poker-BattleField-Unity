using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

using UnityEngine;

public class CardDeck : MonoBehaviour 
{
    [SerializeField]
    private GameObject _cardPrefab;

    [SerializeField]
    private string _sourceAssetBundlePath = "cards";

    public readonly List<Card> CardList =  new List<Card>();                            

    public IEnumerator InstanatiateDeck(IEnumerator onSuccess)
    {
        yield return BundleSingleton.Instance.LoadBundle(_sourceAssetBundlePath, (AssetBundle cardBundle) => {
            string[] nameArray = cardBundle.GetAllAssetNames();
            ShuffleArray(nameArray);

            for (int i = 0; i < nameArray.Length; ++i)
            {
                GameObject cardInstance = (GameObject) Instantiate(_cardPrefab, transform.position, transform.rotation);
                Card card = cardInstance.GetComponent<Card>();
                card.SourceAssetBundlePath = _sourceAssetBundlePath;
                card.TexturePath = nameArray[i];
                CardList.Add(card);
            }

        });
        
        yield return onSuccess;
    }

    public static void ShuffleArray<T>(T[] arr)
    {
        for (int i = arr.Length - 1; i > 0; i--) 
        {
            int r = UnityEngine.Random.Range(0, i);
            T tmp = arr[i];
            arr[i] = arr[r];
            arr[r] = tmp;
        }
    }
}