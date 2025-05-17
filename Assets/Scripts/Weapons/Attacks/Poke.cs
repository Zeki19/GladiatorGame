using System.Timers;
using Unity.Mathematics;
using UnityEngine;

namespace Weapons.Attacks
{
    public class Poke : Attack
    {
        private float _piercing;
        private readonly AnimationCurve _animationCurve;
        private readonly float _animationTime;
        private float _animationClock;

        public Poke(int piercing,AnimationCurve curve)
        {
            _animationCurve = curve;
            if (_animationCurve.length > 0)
                _animationTime = _animationCurve.keys[_animationCurve.length - 1].time;
        }
        public override void StartAttack(Weapon weapon)
        {
            WeaponGameObject = weapon.WeaponGameObject;
            StartingPosition = WeaponGameObject.transform.localPosition;
            WeaponGameObject.transform.position += WeaponGameObject.transform.parent.up * weapon.Range;
            weapon.SetCollision(true);
        }

        public override void ExecuteAttack(Weapon weapon)
        {   
            _animationClock += Time.deltaTime;
            if (_animationClock < _animationTime)
            {
                WeaponGameObject.transform.localPosition =
                    WeaponGameObject.transform.localPosition.normalized *_animationCurve.Evaluate(_animationClock);
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