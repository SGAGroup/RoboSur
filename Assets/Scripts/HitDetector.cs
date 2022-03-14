using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Com.sgagdr.BlackSky
{
    public class HitDetector : MonoBehaviour
    {
        public GameObject whoIsTouched;

        private void OnTriggerStay(Collider other)
        {
            GameObject o = other.gameObject;
            if (o.layer == 10)
            {
                whoIsTouched = o;
            }
            else
            {
                whoIsTouched = new GameObject();
            }
        }
        private void OnTriggerExit(Collider other)
        {
            GameObject o = other.gameObject;
            if (o.layer == 10)
            {
                whoIsTouched = new GameObject();
            }
        }
    }
}