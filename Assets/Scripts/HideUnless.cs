using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideUnless : MonoBehaviour
{
    [SerializeField]
    GameProgression.GamePhase[] visiblePhases;

    private void Start()
    {
        var phase = GameProgression.instance.Phase;
        
        for (int i=0; i<visiblePhases.Length; i++)
        {
            if (visiblePhases[i] == phase) return;
        }

        gameObject.SetActive(false);
    }
}
