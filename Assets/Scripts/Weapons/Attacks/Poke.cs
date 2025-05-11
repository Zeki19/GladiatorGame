using System.Timers;
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
        //private float _currentMag;
        private AnimationCurve _curve;
        private float animationTime;
        private float _timer;

        public Poke(float maxDistance,int piercing,AnimationCurve curve)
        {
            _maxDistance = maxDistance;
            _curve = curve;
            if (_curve.length > 0)
                animationTime = _curve.keys[_curve.length - 1].time;
        }
        public override void StartAttack(Weapon weapon)
        {
            _weapon = weapon.WeaponGameObject;
            _startingPosition = _weapon.transform.localPosition;
            _weapon.transform.position += _weapon.transform.parent.up * weapon.Range;
            //_currentMag = 0;
            _toGo= _weapon.transform.localPosition.magnitude * _maxDistance;
            weapon.SetCollision(true);
        }

        public override void ExecuteAttack(Weapon weapon)
        {   
            _timer += Time.deltaTime;
            if (_timer < animationTime)
            {
                _weapon.transform.localPosition =
                    _weapon.transform.localPosition.normalized *_curve.Evaluate(_timer);
            }
            else
            {
                _timer = 0;
                FinishAnimation.Invoke();
            }
            //if(_currentMag<_toGo)
            //{
            //    _currentMag = Mathf.MoveTowards(_currentMag, _toGo, weapon._attackSpeed * Time.deltaTime);
            //    _weapon.transform.localPosition = _weapon.transform.localPosition.normalized*_currentMag;
            //}
            //else
            //{
            //    FinishAnimation.Invoke();
            //}
        }

        public override void FinishAttack(Weapon weapon)
        {
            _weapon.transform.localRotation =quaternion.identity;
            _weapon.transform.localPosition =_startingPosition;
            weapon.SetCollision(false);
        }
    }
}