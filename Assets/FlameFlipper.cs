using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlameFlipper : MonoBehaviour
{
    Vector3 localScale;

    [SerializeField]
    float flipProbability = 0.4f;

    bool flipped = false;

    void Start()
    {
        localScale = transform.localScale;        
    }

    
    void Update()
    {
        if (Random.value < flipProbability * Time.deltaTime)
        {
            transform.localScale = new Vector3(localScale.x * (flipped ? -1 : 1), localScale.y, localScale.z);
            flipped = !flipped;
        }
    }
}
