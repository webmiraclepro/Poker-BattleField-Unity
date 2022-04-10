using System.Collections;

using UnityEngine;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Photon.Pun.Poker
{
    public class PokerPlayer : MonoBehaviourPunCallbacks, IPunInstantiateMagicCallback
    {
        [SerializeField]
        private Transform _cameraPosition;

        private GameManager _gameManager;

        public void Awake()
        {
            _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        }

        public void OnPhotonInstantiate(PhotonMessageInfo info)
        {
            // Set player parent hierachy for keeping local world
            gameObject.transform.SetParent(GameObject.Find("PlayerSlots").transform);

            if (photonView.IsMine)
            {
                // Move camera to player view position
                GameObject mainCamera = GameObject.FindWithTag("MainCamera");
                mainCamera.transform.SetPositionAndRotation(_cameraPosition.transform.position, _cameraPosition.transform.rotation);

                // Set my player gameobject to GameManager            
                _gameManager.SetLocalPlayer(this);
            }
        }

        [PunRPC]
        public void Call()
        {
            Debug.Log("Call");
        }
    }
}