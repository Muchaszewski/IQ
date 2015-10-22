﻿using System;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using Assets.Scripts.Utils;
using InventoryQuest.Components.Entities;
using InventoryQuest.Game;
using InventoryQuest.Game.Fight;
using InventoryQuest.Utils;
using UnityEngine.UI;

public class Enemy : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{

    public const float MinCombatPosition = 94f;

    public float MaxCombatPosition = 241f;
    public bool IsRightSide = true;
    public int EntityID;
    public GameObject FloatingText;
    public Vector3 floatingTextPosition;

    public float ShrinkedScale = 0.8f;
    public float ShrinkStep = 1f;
    public float GrowScale = 1.1f;
    public float GrowStep = 1f;
    private float _normalScale;
    private bool _isDownScaling;
    private bool _isUpSacling;
    private bool _isHoovering;

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
        SetIcon();
        _normalScale = transform.localScale.x;
    }

    void SetIcon()
    {
        if (EntityData.ImageID != null)
        {
            var path =
                ResourcesNames.MonstersImageNames[EntityData.ImageID.ImageIDType].FullNameList[
                    EntityData.ImageID.ImageIDItem];
            GetComponent<Image>().sprite = ResourceManager.Get(path);
        }
        else
        {
            Debug.LogWarning("Entity " + EntityData.Name + " has no image. Displaying Default");
        }
    }

    void Update()
    {
        // Death
        if (EntityData.Stats.HealthPoints.Current <= 0)
        {
            Destroy(this.gameObject);
        }

        // Init
        _progress = EntityData.Position;
        float position = 0;

        // Update position
        position = Mathf.Sqrt(Math.Abs(_progress)) * 9; // Math.Sqrt returns a NaN for negative numbers

        //Debug.Log("Pos: " + Math.Round(position) + ", Min: " + MinCombatPosition);
        if (_progress >= 0)
        {
            position = position + MinCombatPosition;
        }
        else if (_progress < 0)
        {
            position = -position - MinCombatPosition;
        }


        // Enemey out of bounds
        position = Mathf.Clamp(position, -MaxCombatPosition, MaxCombatPosition);

        // Apply position
        _rectTransform.anchoredPosition = new Vector2(position, 0);

        UpdateScale();
    }

    void UpdateScale()
    {
        if (_isDownScaling)
        {
            var shrink = ShrinkStep * Time.deltaTime;
            transform.localScale -= new Vector3(shrink, shrink, 0);
            if (transform.localScale.x < ShrinkedScale)
            {
                transform.localScale = new Vector3(ShrinkedScale, ShrinkedScale, 1);
                _isDownScaling = false;
                if (_isHoovering)
                {
                    _isUpSacling = true;
                }
            }
        }
        else if (_isUpSacling)
        {
            var grow = GrowStep * Time.deltaTime;
            transform.localScale += new Vector3(grow, grow, 0);
            if (transform.localScale.x > GrowScale)
            {
                transform.localScale = new Vector3(GrowScale, GrowScale, 1);
            }
        }
        else
        {
            if (transform.localScale.x > _normalScale)
            {
                var normal = ShrinkStep * Time.deltaTime;
                transform.localScale -= new Vector3(normal, normal, 0);
                if (transform.localScale.x < _normalScale)
                {
                    transform.localScale = new Vector3(_normalScale, _normalScale, 1);
                    _isDownScaling = false;
                    _isUpSacling = false;
                }
            }
            else if (transform.localScale.x < _normalScale)
            {
                var normal = GrowStep * Time.deltaTime;
                transform.localScale += new Vector3(normal, normal, 0);
                if (transform.localScale.x > _normalScale)
                {
                    transform.localScale = new Vector3(_normalScale, _normalScale, 1);
                    _isDownScaling = false;
                    _isUpSacling = false;
                }
            }
        }
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
        _isDownScaling = true;
        _isUpSacling = false;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        _isUpSacling = true;
        _isHoovering = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isUpSacling = false;
        _isHoovering = false;
    }

    public void CreateFloatingText(AttackMessage message)
    {
        var text = Instantiate(FloatingText).GetComponent<Text>();
        for (int i = 0; i < message.AttackDatas.Count; i++)
        {
            var item = message[i];
            /*
            /m missed
            /p parried
            /b blocked
            /e exhausted
            /a absorb
            /c critical
            */
            if (item.Message == EnumAttackMessage.Missed)
            {
                text.color = text.GetComponent<FloatingTextAnimation>().ColorMissed;
            }
            else if (item.Message == EnumAttackMessage.Critical)
            {
                if (item.Value == 0)
                {
                    text.color = text.GetComponent<FloatingTextAnimation>().ColorDamage;
                }
                else
                {
                    text.color = text.GetComponent<FloatingTextAnimation>().ColorCritical;

                }
            }
            else if (item.Message == EnumAttackMessage.Parried)
            {
                text.color = text.GetComponent<FloatingTextAnimation>().ColorParried;
            }
            else if (item.Message == EnumAttackMessage.Blocked)
            {
                text.color = text.GetComponent<FloatingTextAnimation>().ColorBlocked;
            }
            else if (item.Message == EnumAttackMessage.Exhausted)
            {
                text.color = text.GetComponent<FloatingTextAnimation>().ColorExhausted;
            }
            else if (item.Message == EnumAttackMessage.Absorb)
            {
                text.color = text.GetComponent<FloatingTextAnimation>().ColorAbsorbed;
            }
            if (item.Message == EnumAttackMessage.FinalDamage)
            {
                //text.text = number.ToString("#.#");
                text.text = item.Value.ToString("#");
            }
            else
            {
                text.text = item.Message.ToString();
            }
        }

        text.transform.SetParent(transform.parent.parent);
        //text.transform.position = transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * 30f;
        var pos = transform.position + floatingTextPosition;
        if (Single.IsNaN(pos.x)) { pos = new Vector3(-1000, -1000); }
        text.transform.position = pos;
    }

}
