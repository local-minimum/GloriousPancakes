using UnityEngine;
using System.Linq;
using System.Collections.Generic;

public class PlaceTransition : MonoBehaviour
{
    public enum PlaceTarget { Pen, Coop, Field, Forest };

    [SerializeField]
    float walkTime = 1f;

    [SerializeField]
    PlaceTransition[] connections;

    [SerializeField]
    PlaceTarget[] LeadsTo;

    public void WalkTo(Transform player, PlaceTarget target) {
        Debug.Log($"Recieved player at {name}, target {target}");
        player.position = transform.position;
        StartCoroutine(_DoWalk(player, target));
    }

    PlaceTransition GetNext(PlaceTarget target)
    {
        for (int i = 0; i < connections.Length; i++)
        {
            var connection = connections[i];
            if (connection.TakesPlayerTo(target))
            {
                Debug.Log($"Sending player to {connection.name}");
                return connection;
            }
        }

        Debug.Log($"Found no connection to target {target}");

        if (TakesPlayerTo(target))
        {
            return null;
        }
        else
        {

            throw new System.ArgumentException($"Attempted to reach {target} but I'm lost at {name}");
        }

    }

    IEnumerator<WaitForSeconds> _DoWalk(Transform player, PlaceTarget target)
    {
        PlaceTransition nextTarget = GetNext(target);
        if (nextTarget == null)
        {
            // TODO: Load next scene
            yield break;
        }
        Vector3 startPosition = player.position;
        float startTime = Time.timeSinceLevelLoad;
        float progress = 0;
        Debug.Log($"Starting walk from {name} to {nextTarget.name}");
        while (progress < 1)
        {
            player.position = Vector3.Lerp(startPosition, nextTarget.transform.position, progress);
            yield return new WaitForSeconds(0.02f);
            progress = (Time.timeSinceLevelLoad - startTime) / nextTarget.walkTime;
        }

        nextTarget?.WalkTo(player, target);
    }

    public bool TakesPlayerTo(PlaceTarget target)
    {
        return LeadsTo.Any(v => v == target);
    }
}
