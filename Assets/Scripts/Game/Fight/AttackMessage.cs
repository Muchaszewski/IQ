using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class AttackMessage
{

    public List<AttackMessageData> AttackDatas = new List<AttackMessageData>();

    [Serializable]
    public class AttackMessageData
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

        private AttackMessageData() { }

        public AttackMessageData(EnumAttackMessage message, float value = 0f)
        {
            _message = message;
            _value = value;
        }
    }

    public void Add(EnumAttackMessage message, float value = 0f)
    {
        AttackDatas.Add(new AttackMessageData(message, value));
    }

    public AttackMessageData this[int index]
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
}