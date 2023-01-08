using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MilkMonsterHead : MonoBehaviour
{
    [SerializeField]
    float tossAmount = 5f;

    [SerializeField]
    float pushAmount = 5f;

    [SerializeField]
    float tossHurt = 0.3f;

    [SerializeField]
    float pushHurt = 0.1f;

    FightPlayer player;

    public bool Attacking { get; set; }
    public bool FacingWest { get; set; }

    private void Start()
    {
        player = FindObjectOfType<FightPlayer>();
    }

    private void OnCollisionEnter(Collision collision)
    {        
        if (collision.transform == player.transform) { 
            if (Attacking)
            {
                player.Hurt(Vector3.up * tossAmount, tossHurt);
            } else
            {
                player.Hurt((FacingWest ? -1 : 1) * Vector3.right * pushAmount + Vector3.up * pushAmount, pushHurt);
            }
        }
    }
}
