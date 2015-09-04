using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class TutorialManager : MonoBehaviour
{

    public CharacterCreationHead CharacterCreationHead;
    public Image PlayerHead;
    public GameObject GroupCombat;
    public GameObject GroupHeadCreation;

    void Start()
    {

    }

    public void ChooseHead()
    {
        PlayerHead.sprite = CharacterCreationHead.CurrentHead;
        GroupCombat.SetActive(true);
        GroupHeadCreation.SetActive(false);
    }

}
