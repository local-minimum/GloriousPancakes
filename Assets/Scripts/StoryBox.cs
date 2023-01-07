using UnityEngine;
using UnityEngine.UI;

public class StoryBox : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI Message;

    public void Show(string speaker, string message)
    {
        Message.text = string.IsNullOrEmpty(speaker) ? message : $"{speaker}:\n{message}";
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
