using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestSpot : MonoBehaviour
{
    [SerializeField]
    float harvestTime = 1f;

    [SerializeField]
    bool autoEnableHarvest;

    public bool MayHarvest { get; set; }

    FightPlayer player;

    bool hasPlayer = false;
    
    void Start()
    {
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

    private void Update()
    {
        if (hasPlayer && player.IsAlive && player.IsStill && Input.GetButtonDown("Harvest") && !harvesting)
        {
            harvetStart = Time.timeSinceLevelLoad;
            harvesting = true;
        } else if (harvesting && Input.GetButton("Harvest"))
        {
            // Check if done
            // If so report a harvest
            // Else show progress
        } else
        {
            harvesting = false;
        }
    }
}
