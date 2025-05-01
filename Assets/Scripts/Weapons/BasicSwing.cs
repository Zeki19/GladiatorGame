using Unity.Mathematics;
using UnityEngine;

namespace Weapons
{
    public class BasicSwing : Attack
    {
        private GameObject _weapon;
        private Vector3 _startingPosition;
        private Vector3 _attackPosition;
        private float _rotationStartingPoint;
        private float _currentAngle;
        private float _targetAngle;
        private float _swingAngle;
        private bool _oneTime = true;
        
        public BasicSwing(float startingPoint, float swingAngle)
        {
            _rotationStartingPoint = startingPoint;
            _swingAngle = swingAngle;
        }
        public override void MakeAttack(Weapon weapon)
        {
            if (_oneTime)
            {
                _oneTime = false;
                _weapon = weapon.WeaponGameObject;
                _startingPosition = _weapon.transform.localPosition;
                _currentAngle = _rotationStartingPoint;
                _targetAngle = _rotationStartingPoint + _swingAngle;
                _weapon.transform.localRotation = Quaternion.Euler(0, 0, _rotationStartingPoint);
                _weapon.transform.position += _weapon.transform.up * weapon._range;
                _attackPosition = _weapon.transform.position;
            }
            if(_currentAngle<_targetAngle)
            {
                _currentAngle = Mathf.MoveTowards(_currentAngle, _targetAngle, weapon._attackSpeed * Time.deltaTime);
                _weapon.transform.localRotation = Quaternion.Euler(0, 0, _currentAngle);
                _weapon.transform.position = _attackPosition;
            }
            else
            {
                _oneTime = true;
                _weapon.transform.localRotation =quaternion.identity;
                _weapon.transform.localPosition =_startingPosition;
            }
        }
    }
}