using System;
using Game.Assets;
using Game.Data;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Characters
{
    public abstract class UnitController : DataController, ICharacter
    {
        [SerializeField, TabGroup("Component")] protected Animator _animator;
        [SerializeField, TabGroup("Animation")] private string _isMovingKey = "IsMoving";
        [SerializeField, TabGroup("Animation")] private string _speedKey = "Speed";
        
        public Animator Animator => _animator;
        public Transform Transform => transform;

        public GameObject Asset => gameObject;
        public event Action<IAsset> OnReleased;
        
        protected virtual void Awake()
        {
            
        }

        protected virtual void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            OnReleased?.Invoke(this);
        }

        public virtual void Move(Vector3 direction)
        {
            if (direction == Vector3.zero) return;

            transform.position += direction;
        }

        public virtual void SetRotation(Quaternion targetRotation)
        {
            transform.rotation = targetRotation;
        }
        
        public virtual void SetMotionMove(bool isMoving, float speed = 0f)
        {
            if (_animator == null) return;
            
            _animator.SetBool(_isMovingKey, isMoving);
            _animator.SetFloat(_speedKey, speed);
        }
        
        public void SetAnimator(Animator animator)
        {
            _animator = animator;
        }
    }
}