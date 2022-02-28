using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 direction;
    public float lifeTime = 100f;

    private float currentTime = 0f;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        do
        {
            transform.localPosition += direction*speed;
            currentTime += Time.deltaTime;
        } while (currentTime < lifeTime);
    }
}
