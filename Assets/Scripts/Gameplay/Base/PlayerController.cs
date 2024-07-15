using System.Collections.Generic;
using Game.Assets;
using Game.Characters;
using Game.Data.Fields;
using Game.Gameplay.TriggerZones;
using MimicSpace;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    public class PlayerController : AgentController
    {
        [SerializeField, TabGroup("Component")] private Mimic _mimic;
        [SerializeField, TabGroup("Component")] private TriggerZoneEnemy _triggerZone;

        [Inject] private DiContainer _diContainer;
        
        private Dictionary<IDamageable,AttackerEntity> _damagableEntities = new();
        private Dictionary<AttackerEntity, IDamageable> _attackerEntities = new();
        private VelocityField _velocityField;

        protected override void Awake()
        {
            _velocityField = GetDataField<VelocityField>(true);
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _triggerZone.Controllers.ForEach(controller => OnEnemyEnter(controller.Value));
            _triggerZone.OnControllerEnter += OnEnemyEnter;
            _triggerZone.OnControllerExit += OnEnemyExit;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            
            _triggerZone.Controllers.ForEach(controller => OnEnemyExit(controller.Value));
            _triggerZone.OnControllerEnter -= OnEnemyEnter;
            _triggerZone.OnControllerExit -= OnEnemyExit;
        }

        public override void SetMotionMove(bool isMoving, float speed = 0)
        {
            _velocityField.SetValue(speed);
            base.SetMotionMove(isMoving, speed);
        }
        
        protected virtual void OnEnemyEnter(EnemyController enemy)
        {
            if (!enemy.IsAlive || _damagableEntities.ContainsKey(enemy)) return;

            var leg = _mimic.RequestLeg(enemy.transform.position);

            var attackerEntity = leg.GetComponent<AttackerEntity>();
            
            _diContainer.InjectGameObject(attackerEntity.gameObject);
            
            attackerEntity.Initialize(this);
            
            attackerEntity.SetTarget(enemy);

            attackerEntity.OnReleased += OnAttackerRelease;

            enemy.OnReleased += OnEnemyRelease;
            
            _damagableEntities.Add(enemy, attackerEntity);
            _attackerEntities.Add(attackerEntity, enemy);
        }

        private void OnEnemyExit(EnemyController enemy)
        {
            if (!enemy.IsAlive) return;
            
            if (_damagableEntities.Remove(enemy, out var attackerEntity))
            {
                attackerEntity.SetTarget(null);
            }
        }

        private void OnAttackerRelease(IAsset asset)
        {
            asset.OnReleased -= OnAttackerRelease;
            
            if (asset is AttackerEntity attackerEntity)
            {
                if (_attackerEntities.Remove(attackerEntity, out var damageable))
                {
                    _damagableEntities.Remove(damageable);
                }
            }
        }

        private void OnEnemyRelease(IAsset asset)
        {
            asset.OnReleased -= OnEnemyRelease;
            
            if (asset is IDamageable damageable)
            {
                if (_damagableEntities.Remove(damageable, out var attackerEntity))
                {
                    attackerEntity.SetTarget(null);
                }
            }
        }
    }
}