using Unity.Mathematics;
using UnityEngine;

namespace Weapons.Attacks
{
    public class BasicSwing : Attack
    {
        private Vector3 _attackPosition;
        private readonly float _startingAngle;
        private readonly AnimationCurve _animationCurve;
        private readonly float _animationTime;
        private float _animationClock;
        public BasicSwing(float startingAngle,AnimationCurve curve)
        {
            _startingAngle = startingAngle;
            _animationCurve = curve;
            if (_animationCurve.length > 0)
                _animationTime = _animationCurve.keys[_animationCurve.length - 1].time;
        }
        public override void StartAttack(Weapon weapon)
        {
            WeaponGameObject = weapon.WeaponGameObject;
            StartingPosition = WeaponGameObject.transform.localPosition;
            WeaponGameObject.transform.localRotation = Quaternion.Euler(0, 0, _startingAngle);
            WeaponGameObject.transform.position += WeaponGameObject.transform.parent.up * weapon.Range;
            _attackPosition = WeaponGameObject.transform.position;
            weapon.SetCollision(true);
        }

        public override void ExecuteAttack(Weapon weapon)
        {
            _animationClock += Time.deltaTime;
            if (_animationClock < _animationTime)
            {
                WeaponGameObject.transform.localRotation = Quaternion.Euler(0, 0, _startingAngle+_animationCurve.Evaluate(_animationClock));
                WeaponGameObject.transform.position = _attackPosition;
            }
            else
            {
                _animationClock = 0;
                FinishAnimation.Invoke();
            }
        }

        public override void FinishAttack(Weapon weapon)
        {
            WeaponGameObject.transform.localRotation =quaternion.identity;
            WeaponGameObject.transform.localPosition =StartingPosition;
            weapon.SetCollision(false);
        }
    }
}