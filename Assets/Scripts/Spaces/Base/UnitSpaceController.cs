using System;
using System.Collections.Generic;
using System.Linq;
using Game.Assets;
using Game.Characters;
using Game.Data;
using Game.Spaces.Fields;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.Spaces
{
    public class UnitSpaceController : DataController
    {
        [SerializeField] private UnitController _unitContract;
        [SerializeField] private int _limitSpawn;
        [SerializeField] private ISpawnBehaviour _spawnStrategy;
        [SerializeField] private Collider _spaceCollider;
        [SerializeField] private Collider _spaceEscapeCollider;
        
        private AssetsManager _assetsManager;
        private List<UnitController> _unitsPool = new ();

        public Collider SpaceCollider => _spaceCollider;
        public Collider SpaceEscapeCollider => _spaceEscapeCollider;
        
        public event Action<UnitController> OnUnitSpawn;
        public event Action<UnitController> OnUnitRelease;
        public event Action<DamageableAgentController> OnDamageableUnitKill;

        [Inject]
        private void Install(AssetsManager assetsManager, DiContainer diContainer)
        {
            _assetsManager = assetsManager;
            
            if(_spawnStrategy != null) diContainer.Inject(_spawnStrategy);
        }

        private void Awake()
        {
            _spawnStrategy?.Initialize(this);
        }

        protected virtual void OnEnable()
        {
            int targetCountSpawn = _limitSpawn;
            
            while (_unitsPool.Count < targetCountSpawn) Spawn();
            
            _spawnStrategy?.OnEnable();
        }

        protected virtual void OnDisable()
        {
            _unitsPool.ForEach(unit =>
            {
                unit.OnReleased -= UnitRelease;
                if (unit is DamageableAgentController damageableAgent) damageableAgent.OnKill -= OnDamageableAgentKill;
                _assetsManager.ReleaseAsset(unit);
            });
            _unitsPool.Clear();
            
            _spawnStrategy?.OnDisable();
        }

        public void Spawn()
        {
            if (_unitsPool.Count >= _limitSpawn) return;
            
            var unit = _assetsManager.GetAsset(_unitContract, transform);

            GetDataFields<IDataField>()?.ForEach(dataField => unit.GetDataField(dataField)?.SetInstance(dataField));
            
            unit.GetDataField<UnitSpaceField>(true).SetValue(this);
            
            unit.OnReleased += UnitRelease;

            if (unit is DamageableAgentController damageableAgent)
            {
                damageableAgent.OnKill += OnDamageableAgentKill;
            }
                
            _unitsPool.Add(unit);
            
            OnUnitSpawn?.Invoke(unit);
        }

        private void UnitRelease(IAsset asset)
        {
            asset.OnReleased -= UnitRelease;

            if (asset is UnitController unit)
            {
                _unitsPool.Remove(unit);
                OnUnitRelease?.Invoke(unit);
            }
        }

        private void OnDamageableAgentKill(DamageableAgentController damageableAgent)
        {
            OnDamageableUnitKill?.Invoke(damageableAgent);
        }

        public int GetCountKills()
        {
            return _limitSpawn - _unitsPool.Count(unit => unit is DamageableAgentController { IsAlive: true });
        }
    }
}