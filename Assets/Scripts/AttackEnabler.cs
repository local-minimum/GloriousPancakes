using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnabler : MonoBehaviour
{
    FightPlayer player;
    public bool hasPlayer { get; private set; }

    private void Awake()
    {
        hasPlayer = false;
    }

    private void Start()
    {
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

}
