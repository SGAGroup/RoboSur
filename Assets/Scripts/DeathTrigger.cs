using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.sgagdr.BlackSky
{
    public class DeathTrigger : MonoBehaviour
    {
        public int damage = 1000;

        public Collider col;
        void Start()
        {
            if (!col)
            {
                col = gameObject.GetComponent<BoxCollider>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            other.gameObject.GetComponent<Player>().TakeDamage(damage);
        }
    }
}

