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

    Transform player;
    PlaceTarget target;
    PlaceTransition nextTarget;


    public void WalkTo(Transform player, PlaceTarget target) {
        Debug.Log($"Recieved player at {name}, target {target}");
        player.position = transform.position;
        
        // StartCoroutine(_DoWalk(player, target));

        nextTarget = GetNext(target);
        if (nextTarget == null)
        {
            // Load Fight Scene
            Debug.Log($"I'm at {target}");
            return;
        }

        this.player = player;
        this.target = target;
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

    public bool TakesPlayerTo(PlaceTarget target)
    {
        return LeadsTo.Any(v => v == target);
    }


    float progress = 0;
    float speedScale = 1f;

    private void Update()
    {
        if (player == null) return;

        // Going forward
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
        {
            progress += Time.deltaTime * speedScale / nextTarget.walkTime;
            progress = Mathf.Clamp01(progress);
            player.position = Vector3.Lerp(transform.position, nextTarget.transform.position, progress);
            if (progress == 1)
            {
                nextTarget.WalkTo(player, target);
                player = null;
            }
        }
    }
}
