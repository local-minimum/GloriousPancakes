using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : MonoBehaviour
{
    TMPro.TextMeshProUGUI Text;
    Image Progress;

    void Start()
    {
        Text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Progress = GetComponentInChildren<Image>();
    }


    public void SetText(string text, bool clearProgress = true)
    {
        Text.text = text;
        if (clearProgress) SetProgress(0);
        gameObject.SetActive(true);
    }

    public void SetProgress(float progress)
    {
        if (progress > 0) Text.text = "";
        Progress.fillAmount = progress;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
