using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Weapons
{
    [Serializable]
    public abstract class SOAttack :ScriptableObject
    {
        public abstract Attack Clone();
    }
}