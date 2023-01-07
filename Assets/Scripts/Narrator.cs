using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Narrator : MonoBehaviour
{
    [SerializeField]
    StoryBox storyBox;

    [SerializeField]
    StoryBeat[] storyBeats;

    [SerializeField]
    HousePerson[] housePeople;

    int storyIndex = 0;
    bool showing = false;

    HousePerson currentSpeaker;


    void Start()
    {
        storyBox.Hide();
        MakeStory();
    }

    bool MakeStory()
    {
        if (currentSpeaker != null) currentSpeaker.Silence();

        if (storyIndex >= storyBeats.Length) return false;

        var beat = storyBeats[storyIndex];

        if (beat.delayBefore == 0) {
            SpeachAct(beat);
        } else
        {
            StartCoroutine(DelayedStory(beat));            
        }

        return true;
    }

    IEnumerator<WaitForSeconds> DelayedStory(StoryBeat beat)
    {
        yield return new WaitForSeconds(beat.delayBefore);
        SpeachAct(beat);
    }

    void SpeachAct(StoryBeat beat)
    {
        storyBox.Show(beat.speaker, beat.message);
        Speak(beat.speaker);
        showing = true;
    }

    void Speak(string speaker) {
        if (string.IsNullOrEmpty(speaker)) return;

        for (int i = 0; i<housePeople.Length; i++)
        {
            var person = housePeople[i];

            if (person.IsCalled(speaker)) {
                person.Speak(2f);
                currentSpeaker = person;
                return;
            }
        }
    }

    private void Update()
    {
        if (!showing) return;

        if (Input.anyKeyDown)
        {
            showing = false;
            storyIndex++;
            if (!MakeStory())
            {
                // Load next sccene!
            }
        }
    }
}