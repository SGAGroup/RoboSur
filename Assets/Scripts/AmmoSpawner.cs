using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.sgagdr.BlackSky
{
    public class AmmoSpawner : MonoBehaviour
    {
        //Place for spawn
        public Transform spawn;
        //Time for next spawn
        public float cooldown;
        //Array of boxes for spawn
        public GameObject[] boxes;
        //Choose type for spawn
        public typeOfAmmo typeOfBox;
        //Variable says is cube already spawned
        public bool isSpawned = false;

        private GameObject box;

        void Start()
        {
            box = boxes[(int)typeOfBox];
            box.GetComponent<AmmoBox>().spawner = this;
        }

        void Update()
        {
            if (!isSpawned)
            {
                StartCoroutine(Spawn());
            }
        }

        IEnumerator Spawn()
        {
            isSpawned = true;
            yield return new WaitForSecondsRealtime(cooldown);
            Instantiate(box, spawn);
        }
    }
}
