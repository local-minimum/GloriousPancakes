using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterKiller : MonoBehaviour
{
    [SerializeField]
    GameObject[] characters;
    GameProgression progression;

    void Start()
    {
        progression = GameProgression.instance;
        KillOffMembers();
    }

    void KillOffMembers()
    {
        for (int i = 0; i<characters.Length; i++)
        {
            var character = characters[i];
            var member = progression.NameToMember(character.name);
            if (member == GameProgression.FamilyMember.NONE)
            {
                Debug.LogWarning($"{character.name} is not a family member");
                character.SetActive(false);

                continue;
            }

            if (progression.Health(member) == 0) {
                character.SetActive(false);
            }
        }
    }
}
