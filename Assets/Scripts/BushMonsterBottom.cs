using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BushMonsterBottom : MonoBehaviour
{
    public bool Attacking { get; set; }

    [SerializeField]
    float attackHurt = 0.3f;

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

    private void OnCollisionEnter(Collision collision)
    {
        if (Attacking && collision.transform == player.transform)
        {
            var direction = Mathf.Sign(collision.transform.position.x - transform.position.x);
            if (direction == 0) direction = RandomDirection;

            player.Hurt(Vector2.up * attackPush + attackPush * direction * Vector2.right, attackHurt);
        }
    }
}
