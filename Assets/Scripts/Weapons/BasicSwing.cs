using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Weapons
{
    public class BasicSwing : Attack
    {
        private GameObject _weapon;
        [SerializeField] private float rotationStartingPoint;
        [SerializeField] private float swingAngle;
        private bool _attack;
        private bool _oneTime =true;
        private Vector3 _startingPosition;
        private Vector3 _attackPosition;
        private InputAction _attackAction;
        private float _currentAngle;
        private float _targetAngle ;
        private Vector3 _parentStartingAngle;
        
        public BasicSwing(float StartingPoint, float swingAngle)
        {
            rotationStartingPoint = StartingPoint;
            this.swingAngle = swingAngle;
        }
        public override void MakeAttack(Weapon weapon)
        {
            if (_oneTime)
            {
                _weapon = weapon.WeaponGameObject;
                _oneTime = false;
                _startingPosition = _weapon.transform.localPosition;
                _currentAngle = rotationStartingPoint;
                _targetAngle = rotationStartingPoint + swingAngle;
                _weapon.transform.localRotation = Quaternion.Euler(0, 0, rotationStartingPoint);
                _weapon.transform.position += _weapon.transform.up * weapon._range;
                _parentStartingAngle = _weapon.transform.parent.eulerAngles;
                _attackPosition = _weapon.transform.position;
            }
            if(_currentAngle<_targetAngle)
            {
                _currentAngle = Mathf.MoveTowards(_currentAngle, _targetAngle, weapon._attackSpeed * Time.deltaTime);
                _weapon.transform.localRotation = Quaternion.Euler(0, 0, _currentAngle);
                var offset =_parentStartingAngle-_weapon.transform.parent.eulerAngles;
                _weapon.transform.localEulerAngles += offset;
                _weapon.transform.position = _attackPosition;
            }
            else
            {
                _attack = false;
                _oneTime = true;
                _weapon.transform.localRotation =quaternion.identity;
                _weapon.transform.localPosition =_startingPosition;
            }
        }

        private Transform Attack(float range, float attackSpeed)
        {
            if (_attackAction.triggered)
            {
                _attack = true;
            }
            if ( _attack)
            {
                
            }

            return _weapon.transform;
        }
    }
    
}