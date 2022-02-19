using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Com.sgagdr.SimpleHostile
{
    public class Manager : MonoBehaviour
    {
        public string playerPrefab;
        public Transform spawnPoint;
        void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            PhotonNetwork.Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
        }
    }
}