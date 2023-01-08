using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkMonster : MonoBehaviour
{
    [SerializeField]
    Transform westRushStart;

    [SerializeField]
    Transform westRushEnd;

    bool rushingWest = true;

    Animator anim;

    [SerializeField, Range(1, 10)]
    float BeforeRushDelayMin = 1;
    [SerializeField, Range(1, 10)]
    float BeforeRushDelayMax = 3;

    [SerializeField]
    float HeadTiltWait = 0.5f;

    [SerializeField]
    float RushDuration = 1f;

    [SerializeField, Range(1, 10)]
    float CrashTimeMin = 5f;
    [SerializeField, Range(1, 10)]
    float CrashTimeMax = 8f;

    bool sequencing = false;

    float BeforeRushDelay
    {
        get
        {
            return Random.Range(BeforeRushDelayMin, BeforeRushDelayMax);
        }
    }

    float CrashTime
    {
        get
        {
            return Random.Range(CrashTimeMin, CrashTimeMax);
        }
    }

    void Start()
    {
        anim = GetComponentInChildren<Animator>();
    }

    void SetRushStart()
    {
        if (rushingWest)
        {
            transform.position = westRushStart.position;
            StartCoroutine(DelayRush(westRushStart.position, westRushEnd.position));
        }
    }

    IEnumerator<WaitForSeconds> DelayRush(Vector3 from, Vector3 to)
    {
        sequencing = true;
        yield return new WaitForSeconds(BeforeRushDelay);        
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(HeadTiltWait);

        float startTime = Time.timeSinceLevelLoad;
        float progress = 0;
        while (progress < 1)
        {
            yield return new WaitForSeconds(0.02f);
            progress = Mathf.Clamp01((Time.timeSinceLevelLoad - startTime) / RushDuration);
            transform.position = Vector3.Lerp(from, to, progress);
        }

        transform.position = to;
        anim.SetTrigger("Crash");

        yield return new WaitForSeconds(CrashTime);
        anim.SetTrigger("Ready");

        sequencing = false;
    }

    private void Update()
    {
        if (!sequencing) SetRushStart();
    }
}
