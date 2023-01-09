using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintUI : MonoBehaviour
{
    [SerializeField]
    AudioClip startSound;

    AudioSource speaker;

    TMPro.TextMeshProUGUI Text;
    Image Progress;

    void Awake()
    {
        Text = GetComponentInChildren<TMPro.TextMeshProUGUI>();
        Progress = GetComponentInChildren<Image>();
        speaker = GetComponent<AudioSource>();
    }


    public void SetText(string text, bool clearProgress = true)
    {
        Text.text = text;
        if (clearProgress) SetProgress(0);
        gameObject.SetActive(true);
    }

    public void SetProgress(float progress)
    {
        if (progress > 0)
        {
            Text.text = "";
            if (!speaker.isPlaying)
            {
                speaker.PlayOneShot(startSound);
            }
        }
        Progress.fillAmount = progress;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Update()
    {        
    }
}
