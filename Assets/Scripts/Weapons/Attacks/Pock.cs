using Unity.Mathematics;
using UnityEngine;
using Weapons.Attacks;

namespace Weapons
{
    public class Pock : Attack
    {
        private Vector3 _startingPosition;
        private float _maxDistance;
        private float _piercing;
        private float _toGo;
        private float _currentMag;
        public Pock(float maxDistance,int piercing)
        {
            _maxDistance = maxDistance;
        }
        public override void StartAttack(Weapon weapon)
        {
            Weapon = weapon;
            WeaponGO = Weapon.WeaponGameObject;
            WeaponGO.transform.position += WeaponGO.transform.parent.up * weapon._range;
            _startingPosition = Weapon.WeaponGameObject.transform.localPosition;
            _currentMag = 0;
            _toGo = WeaponGO.transform.localPosition.magnitude * _maxDistance;
        }

        public override void ExecuteAttack(Weapon weapon)
        {   
            if(_currentMag<_toGo)
            {
                _currentMag = Mathf.MoveTowards(_currentMag, _toGo, weapon._attackSpeed * Time.deltaTime);
                WeaponGO.transform.localPosition = WeaponGO.transform.localPosition.normalized*_currentMag;
            }
        }

        public override void FinishAttack(Weapon weapon)
        {
            WeaponGO.transform.localRotation = quaternion.identity;
            WeaponGO.transform.localPosition = _startingPosition;
        }
    }
}