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

    PlaceTransition.PlaceTarget ResolveTarget(GameProgression.GamePhase phase)
    {
        switch (phase)
        {
            case GameProgression.GamePhase.FirstWheat:
                return PlaceTransition.PlaceTarget.Field;
            case GameProgression.GamePhase.Milk:
                return PlaceTransition.PlaceTarget.Pen;
            default:
                return target;

        }
    }

    private void Start()
    {
        var progression = GameProgression.instance;
        Debug.Log($"Phase {progression.Phase}");
        target = ResolveTarget(progression.Phase);

        StartCoroutine(WaitWalk());
    }

    IEnumerator<WaitForSeconds> WaitWalk() {
        transform.position = home.transform.position;
        yield return new WaitForSeconds(waitBeforeWalk);
        home.WalkTo(transform, target);
    }

}
