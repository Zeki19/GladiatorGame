using Unity.Mathematics;
using UnityEngine;

namespace Weapons
{
    public class BasicSwing : Attack
    {
        private GameObject _weapon;
        private Vector3 _startingPosition;
        private Vector3 _attackPosition;
        private readonly float _startingOffset;
        private float _currentAngle;
        private float _targetAngle;
        private readonly float _swingAngle;
        
        public BasicSwing(float startingOffset, float swingAngle)
        {
            _startingOffset = startingOffset;
            _swingAngle = swingAngle;
        }
        public override void StartAttack(Weapon weapon)
        {
            _weapon = weapon.WeaponGameObject;
            _startingPosition = _weapon.transform.localPosition;
            _currentAngle = _startingOffset;
            _targetAngle = _startingOffset + _swingAngle;
            _weapon.transform.localRotation = Quaternion.Euler(0, 0, _startingOffset);
            _weapon.transform.position += _weapon.transform.parent.up * weapon.Range;
            _attackPosition = _weapon.transform.position;
            weapon.SetCollision(true);
        }

        public override void ExecuteAttack(Weapon weapon)
        {
            if(_currentAngle<_targetAngle)
            {
                _currentAngle = Mathf.MoveTowards(_currentAngle, _targetAngle, weapon.AttackSpeed * Time.deltaTime);
                _weapon.transform.localRotation = Quaternion.Euler(0, 0, _currentAngle);
                _weapon.transform.position = _attackPosition;
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