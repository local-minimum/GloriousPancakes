using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlayer : MonoBehaviour
{
    [SerializeField]
    PlaceTransition home;

    [SerializeField]
    PlaceTransition.PlaceTarget target;

    [SerializeField]
    float waitBeforeWalk = 1f;

    private void Start()
    {        
        StartCoroutine(WaitWalk());
    }

    IEnumerator<WaitForSeconds> WaitWalk() {
        transform.position = home.transform.position;
        yield return new WaitForSeconds(waitBeforeWalk);
        home.WalkTo(transform, target);
    }

}
