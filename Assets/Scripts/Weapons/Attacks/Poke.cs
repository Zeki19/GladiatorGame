using Unity.Mathematics;
using UnityEngine;

namespace Weapons.Attacks
{
    public class Poke : Attack
    {
        private GameObject _weapon;
        private Vector3 _startingPosition;
        private float _maxDistance;
        private float _piercing;
        private float _toGo;
        private float _currentMag;
        public Poke(float maxDistance,int piercing)
        {
            _maxDistance = maxDistance;
        }
        public override void StartAttack(Weapon weapon)
        {
            _weapon = weapon.WeaponGameObject;
            _startingPosition = _weapon.transform.localPosition;
            _weapon.transform.position += _weapon.transform.parent.up * weapon._range;
            _currentMag = 0;
            _toGo= _weapon.transform.localPosition.magnitude * _maxDistance;
            weapon.SetCollision(true);
        }

        public override void ExecuteAttack(Weapon weapon)
        {   
            if(_currentMag<_toGo)
            {
                _currentMag = Mathf.MoveTowards(_currentMag, _toGo, weapon._attackSpeed * Time.deltaTime);
                _weapon.transform.localPosition = _weapon.transform.localPosition.normalized*_currentMag;
            }
            else
            {
                FinishAnimation.Invoke();
            }
        }

        public override void FinishAttack(Weapon weapon)
        {
            _weapon.transform.localRotation =quaternion.identity;
            _weapon.transform.localPosition =_startingPosition;
            weapon.SetCollision(false);
        }
    }
}