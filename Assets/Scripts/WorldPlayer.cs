using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPlayer : MonoBehaviour
{

    AudioSource speaker;

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
            case GameProgression.GamePhase.Eggs:
                return PlaceTransition.PlaceTarget.Coop;
            case GameProgression.GamePhase.Berries:
                return PlaceTransition.PlaceTarget.Forest;
            default:
                return target;

        }
    }

    private void Start()
    {
        speaker = GetComponent<AudioSource>();
        var progression = GameProgression.instance;
        Debug.Log($"Phase {progression.Phase}");
        target = ResolveTarget(progression.Phase);

        StartCoroutine(WaitWalk());
    }

    IEnumerator<WaitForSeconds> WaitWalk()
    {
        transform.position = home.transform.position;
        yield return new WaitForSeconds(waitBeforeWalk);
        home.WalkTo(transform, target);
    }

    private void Update()
    {
        if (Input.GetAxis("Horizontal") > 0 || Input.GetAxis("Vertical") > 0)
        {
            if (!speaker.isPlaying) speaker.Play();
        }
        else
        {
            if (speaker.isPlaying) speaker.Stop();
        }
    }
}
