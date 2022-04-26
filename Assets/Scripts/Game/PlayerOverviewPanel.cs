using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using ExitGames.Client.Photon;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Photon.Pun;

namespace PokerBattleField
{
    public class PlayerOverviewPanel : MonoBehaviourPunCallbacks
    {
        public GameObject PlayerOverviewEntryPrefab;

        private Dictionary<int, GameObject> playerListEntries;

        #region UNITY

        public void Awake()
        {
            playerListEntries = new Dictionary<int, GameObject>();

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerOverviewEntryPrefab);
                entry.transform.SetParent(gameObject.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<Text>().color = PokerGame.GetColor(p.GetPlayerNumber());
                entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}", p.NickName, p.GetScore());

                playerListEntries.Add(p.ActorNumber, entry);
            }
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            GameObject go = null;
            if (this.playerListEntries.TryGetValue(otherPlayer.ActorNumber, out go))
            {
                Destroy(playerListEntries[otherPlayer.ActorNumber]);
                playerListEntries.Remove(otherPlayer.ActorNumber);
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                entry.GetComponent<Text>().text = string.Format("{0}\nScore: {1}", targetPlayer.NickName, targetPlayer.GetScore());
            }
        }

        #endregion
    }
}