using UnityEngine.SceneManagement;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    void Update()
    {
        if (Input.anyKeyDown)
        {
            var progress = GameProgression.instance;

            progress.debugPhase = GameProgression.GamePhase.Intro;
            progress.ResetGame();

            SceneManager.LoadScene("HouseScene");
        }

    }
}
