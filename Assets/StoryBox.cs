using UnityEngine;
using UnityEngine.UI;

public class StoryBox : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI Message;

    public void Show(string message)
    {
        Message.text = message;
        gameObject.SetActive(true);
    }
}
