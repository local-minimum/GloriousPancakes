using System.Collections.Generic;
using UnityEngine;


public class GameProgression : MonoBehaviour
{
    public static GameProgression instance { get; private set; }
    
    public enum GamePhase { Intro, FirstWheat, Milk, Eggs, Berries, EndingFeast, EndingMeagerFeast, EndingAlone };
    public enum FamilyMember { Jane, Sam, Lucinda, Kim, Alex, Carol, NONE };

    [SerializeField]
    public GamePhase debugPhase;

    GamePhase phase = GamePhase.Intro;
    public GamePhase Phase { get { return phase; } }

    Dictionary<FamilyMember, float> familyHealth = new Dictionary<FamilyMember, float>();

    public bool NewDeath { get; set; }
    
    private void Awake()
    {       
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        } else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            ResetGame();
        }
    }

    public void NextPhase()
    {
        switch (phase) {
            case GamePhase.Intro:
                phase = GamePhase.FirstWheat;
                break;
            case GamePhase.FirstWheat:
                phase = GamePhase.Milk;
                break;
            case GamePhase.Milk:
                phase = GamePhase.Eggs;
                break;
            case GamePhase.Eggs:
                phase = GamePhase.Berries;
                break;
            case GamePhase.Berries:
                if (YoungestAlive() == FamilyMember.Jane)
                {
                    phase = GamePhase.EndingMeagerFeast;
                } else
                {
                    phase = GamePhase.EndingFeast;
                }
                break;
            default:
                Debug.LogWarning($"No known phase after {phase}");
                break;
        }
        Debug.Log($"New phase {phase}");
    }

    public void ResetGame() {
        phase = GamePhase.Intro;
        phase = debugPhase;
        familyHealth[FamilyMember.Alex] = 1;
        familyHealth[FamilyMember.Carol] = 1;
        familyHealth[FamilyMember.Jane] = 1;
        familyHealth[FamilyMember.Kim] = 1;
        familyHealth[FamilyMember.Lucinda] = 1;
        familyHealth[FamilyMember.Sam] = 1;
        Debug.Log("Game reset");
    }

    public float YoungestMembersHealt
    {
        get
        {
            return familyHealth[YoungestAlive()];
        }
    }

    public float Health(FamilyMember member)
    {
        return familyHealth[member];
    }

    public void SetYoungestMemberHealth(float health)
    {
        familyHealth[YoungestAlive()] = health;
    }

    public FamilyMember NameToMember(string name)
    {
        switch (name.ToUpper().Trim())
        {
            case "ALEX":
                return FamilyMember.Alex;
            case "CAROL":
                return FamilyMember.Carol;
            case "JANE":
                return FamilyMember.Jane;
            case "KIM":
                return FamilyMember.Kim;
            case "SAM":
                return FamilyMember.Sam;
            case "LUCINDA":
                return FamilyMember.Lucinda;
            default:
                return FamilyMember.NONE;
        }
    }

    public string MemberName(FamilyMember member)
    {
        switch (member)
        {
            case FamilyMember.Alex:
                return "Alex";
            case FamilyMember.Carol:
                return "Carol";
            case FamilyMember.Jane:
                return "Jane";
            case FamilyMember.Kim:
                return "Kim";
            case FamilyMember.Lucinda:
                return "Lucinda";
            case FamilyMember.Sam:
                return "Sam";
            default:
                throw new System.ArgumentException($"Not a family member {member}");
        }
    }

    public FamilyMember YoungestAlive()
    {
        if (familyHealth[FamilyMember.Jane] > 0) return FamilyMember.Jane;
        if (familyHealth[FamilyMember.Sam] > 0) return FamilyMember.Sam;
        if (familyHealth[FamilyMember.Lucinda] > 0) return FamilyMember.Lucinda;
        if (familyHealth[FamilyMember.Kim] > 0) return FamilyMember.Kim;
        if (familyHealth[FamilyMember.Alex] > 0) return FamilyMember.Alex;
        if (familyHealth[FamilyMember.Carol] > 0) return FamilyMember.Carol;        
        return FamilyMember.NONE;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}
