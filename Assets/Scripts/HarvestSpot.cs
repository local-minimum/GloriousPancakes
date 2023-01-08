using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestSpot : MonoBehaviour
{
    [SerializeField]
    float harvestTime = 1f;

    [SerializeField]
    bool autoEnableHarvest;

    [SerializeField]
    GameObject[] harvestables;

    HintUI Hint;

    public bool MayHarvest { get; set; }

    FightPlayer player;

    bool hasPlayer = false;
    bool harvested = false;
    
    void Start()
    {
        Hint = GetComponentInChildren<HintUI>();
        Hint.Hide();

        if (autoEnableHarvest)
        {
            MayHarvest = true;
        }

        player = FindObjectOfType<FightPlayer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player.transform)
        {
            hasPlayer = true;
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player.transform)
        {
            hasPlayer = false;
        }
    }

    float harvetStart = 0;
    bool harvesting = false;

    float harvestProgress
    {
        get
        {
            if (!harvesting) return 0;

            return Mathf.Clamp01((Time.timeSinceLevelLoad - harvetStart) / harvestTime);
        }
    }

    private void Update()
    {
        if (harvested) return;
        if (hasPlayer && player.IsAlive && player.IsStill)
        {
            if (!harvesting)
            {
                if (Input.GetButtonDown("Harvest"))
                {
                    harvetStart = Time.timeSinceLevelLoad;
                    harvesting = true;
                }
                Hint.SetText("Harvest");
            } else if (harvesting && Input.GetButton("Harvest"))
            {
                var progress = harvestProgress;
                if (progress == 1)
                {
                    harvesting = false;
                    Hint.Hide();

                    FindObjectOfType<Inventory>().AddToInventory();

                    RemoveHarvest();
                    harvested = true;
                }
                else
                {
                    Hint.SetProgress(progress);
                }
            } else
            {
                Hint.Hide();
                harvesting = false;

            }
        } else
        {
            Hint.Hide();
            harvesting = false;
        }
    }

    void RemoveHarvest()
    {
        for (int i = 0; i<harvestables.Length; i++)
        {
            harvestables[i].SetActive(false);
        }
    }
}
