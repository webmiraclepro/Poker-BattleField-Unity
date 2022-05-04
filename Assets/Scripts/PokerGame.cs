using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace PokerBattleField
{
    public class PokerGame
    {
        public const int PLAYER_NUMBER = 6;
        public const string PLAYER_READY = "IsPlayerReady";
        public const string PLAYER_LOADED_LEVEL = "PlayerLoadedLevel";
        public const string CHARACTER_READY = "CHARACTER_READY";
        public const string CARD_DEALER_READY = "CARD_DEALER_READY";
        public const string DEALT_HOLE_CARDS = "DEALT_HOLE_CARDS";
        
        public static Color GetColor(int colorChoice)
        {
            switch (colorChoice)
            {
                case 0: return Color.red;
                case 1: return Color.green;
                case 2: return Color.blue;
                case 3: return Color.yellow;
                case 4: return Color.cyan;
                case 5: return Color.grey;
                case 6: return Color.magenta;
                case 7: return Color.white;
            }

            return Color.black;
        }

        public static void SetPlayerStatus(string prop, bool flag)
        {
            Hashtable props = new Hashtable { { prop, flag } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        
        public static bool CheckPlayersStatus(Hashtable changedProps, string statusKey)
        {
            if (!changedProps.ContainsKey(statusKey))
            {
                return false;
            }
            
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object status;
                
                if (p.CustomProperties.TryGetValue(statusKey, out status))
                {
                    if (!(bool) status)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }
    }
}