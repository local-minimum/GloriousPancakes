using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushMonsterTop : MonoBehaviour
{
    public bool Attacking { get; set; }

    [SerializeField]
    float attackHurt = 0.25f;

    [SerializeField]
    float attackPush = 20f;

    FightPlayer player;

    private void Start()
    {
        player = FindObjectOfType<FightPlayer>();
    }

    int RandomDirection
    {
        get
        {
            return Random.Range(0, 2) == 0 ? -1 : 1;
        }
    }
    private void OnCollisionStay(Collision collision)
    {
        if (Attacking && collision.transform == player.transform)
        {
            player.Hurt(Vector3.up * attackPush + attackPush * RandomDirection * Vector3.right, attackHurt);
        }
    }
}
