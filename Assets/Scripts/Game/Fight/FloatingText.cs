using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public GameObject AnimationPrefab;
    public Vector2 InstantianePosition;
    public float Direction;

    public void CreateFloatingText(FloatingMessage message)
    {
        var text = Instantiate(AnimationPrefab).GetComponent<Text>();
        for (int i = 0; i < message.AttackDatas.Count; i++)
        {
            var item = message[i];
            if (item.Message == EnumAttackMessage.Experience)
            {
                text.color = text.GetComponent<FloatingTextAnimation>().ColorExperience;
                text.text = item.Value.ToString("#");
                continue;
            }

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
                text.text = item.Value.ToString("#");
            }
            else
            {
                text.text = item.Message.ToString();
            }
        }

        text.transform.SetParent(transform.parent.parent);
        //text.transform.position = transform.position + (Vector3)UnityEngine.Random.insideUnitCircle * 30f;
        var pos = transform.position + (Vector3)InstantianePosition;
        if (float.IsNaN(pos.x)) { pos = new Vector3(-1000, -1000); }
        text.transform.position = pos;
        if (Direction != 0)
        {
            text.GetComponent<FloatingTextAnimation>().Direction = Direction;
        }
    }
}
