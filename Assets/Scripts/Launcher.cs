using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.sgagdr.SimpleHostile
{
    public class Launcher : MonoBehaviour
    {
        public void OnEnable()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            Connect();
        }
        public void Connect()
        {

        }
        public void Join()
        {

        }
        public void StartGame()
        {

        }
    }
}
