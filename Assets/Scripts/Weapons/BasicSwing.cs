using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Weapons
{
    public class BasicSwing : Attack
    {
        public float RotationStartingPoint;
        public float SwingAngle;
        public float AttackSpeed;
        public float Range;
        public GameObject weapon;
        public WeaponTest test;
        public bool attack=false;
        public AnimationCurve curve;
        private Vector3 _startingPosition;
        private Vector3 _startingRotation;
        private bool _oneTime =true;
        private InputAction _attackAction;
        [SerializeField]private PlayerInput _playerInput;
        float currentAngle;
        float targetAngle ;
        float speed = 270f;
        private Vector3 parentStartingAngle;

        private void Start()
        {
            var actionMap = _playerInput.actions.FindActionMap("Player");
            _attackAction = actionMap.FindAction("Attack");
        }

        private void Update()
        {
            if (_attackAction.triggered)
            {
                attack = true;
            }
            if ( attack)
            {
                if (_oneTime)
                {
                    _startingPosition = transform.localPosition;
                    weapon.transform.position += transform.up * Range;
                    transform.localRotation = Quaternion.Euler(0, 0, RotationStartingPoint);
                    currentAngle = RotationStartingPoint;
                    _startingRotation = weapon.transform.localEulerAngles;
                    _oneTime = false;
                    targetAngle = RotationStartingPoint + SwingAngle;
                    parentStartingAngle = transform.parent.eulerAngles;
                }
                if(currentAngle<targetAngle)
                {
                    currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, AttackSpeed * Time.deltaTime);
                    transform.localRotation = Quaternion.Euler(0, 0, currentAngle+parentStartingAngle.z-transform.parent.rotation.eulerAngles.z);
                    transform.parent.eulerAngles = parentStartingAngle;

                }
                else
                {
                    attack = false;
                    _oneTime = true;
                    weapon.transform.localRotation =quaternion.identity;
                    weapon.transform.localPosition =_startingPosition;
                }
            }
        }

        [ContextMenu("swing")]
        public override void MakeAttack()
        {
            base.MakeAttack();
            test.corrutineUser(SwingAction());
        }
        IEnumerator SwingAction()
        {
            Debug.Log("Test");
            float initialZRot =weapon.transform.rotation.eulerAngles.z;
            while (Mathf.Abs(weapon.transform.rotation.eulerAngles.z)<(initialZRot+SwingAngle))
            {
                weapon.transform.Rotate(0,0,AttackSpeed*Time.deltaTime);
                yield return 0;
            }
        
        }
        float NormalizeAngle(float angle)
        {
            angle %= 360f;
            if (angle > 360f) angle -= 360f;
            return angle;
        }
    }
    
}