using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Com.sgagdr.BlackSky
{
    public class Manager : MonoBehaviour
    {
        public string playerPrefab;
        public Transform[] spawnPoints;
        void Start()
        {
            Spawn();
        }

        public void Spawn()
        {
            Transform t_spawn = spawnPoints[Random.Range(0, spawnPoints.Length)];
            PhotonNetwork.Instantiate(playerPrefab, t_spawn.position, t_spawn.rotation);
        }
    }
}