using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenMonsterHead : MonoBehaviour
{
    [SerializeField, Range(1, 20)]
    float pushLateral = 5f;

    [SerializeField, Range(1, 20)]
    float pushUp = 2f;

    [SerializeField, Range(0, 1)]
    float peckHurt = 0.4f;

    [SerializeField, Range(0, 1)]
    float baseHurt = 0.2f;

    public bool Pecking { get; set; }

    FightPlayer player;
    

    private void Start()
    {
        player = FindObjectOfType<FightPlayer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform == player.transform)
        {
            float dir = Mathf.Sign(collision.transform.position.x - transform.position.x);
            if (dir == 0) dir = 1;

            if (Pecking)
            {
                player.Hurt(Vector3.up * pushUp + dir * Vector3.right * pushLateral, peckHurt);
            } else
            {
                player.Hurt(dir * Vector3.right * pushLateral, baseHurt);
            }
        }
    }
}
