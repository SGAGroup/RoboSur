using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailScript : MonoBehaviour
{
    public Vector3 finalPoint;
    public float lifeTime = 4f;

    private float currentTime = 0f;

    void Start()
    {
        StartCoroutine(teleport());
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime;

        if(currentTime > lifeTime)
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator teleport()
    {
        yield return new WaitForSecondsRealtime(0.02f);
        gameObject.transform.position = finalPoint;
    }
}
