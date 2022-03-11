using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Com.sgagdr.BlackSky
{
    public class PickableItem : MonoBehaviour
    {
        //Parent of box
        public ItemSpawner spawner;
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
                Action(other.gameObject);
            }
        }
        virtual public void Action(GameObject other)
        {
            Debug.Log("Picked an item!");
        }
        public IEnumerator DestroyObj()
        {
            yield return new WaitForSecondsRealtime(0.1f);
            spawner.isSpawned = false;
            Destroy(gameObject);
        }
    }
}
