using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;


namespace Com.sgagdr.BlackSky
{
    public class PickableItem : MonoBehaviourPun
    {
        //Parent of box
        public ItemSpawner spawner;
        public AudioClip audioClip;
        private void OnTriggerEnter(Collider other)
        {
            
            if (other.gameObject.layer == 8)
            {
                Action(other.gameObject);
            }
        }
        virtual public void Action(GameObject other)
        {
            
            PlaySound(spawner.sfx);
            spawner.photonView.RPC("Disable", RpcTarget.All);
        }
        virtual public void PlaySound(AudioSource audio)
        {
            audio.clip = audioClip;
            audio.Play();
        }
    }
}
