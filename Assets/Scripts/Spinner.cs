using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spinner : MonoBehaviour
{
    [SerializeField]
    Camera cam;

    [SerializeField]
    Transform[] stars;

    [SerializeField]
    float angleSpeed = 10;

    float angle = 0;


    private void Update()
    {        
        transform.Rotate(transform.up, angleSpeed * Time.deltaTime);
        for (int i = 0; i<stars.Length; i++)
        {
            stars[i].rotation = Quaternion.LookRotation(transform.position - cam.transform.position, transform.up);
        }
    }
}
