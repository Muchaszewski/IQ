using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class FloatingMessage
{

    public List<FloatingMessageData> AttackDatas = new List<FloatingMessageData>();

    [Serializable]
    public class FloatingMessageData
    {
        private EnumAttackMessage _message;
        private float _value;

        public EnumAttackMessage Message
        {
            get { return _message; }
            set { _message = value; }
        }

        public float Value
        {
            get { return _value; }
            set { _value = value; }
        }

        private FloatingMessageData() { }

        public FloatingMessageData(EnumAttackMessage message, float value = 0f)
        {
            _message = message;
            _value = value;
        }
    }

    public void Add(EnumAttackMessage message, float value = 0f)
    {
        AttackDatas.Add(new FloatingMessageData(message, value));
    }

    public FloatingMessageData this[int index]
    {
        get { return AttackDatas[index]; }
    }
}

public enum EnumAttackMessage
{
    Missed,
    Parried,
    Blocked,
    Exhausted,
    Absorb,
    Critical,
    FinalDamage,
    Experience,
}