using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathNarrator : MonoBehaviour
{
    GameProgression progression;
    [SerializeField]
    GameObject carol;

    [SerializeField]
    StoryBeat[] NextCarol;
    [SerializeField]
    StoryBeat[] NextAlex;
    [SerializeField]
    StoryBeat[] NextKim;
    [SerializeField]
    StoryBeat[] NextLucinda;
    [SerializeField]
    StoryBeat[] NextSam;
    [SerializeField]
    StoryBeat[] NooneLeft;

    StoryBeat[] storyBeats;
    bool showing = false;

    private void Start()
    {
        progression = GameProgression.instance;

        if (progression.NewDeath)
        {
            storyBox.Hide();
            NarrateDeath();
        } else
        {
            gameObject.SetActive(false);
        }
    }

    void NarrateDeath()
    {
        switch (progression.YoungestAlive())
        {
            case GameProgression.FamilyMember.Carol:
                storyBeats = NextCarol;
                break;
            case GameProgression.FamilyMember.Alex:
                storyBeats = NextAlex;
                break;
            case GameProgression.FamilyMember.Kim:
                storyBeats = NextKim;
                break;
            case GameProgression.FamilyMember.Lucinda:
                storyBeats = NextLucinda;
                break;
            case GameProgression.FamilyMember.Sam:
                storyBeats = NextSam;
                break;
            default:
                storyBeats = NooneLeft;
                break;
        }
        MakeStory();
    }

    [SerializeField]
    HousePerson[] housePeople;

    HousePerson currentSpeaker;

    int storyIndex = 0;


    IEnumerator<WaitForSeconds> RemoveCarol()
    {
        yield return new WaitForSeconds(1f);
        carol.SetActive(false);
    }

    bool MakeStory()
    {
        if (currentSpeaker != null) currentSpeaker.Silence();

        if (storyIndex >= storyBeats.Length) return false;

        var beat = storyBeats[storyIndex];

        if (storyIndex == storyBeats.Length - 2 && GameProgression.instance.YoungestAlive() == GameProgression.FamilyMember.Carol)
        {
            StartCoroutine(RemoveCarol());
        }

        if (beat.delayBefore == 0)
        {
            SpeachAct(beat);
        }
        else
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


    [SerializeField]
    StoryBox storyBox;

    void SpeachAct(StoryBeat beat)
    {
        storyBox.Show(beat.speaker, beat.message);
        Speak(beat.speaker);
        showing = true;
    }

    void Speak(string speaker)
    {
        if (string.IsNullOrEmpty(speaker)) return;

        for (int i = 0; i < housePeople.Length; i++)
        {
            var person = housePeople[i];

            if (person.IsCalled(speaker))
            {
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
                progression.NewDeath = false;
                var youngest = progression.YoungestAlive();                
                if (
                    youngest == GameProgression.FamilyMember.NONE 
                    || youngest == GameProgression.FamilyMember.Carol
                )
                {
                    SceneManager.LoadScene("MainMenu");
                }
                else
                {
                    SceneManager.LoadScene("WorldScene");
                }
            }
        }
    }
}
