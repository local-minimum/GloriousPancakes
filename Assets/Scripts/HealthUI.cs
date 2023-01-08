using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{

    FightPlayer player;
    Image health;

    float previousHealth = 1;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<FightPlayer>();
        health = GetComponentInChildren<Image>();
        health.fillAmount = previousHealth;
    }

    void Update()
    {
        if (player.Health != previousHealth && !updating)
        {
            StartCoroutine(ChangeHealth(player.Health));
        }
    }

    bool updating = false;

    IEnumerator<WaitForSeconds> ChangeHealth(float newHealth)
    {
        updating = true;
        health.enabled = false;
        for (int i=0; i<3; i++)
        {
            yield return new WaitForSeconds(0.2f);
            health.enabled = true;
            if (i < 2)
            {
                yield return new WaitForSeconds(0.25f);
                health.enabled = false;
            }
        }

        int steps = 30;
        for (int i=0; i<=steps; i++)
        {
            health.fillAmount = Mathf.Lerp(previousHealth, newHealth, i / (float)steps);
            yield return new WaitForSeconds(0.02f);
        }
        previousHealth = newHealth;
        health.fillAmount = newHealth;
        updating = false;
    }
}