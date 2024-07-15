using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Zones
{
    public abstract class TriggerZone<TController> : MonoBehaviour, ITriggerZone
    {
        [SerializeField] protected UnityEvent EnterAction;
        [SerializeField] protected UnityEvent ExitAction;
        
        protected Dictionary<Collider, TController> _controllers = new ();

        public Dictionary<Collider, TController> Controllers => _controllers;

        public event Action OnEnter;
        public event Action OnExit;

        public event Action<TController> OnControllerEnter;
        public event Action<TController> OnControllerExit;

        protected virtual  void OnEnable()
        {
            
        }

        protected virtual void OnDisable()
        {
            _controllers.ToArray().ForEach(controller => Exit(controller.Key, controller.Value));
            _controllers.Clear();
        }

        protected void OnTriggerEnter(Collider collider)
        {
            if (!_controllers.ContainsKey(collider) && TryGetController(collider, out TController controller))
            {
                Enter(collider, controller);
            }
        }

        protected void OnTriggerExit(Collider collider)
        {
            if (_controllers.TryGetValue(collider, out var controller))
            {
                Exit(collider, controller);
            }
        }

        private void LateUpdate()
        {
            if (_controllers.Count > 0)
            {
                var controllers = _controllers.Where(controller => !controller.Key.enabled);
                if (controllers.Any())
                {
                    controllers.ToArray().ForEach(controller => Exit(controller.Key, controller.Value));   
                }
            }
        }

        protected virtual void Enter(Collider collider, TController controller)
        {
            _controllers.Add(collider, controller);
            OnEnter?.Invoke();
            OnControllerEnter?.Invoke(controller);
            EnterAction?.Invoke();
        }

        protected virtual void Exit(Collider collider, TController controller)
        {
            _controllers.Remove(collider);
            OnExit?.Invoke();
            OnControllerExit?.Invoke(controller);
            ExitAction?.Invoke();
        }

        private bool TryGetController(Collider collider, out TController controller)
        {
            controller = collider.GetComponentInParent<TController>();
            
            return controller != null;
        }
        
        public virtual bool HasControllers()
        {
            return _controllers.Count > 0;
        }
    }
}