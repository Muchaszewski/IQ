using UnityEngine;
using System.Collections;
using InventoryQuest.Game.Fight;

public class ExperienceBar : MonoBehaviour {

	// Use this for initialization
	void Start () {
        FightController.onKill += FightController_onVicotry;
	}

    void OnDestroy()
    {
        FightController.onVicotry -= FightController_onVicotry;
    }

    private void FightController_onVicotry(object sender, FightControllerEventArgs e)
    {
        GetComponent<FloatingText>().CreateFloatingText(e.Message);
    }
}
