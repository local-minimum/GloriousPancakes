using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitArea : MonoBehaviour
{
    Inventory inventory;
    HintUI Hint;

    [SerializeField]
    float exitTime = 1;

    FightPlayer player;

    // Start is called before the first frame update
    void Start()
    {
        player = FindObjectOfType<FightPlayer>();
        inventory = FindObjectOfType<Inventory>();
        Hint = GetComponentInChildren<HintUI>();
    }

    bool HasPlayer;
    float playerEntryTime = 0;

    float ExitProgress
    {
        get
        {
            if (!HasPlayer) return 0;
            return Mathf.Clamp01((Time.timeSinceLevelLoad - playerEntryTime) / exitTime);
        }
    }

    private void Update()
    {
        if (inventory.Full && player.IsAlive)
        {
            Hint.SetText("Exit", false);

            float progress = ExitProgress;

            Hint.SetProgress(progress);

            if (progress == 1)
            {                
                var progression = GameProgression.instance;
                progression.NextPhase();
                progression.SetYoungestMemberHealth(player.Health);
                SceneManager.LoadScene("HouseScene");
            }
        } else
        {
            Hint.Hide();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player.transform)
        {
            playerEntryTime = Time.timeSinceLevelLoad;
            HasPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform == player.transform)
        {
            HasPlayer = false;
        }
    }
}
