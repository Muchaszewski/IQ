using UnityEngine;
using System.Collections;
using InventoryQuest.Game;
using InventoryQuest.Game.Fight;

public class CombatLineManager : MonoBehaviour
{

    public GameObject EnemyHead;

    public void OnEnable()
    {
        FightController.onCreatingEnemies += FightController_onCreatingEnemies;
        if (CurrentGame.Instance != null)
        {
            FightController_onCreatingEnemies(this, new FightControllerEventArgs(CurrentGame.Instance.FightController, null));
        }
    }
    void OnDisable()
    {
        FightController.onCreatingEnemies -= FightController_onCreatingEnemies;
    }

    private void FightController_onCreatingEnemies(object sender, FightControllerEventArgs e)
    {
        for (int i = 0; i < e.FightController.Enemy.Count; i++)
        {
            var item = e.FightController.Enemy[i];
            var enemy = Instantiate(EnemyHead).GetComponent<Enemy>();
            enemy.transform.SetParent(this.transform);
            enemy.GetComponent<RectTransform>().anchoredPosition = new Vector2(float.MaxValue, float.MaxValue);
            enemy.transform.localScale = Vector3.one;
            enemy.EntityID = i;
        }
    }
}
