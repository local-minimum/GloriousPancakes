using UnityEngine;
using System.Linq;
using UnityEngine.SceneManagement;

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

    void LoadScene()
    {
        switch (GameProgression.instance.Phase)
        {
            case GameProgression.GamePhase.FirstWheat:
                SceneManager.LoadScene("FirstWheat");
                return;
            case GameProgression.GamePhase.Milk:
                SceneManager.LoadScene("FightMilk");
                return;
            case GameProgression.GamePhase.Eggs:
                SceneManager.LoadScene("FightEggs");
                return;
            case GameProgression.GamePhase.Berries:
                SceneManager.LoadScene("FightBerries");
                return;
            default:
                Debug.LogWarning("I don't know what to do");
                return;
        }
    }

    public void WalkTo(Transform player, PlaceTarget target) {
        Debug.Log($"Recieved player at {name}, target {target}");
        player.position = transform.position;
        
        nextTarget = GetNext(target);
        if (nextTarget == null)
        {            
            Debug.Log($"I'm at {target}");
            LoadScene();
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
