using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using InventoryQuest.Components.Entities;
using InventoryQuest.Game;
using InventoryQuest.Game.Fight;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IPointerClickHandler
{

    public const float MaxCombatPosition = 241f;
    public const float MinCombatPosition = 94f;

    public bool IsRightSide = true;
    public int EntityID;
    public GameObject FloatingText;
    public Vector3 floatingTextPosition;

    private float _progress;
    private Entity _entityData;
    private RectTransform _rectTransform;

    public Entity EntityData
    {
        get { return _entityData; }
    }

    // Use this for initialization
    void Start()
    {
        _entityData = CurrentGame.Instance.FightController.Enemy[EntityID];
        _rectTransform = GetComponent<RectTransform>();
        FightController.onAttack += FightController_onAttack;
    }


    // Update is called once per frame
    void Update()
    {

        // Death
        if (EntityData.Stats.HealthPoints.Current <= 0)
        {
            Destroy(this.gameObject);
        }

        _progress = EntityData.Position;
        float position = 0;

        // Update position
        position = _progress + (_progress < 0 ? -MinCombatPosition : MinCombatPosition);
        // Enemey out of bounds
        if (Mathf.Abs(position) > MaxCombatPosition)
        {
            position = _progress < 0 ? -MaxCombatPosition : MaxCombatPosition;
        }

        // Apply position
        _rectTransform.anchoredPosition = new Vector2(position, 0);

    }
    public void OnDestroy()
    {
        FightController.onAttack -= FightController_onAttack;
    }

    private void FightController_onAttack(object sender, FightControllerEventArgs e)
    {
        if (e.Target.Equals(EntityData))
        {
            CreateFloatingText(e.Message);
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        CurrentGame.Instance.FightController.Target = EntityData;
        CurrentGame.Instance.FightController.Attack(CurrentGame.Instance.Player, EntityData);
    }

    public void CreateFloatingText(string message)
    {
        var text = Instantiate(FloatingText).GetComponent<Text>();
        var splitted = message.Split('@');
        float number;
        for (int i = 0; i < splitted.Length; i++)
        {
            var item = splitted[i];
            /*
            /m missed
            /p parried
            /b blocked
            /e exhausted
            /a absorb
            /c critical
            */
            if (item.Contains("/m"))
            {
                splitted[i] = item.Replace("/m", "");
                text.color = text.GetComponent<FloatingTextAnimation>().ColorMissed;
            }
            else if (item.Contains("/c"))
            {
                splitted[i] = item.Replace("/c", "");
                float.TryParse(splitted[i], out number);
                if (number == 0)
                {
                    text.color = text.GetComponent<FloatingTextAnimation>().ColorDamage;
                }
                else
                {
                    text.color = text.GetComponent<FloatingTextAnimation>().ColorCritical;

                }
            }
            else if (item.Contains("/p"))
            {
                splitted[i] = item.Replace("/p", "");
                text.color = text.GetComponent<FloatingTextAnimation>().ColorParried;
            }
            else if (item.Contains("/b"))
            {
                splitted[i] = item.Replace("/b", "");
                text.color = text.GetComponent<FloatingTextAnimation>().ColorBlocked;
            }
            else if (item.Contains("/e"))
            {
                splitted[i] = item.Replace("/e", "");
                text.color = text.GetComponent<FloatingTextAnimation>().ColorExhausted;
            }
            else if (item.Contains("/a"))
            {
                splitted[i] = item.Replace("/a", "");
                text.color = text.GetComponent<FloatingTextAnimation>().ColorAbsorbed;
            }
        }
        if (float.TryParse(splitted[splitted.Length - 1], out number))
        {
            //text.text = number.ToString("#.#");
            text.text = number.ToString("#");
        }
        else
        {
            text.text = splitted[splitted.Length - 1];
        }

        text.transform.SetParent(transform.parent.parent);
        //text.transform.position = transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * 30f;
        var pos = transform.position + floatingTextPosition;
        if (Single.IsNaN(pos.x)) { pos = Vector3.zero; }
        text.transform.position = pos;
    }

}
