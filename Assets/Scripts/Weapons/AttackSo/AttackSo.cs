using System;
using UnityEngine;

namespace Weapons.AttackSo
{
    [Serializable]
    public abstract class AttackSo :ScriptableObject
    {
        public abstract Attack Clone();
    }
}