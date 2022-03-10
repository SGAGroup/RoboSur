using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    public float speed = 1f;
    public Vector3 direction;
    public float lifeTime = 100f;

    private float currentTime = 0f;

    private void OnCollisionEnter(Collision collision)
    {
        speed = 0f;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        transform.localPosition += direction * speed*Time.deltaTime;
        currentTime += Time.deltaTime;

        if(currentTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }
}
