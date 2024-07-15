using Game.Core;
using UnityEngine;

namespace Game.Characters.Handlers.Agent
{
    public class AgentMotionHandler : IHandler<IAgent>
    {
        private void LateUpdate()
        {
            var velocity = _targetData.NavMeshAgent.velocity;

            if (velocity != Vector3.zero)
            {
                velocity.Normalize();
                var speed = velocity.magnitude;
                _targetData.SetMotionMove(true, speed);
            }
            else _targetData.SetMotionMove(false);
        }
    }
}