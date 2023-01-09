using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvestSpot : MonoBehaviour
{
    [SerializeField]
    float harvestTime = 1f;

    [SerializeField]
    bool autoEnableHarvest;

    [SerializeField, Range(1, 5)]
    int harvestIterations = 1;

    [SerializeField]
    GameObject[] harvestables;

    [SerializeField]
    HintUI Hint;

    bool _MayHarvest = false;
    public bool MayHarvest { 
        get { return _MayHarvest; }
        set {
            if (value) Debug.Log($"Remaining {harvestIterations}");
            if (value && harvestIterations > 0)
            {
                _MayHarvest = true;
                harvestIterations--;
                harvested = false;
                EnableHarvestables();
            } else
            {
                DisableHarvestables();
                _MayHarvest = false;
            }
        }
    }    

    FightPlayer player;

    bool hasPlayer = false;
    bool harvested = false;

    public bool Harvested
    {
        get
        {
            return harvested;
        }
    }
    

    void Start()
    {
        if (Hint == null) Hint = GetComponentInChildren<HintUI>();
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
                if (Input.GetButton("Harvest"))
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

                    DisableHarvestables();
                    harvested = true;
                    MayHarvest = false;
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

    void DisableHarvestables()
    {
        for (int i = 0; i<harvestables.Length; i++)
        {
            harvestables[i].SetActive(false);
        }
    }

    void EnableHarvestables()
    {
        for (int i = 0; i < harvestables.Length; i++)
        {
            harvestables[i].SetActive(true);
        }
    }
}
