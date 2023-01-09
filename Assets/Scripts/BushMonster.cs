using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushMonster : MonoBehaviour
{
    enum BushPhases { Stand, Shrug, Jump };

    BushPhases phase = BushPhases.Jump;

    HarvestSpot harvest;

    [SerializeField, Range(0, 2)]
    float shurgLength = 0.8f;

    [SerializeField, Range(1, 10)]
    float minRestTime = 3f;

    [SerializeField, Range(1, 10)]
    float maxRestTime = 6f;

    [SerializeField]
    Transform[] locations;

    int currentLocation = 0;

    float RestTime { 
        get
        {
            return Random.Range(minRestTime, maxRestTime);
        }
    }

    BushMonsterTop Top;
    BushMonsterBottom Bottom;

    bool sequencing = false;
    Animator anim;

    void Start()
    {
        Top = GetComponentInChildren<BushMonsterTop>();
        Bottom = GetComponentInChildren<BushMonsterBottom>();
        anim = GetComponentInChildren<Animator>();
        harvest = GetComponentInChildren<HarvestSpot>();

        PlaceInCurrentLocation();
    }

    void PlaceInCurrentLocation()
    {
        transform.position = locations[currentLocation].position;
    }

    IEnumerator<WaitForSeconds> Shrug()
    {
        phase = BushPhases.Shrug;
        sequencing = true;
        anim.SetTrigger("Shrug");
        Top.Attacking = true;

        yield return new WaitForSeconds(shurgLength);

        Top.Attacking = false;
        anim.SetTrigger("Normal");
        sequencing = false;
    }

    int NewLocationIndex
    {
        get
        {
            int target = Random.Range(0, locations.Length - 1);
            if (target >= currentLocation) return target + 1;
            return target;
        }
    }

    [SerializeField]
    AnimationCurve lateralJump;

    [SerializeField]
    AnimationCurve horizontalJump;

    [SerializeField]
    float jumpMagnitudePerLateralDistance = 1.5f;

    [SerializeField]
    float jumpDurationPerDistance = 0.7f;

    IEnumerator<WaitForSeconds> Jump()
    {
        phase = BushPhases.Jump;
        sequencing = true;
        Bottom.Attacking = true;

        int target = NewLocationIndex;
        Transform from = locations[currentLocation];
        Transform to = locations[target];

        float distance = Vector3.Magnitude(from.position - to.position);
        if (distance == 0) distance = 1;

        float duration = Mathf.Sqrt(distance) * jumpDurationPerDistance;
        float heightFactor = Mathf.Sqrt(distance) * jumpMagnitudePerLateralDistance;
        float start = Time.timeSinceLevelLoad;
        float progress = 0;

        while (progress < 1)
        {
            progress = (Time.timeSinceLevelLoad - start) / duration;
            Vector3 groundPosition = Vector3.Lerp(from.position, to.position, lateralJump.Evaluate(progress));
            Vector3 height = Vector3.up * horizontalJump.Evaluate(progress) * heightFactor;
            transform.position = groundPosition + height;
            yield return new WaitForSeconds(0.02f);            
        }

        transform.position = to.position;
        currentLocation = target;
        Bottom.Attacking = false;
        sequencing = false;
    }

    IEnumerator<WaitForSeconds> Stand()
    {
        phase = BushPhases.Stand;
        sequencing = true;

        yield return new WaitForSeconds(RestTime);

        sequencing = false;
    }

    private void Update()
    {
        if (sequencing) return;

        if (harvest.Harvested)
        {
            StartCoroutine(Stand());
            return;
        }

        if (phase == BushPhases.Jump)
        {
            StartCoroutine(Stand());
        } else if (phase == BushPhases.Stand)
        {
            StartCoroutine(Shrug());
        } else if (phase == BushPhases.Shrug)
        {
            StartCoroutine(Jump());
        }
    }   
}
