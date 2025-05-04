using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons.AttackSo
{
    [Serializable]
    public abstract class AttackSo :ScriptableObject
    {
        public abstract Attack Clone();
    }
}