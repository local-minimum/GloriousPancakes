using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HousePerson : MonoBehaviour
{
    [SerializeField]
    AudioClip speach;

    AudioSource speaker;
    
    [SerializeField]
    string SpeakerName;

    Vector3 origin;
    Quaternion startRotation;
    bool speaking = false;

    [SerializeField]
    float positionChangeFrequency = 0.7f;

    [SerializeField]
    Vector3 moveMagnitude = Vector3.up * 0.1f;

    private void Start()
    {
        origin = transform.position;
        startRotation = transform.localRotation;
        speaker = GetComponent<AudioSource>();
    }

    public bool IsCalled(string name)
    {
        return name == SpeakerName;
    }

    public void Speak(float duration) {
        StartCoroutine(_Speak(duration));
    }

    IEnumerator<WaitForSeconds> _Speak(float duration)
    {
        if (speaking) {
            yield break;
        }

        speaker.PlayOneShot(speach);

        speaking = true;
        var start = Time.timeSinceLevelLoad;
        while (Time.timeSinceLevelLoad - start < duration)
        {
            transform.position = origin + Random.value * moveMagnitude;
            yield return new WaitForSeconds(positionChangeFrequency);
        }
        speaking = false;
    }


    public void Silence() {
        speaking = false;
        transform.position = origin;
        transform.localRotation = startRotation;
        speaker.Stop();
    }
}
