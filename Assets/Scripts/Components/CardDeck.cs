using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

public class CardDeck : MonoBehaviour 
{
    [SerializeField]
    private GameObject _cardPrefab;

    [SerializeField]
    private string _sourceAssetBundlePath = "cards";

    public readonly List<Card> CardList =  new List<Card>();                            

    public async Task InstanatiateDeck()
    {
        AssetBundle cardBundle = await BundleSingleton.Instance.LoadBundle(_sourceAssetBundlePath);

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