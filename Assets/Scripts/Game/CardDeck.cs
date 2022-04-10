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
		AssetBundle cardBundle = BundleSingleton.Instance.LoadBundle("card");
		string[] nameArray = cardBundle.GetAllAssetNames();
		ShuffleArray(nameArray);

		for (int i = 0; i < nameArray.Length; ++i)
		{
			string NameTemp = Path.GetFileNameWithoutExtension(nameArray[i]);
			GameObject cardInstance = (GameObject)Instantiate(_cardPrefab);
			Card card = cardInstance.GetComponent<Card>();
			card.TexturePath = nameArray[i];
			card.transform.position = new Vector3(0, 1, 0);
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

	private int StringToFaceValue(string input)
	{
		for (int i = 2; i < 11; ++i)
		{
			if (input.Contains(i.ToString()))
			{
				return i;
			}
		}
		if (input.Contains("jack") ||
			input.Contains("queen") ||
			input.Contains("king"))
		{
			return 10;
		}
		if (input.Contains("ace"))
		{
			return 11;
		}
		return 0;
	}
}
