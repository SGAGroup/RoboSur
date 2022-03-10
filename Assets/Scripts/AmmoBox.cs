using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.sgagdr.BlackSky
{

    public class AmmoBox : MonoBehaviour
    {
        //Amount of ammo to add
        public int amount = 30;
        //Parent of box
        public AmmoSpawner spawner;
        //Enum type of ammo
        public typeOfAmmo type;
        public AudioSource sfx;
        private void Start()
        {
            sfx.Stop();
        }
        private void OnTriggerEnter(Collider other)
        {
            Debug.Log("Collision Enter!");
            Debug.Log(other.gameObject.layer);
            if (other.gameObject.layer == 8)
            {
                Gun[] arr = other.gameObject.GetComponent<Weapon>().loadout;
                foreach (Gun gun in arr)
                {
                    if (gun.ammoType == type)
                    {
                        gun.AddStash(amount);
                        sfx.Play();
                        StartCoroutine(DestroyObj());
                    }
                }

            }
        }
        IEnumerator DestroyObj()
        {
            yield return new WaitForSecondsRealtime(sfx.clip.length);
            spawner.isSpawned = false;
            Destroy(gameObject);
        }
    }
}
