using UnityEngine;
using System.Collections;

public class MenuPanel : MonoBehaviour
{

    public void ShowCharacter()
    {
        MovableAnimator.Instance.AddToQueue(0, 0);
    }

    public void ShowSkills()
    {
        MovableAnimator.Instance.AddToQueue(0, 1);
    }

}
