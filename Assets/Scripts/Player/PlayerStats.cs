using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    [Serializable]
    public class PlayerStats
    {
        [Header("Health")] 
        [SerializeField] private float maxHealth;
        public float MaxHealth => maxHealth;
        public int[] stateThreshold;
        public float IFrames;
        #region SpeedVariables

        [Header("Speed")] 
        [SerializeField] private float moveSpeed;
        public float MoveSpeed => moveSpeed;
        private float _speedModifier = 1;

        public float SpeedModifier
        {
            get => _speedModifier;
            set => _speedModifier = value;
        }

        #endregion

        #region DashProperties

        [Header("Dash")] [SerializeField] private float dashForce;
        public float DashForce => dashForce;
        [SerializeField] private float dashDuration;
        public float DashDuration => dashDuration;
        [SerializeField] private float dashCooldown;
        public float DashCooldown => dashCooldown;
        [SerializeField] private float dashInvincibility;
        public float DashInvincibility => dashInvincibility;

        #endregion

        #region KnockBackVariables

        [Header("KnockBack")] 
        [SerializeField] private float knockbackWeight;
        public float KnockbackWeight => knockbackWeight;
        public bool canBeKnockedBack;

        #endregion
    }
}