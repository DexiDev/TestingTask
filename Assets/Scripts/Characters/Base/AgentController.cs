using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

namespace Game.Characters
{
    [RequireComponent(typeof(NavMeshAgent))]
    public abstract class AgentController : UnitController, IAgent
    {
        [SerializeField, TabGroup("Component")] protected NavMeshAgent _navMeshAgent;

        public float Speed => _navMeshAgent.angularSpeed;
        public NavMeshAgent NavMeshAgent => _navMeshAgent;

        public override void Move(Vector3 direction)
        {
            if (direction == Vector3.zero) return;
            
            _navMeshAgent.Move(direction * _navMeshAgent.speed);
        }
    }
}