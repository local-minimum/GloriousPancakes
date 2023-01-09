using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkMonster : MonoBehaviour
{
    [SerializeField]
    AudioClip moo;

    Spinner spinner;

    AudioSource speaker;

    [SerializeField]
    Transform monsterFlipRoot;

    [SerializeField]
    Transform milkUI;

    [SerializeField]
    Transform westRushStart;

    [SerializeField]
    Transform westRushEnd;

    [SerializeField]
    Transform eastRushStart;

    [SerializeField]
    Transform eastRushEnd;

    MilkMonsterHead head;

    HarvestSpot harvest;

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

    Vector3 milkUIOffset;

    void Start()
    {
        spinner = GetComponentInChildren<Spinner>();
        head = GetComponentInChildren<MilkMonsterHead>();
        anim = GetComponentInChildren<Animator>();
        harvest = GetComponentInChildren<HarvestSpot>();
        speaker = GetComponent<AudioSource>();

        harvest.MayHarvest = false;
        head.FacingWest = rushingWest;

        spinner.gameObject.SetActive(false);

        milkUIOffset = milkUI.localPosition;
    }

    void SetRushStart()
    {
        if (rushingWest)
        {
            milkUI.localPosition = milkUIOffset;
            monsterFlipRoot.localScale = Vector3.one;
            transform.position = westRushStart.position;
            StartCoroutine(DelayRush(westRushStart.position, westRushEnd.position, westRushEnd.GetComponentInChildren<AttackEnabler>()));
        } else 
        {
            milkUI.localPosition = new Vector3(-milkUIOffset.x, milkUIOffset.y, milkUIOffset.z);
            monsterFlipRoot.localScale = new Vector3(-1, 1, 1);
            transform.position = eastRushStart.position;
            StartCoroutine(DelayRush(eastRushStart.position, eastRushEnd.position, eastRushEnd.GetComponentInChildren<AttackEnabler>()));
        }
    }

    IEnumerator<WaitForSeconds> DelayRush(Vector3 from, Vector3 to, AttackEnabler enabler)
    {
        sequencing = true;
        yield return new WaitForSeconds(BeforeRushDelay);
        
        while (!enabler.hasPlayer)
        {
            yield return new WaitForSeconds(0.02f);
        }

        speaker.PlayOneShot(moo);

        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(HeadTiltWait);

        head.Attacking = true;
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
        head.Dizzy = true;
        head.Attacking = false;
        spinner.gameObject.SetActive(true);

        if (!harvest.MayHarvest)
        {
            harvest.MayHarvest = true;
        }

        yield return new WaitForSeconds(CrashTime);

        spinner.gameObject.SetActive(false);

        anim.SetTrigger("Ready");

        rushingWest = !rushingWest;
        sequencing = false; 
        head.FacingWest = rushingWest;
        head.Dizzy = false;
    }

    private void Update()
    {
        if (!sequencing) SetRushStart();
    }
}
