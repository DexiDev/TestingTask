using Game.Characters;
using Game.Data.Fields;
using Game.Rewards;
using Sirenix.Utilities;
using UnityEngine;
using Zenject;

namespace Game.Gameplay
{
    [RequireComponent(typeof(Collider))]
    public class EnemyController : DamageableAgentController
    {
        [Inject] private RewardsManager _rewardsManager;

        private IRewardField[] _rewardFields;
        
        private MovePointField _movePointField;
        
        private float _speed;
        
        protected override void Awake()
        {
            _speed = _navMeshAgent.angularSpeed;
            _movePointField = GetDataField<MovePointField>(true);
            base.Awake();
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            _rewardFields = GetDataFields<IRewardField>();   
            OnFieldChanged += OnFieldChangedHandler;
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            OnFieldChanged -= OnFieldChangedHandler;
            if (!IsAlive) _rewardFields?.ForEach(rewardField => _rewardsManager.Reward(rewardField));
            SetBattleState(false);
        }

        public override void SetBattleState(bool isBattle)
        {
            base.SetBattleState(isBattle);
            _movePointField.SetValue(Vector3.zero);
        }

        private void OnFieldChangedHandler()
        {
            _rewardFields = GetDataFields<IRewardField>();
        }

        private void LateUpdate()
        {
            if (IsAlive)
            {
                var speed = _navMeshAgent.angularSpeed / _speed;
                SetMotionMove(true, speed);
            }
        }
    }
}