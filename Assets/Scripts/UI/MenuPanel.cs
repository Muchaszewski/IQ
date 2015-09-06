using UnityEngine;
using System.Collections;

public class MenuPanel : MonoBehaviour
{

    // Left Panels

    public void ShowCharacter()
    {
        MovableAnimator.Instance.AddToQueue(0, 0);
    }

    public void ShowSkills()
    {
        MovableAnimator.Instance.AddToQueue(0, 1);
    }

    public void ShowLore()
    {
        MovableAnimator.Instance.AddToQueue(0, 2);
    }

    public void ShowMap()
    {
        MovableAnimator.Instance.AddToQueue(0, 3);
    }

    //Middle panels
    private bool _isShown;
    public void ShowOptions()
    {
        MovableAnimator.Instance.AddToQueue(1, _isShown ? 0 : 1);
        _isShown = !_isShown;
    }


    // Right panels

    public void ShowEquipment()
    {
        MovableAnimator.Instance.AddToQueue(2, 0);
    }

    public void ShowQuests()
    {
        MovableAnimator.Instance.AddToQueue(2, 1);
    }

}