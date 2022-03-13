using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

namespace Com.sgagdr.BlackSky
{
    public class ItemSpawner : MonoBehaviourPun
    {
        
        public Transform spawn;
        public float cooldown;
        private float t_cd; 
        public GameObject box;
        public bool isEnabled = true;

        public AudioSource sfx;

        [SerializeField]
        private GameObject spawnedBox;
        

        void Start()
        {
            //box = boxes[(int)typeOfBox];
            box.GetComponent<PickableItem>().spawner = this;
            t_cd = cooldown;
            Spawn();
        }

        void Update()
        {
            if (!isEnabled)
            {
                if (t_cd > 0)
                {
                    t_cd -= Time.deltaTime;
                }
                else { Enable();
                }
            }
        }

       
        [PunRPC]
        public void Disable()
        {
            isEnabled = false;
            t_cd = cooldown;
            spawnedBox.SetActive(false);
        }

        public void Enable()
        {
            isEnabled = true;
            spawnedBox.SetActive(true);
        }

        void Spawn()
        {
            isEnabled = true;
            spawnedBox = Instantiate(box, spawn);
        }
    }
}
