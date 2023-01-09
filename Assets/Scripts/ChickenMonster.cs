using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMonster : MonoBehaviour
{

    ParticleSystem duster;

    enum ChickenPhases { Tracking, LayEgg, ReclaimEgg };
    ChickenMonsterHead[] heads;

    [SerializeField, Range(1, 10)]
    float minTrackTime = 3f;

    [SerializeField, Range(1, 10)]
    float maxTrackTime = 7f;

    [SerializeField, Range(1, 10)]
    float minReclaimEggTime = 2f;

    [SerializeField, Range(1, 10)]
    float maxReclaimEggTime = 3.5f;

    [SerializeField]
    HarvestSpot egg;

    [SerializeField]
    float trackingPlayerOffset;

    Animator anim;
    ChickenPhases phase = ChickenPhases.Tracking;
    bool sequencing = false;

    [SerializeField, Range(0, 1)]
    float EggXOffset = 0.4f;

    float playerTarget;

    FightPlayer player;

    private void Start()
    {
        heads = GetComponentsInChildren<ChickenMonsterHead>();
        anim = GetComponentInChildren<Animator>();
        player = FindObjectOfType<FightPlayer>();
        duster = GetComponentInChildren<ParticleSystem>();

        egg.gameObject.SetActive(false);

        StartCoroutine(Tracking());
    }

    float reclaimEggTime
    {
        get { return Random.Range(minReclaimEggTime, maxReclaimEggTime);  }
    }

    float trackTime
    {
        get { return Random.Range(minTrackTime, maxTrackTime);  }
    }

    [SerializeField, Range(0, 5)]
    float xTrackingSpeed;

    [SerializeField, Range(0, 5)]
    float reCheckPlayerPosition = 2f;

    private void Update()
    {
        if (phase == ChickenPhases.Tracking)
        {
            if (Random.value < reCheckPlayerPosition * Time.deltaTime)
            {
                playerTarget = player.transform.position.x;
            }

            float correction = 0;
            var error = playerTarget + trackingPlayerOffset - transform.position.x;
            if (error > 0) {
                correction = Mathf.Min(error * 0.8f, xTrackingSpeed * Time.deltaTime);
            } else if (error < 0)
            {
                correction = Mathf.Max(error * 0.8f, -xTrackingSpeed * Time.deltaTime);
            }

            if (correction != 0)
            {
                Vector3 pos = transform.position;
                pos.x += correction;
                transform.position = pos;
            }
        }

        if (sequencing) return;

        NextPhase();
    }

    void NextPhase()
    {
        switch (phase)
        {
            case ChickenPhases.Tracking:
                StartCoroutine(Peck());
                break;
            case ChickenPhases.LayEgg:
                StartCoroutine(Reclaim());
                break;
            case ChickenPhases.ReclaimEgg:
                StartCoroutine(Tracking());
                break;
        }
    }
    IEnumerator<WaitForSeconds> Tracking()
    {
        sequencing = true;
        phase = ChickenPhases.Tracking;

        yield return new WaitForSeconds(trackTime);

        sequencing = false;
    }

    void SetHeadPecking(bool pecking)
    {
        for (int i = 0; i<heads.Length; i++)
        {
            heads[i].Pecking = pecking;
        }
    }

    float AverageHeadX
    {
        get
        {
            float x = 0;

            for (int i = 0; i < heads.Length; i++)
            {
                x += heads[i].transform.position.x;
            }

            return x / heads.Length;
        }
    }

    IEnumerator<WaitForSeconds> Peck()
    {
        sequencing = true;
        SetHeadPecking(true);
        anim.SetTrigger("Pick");
        phase = ChickenPhases.LayEgg;

        yield return new WaitForSeconds(0.4f);

        duster.Play();

        yield return new WaitForSeconds(0.6f);

        Vector3 pos = egg.transform.position;
        pos.x = AverageHeadX + EggXOffset;
        egg.transform.position = pos;

        if (!egg.MayHarvest)
        {
            egg.MayHarvest = true;
        }
        egg.gameObject.SetActive(true);

        yield return new WaitForSeconds(0.55f);

        SetHeadPecking(false);
        sequencing = false;
    }

    IEnumerator<WaitForSeconds> Reclaim()
    {
        sequencing = true;
        phase = ChickenPhases.ReclaimEgg;

        yield return new WaitForSeconds(reclaimEggTime);

        SetHeadPecking(true);
        anim.SetTrigger("Pick");

        yield return new WaitForSeconds(0.4f);

        duster.Play();

        yield return new WaitForSeconds(0.6f);

        egg.gameObject.SetActive(false);

        yield return new WaitForSeconds(0.55f);

        SetHeadPecking(false);
        sequencing = false;
    }
}
