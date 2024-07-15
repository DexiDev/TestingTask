using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Game.Core;
using Game.Data.Fields;
using UnityEngine;

namespace Game.Characters.Handlers.Agent
{
    public class AgentMoveHandler : IHandler<IAgent>
    {
        private MovePointField _movePointField;
        private CancellationTokenSource _cancellationToken;

        private void Awake()
        {
            _movePointField = _targetData.GetDataField<MovePointField>(true);
        }

        private void OnEnable()
        {
            _movePointField.OnChanged += OnChangedPoint;
            OnChangedPoint(_movePointField.Value);
        }

        private void OnDisable()
        {
            _movePointField.OnChanged -= OnChangedPoint;
            _cancellationToken?.Cancel();
        }

        private void OnChangedPoint(Vector3 point)
        {
            _cancellationToken?.Cancel();

            if (point == Vector3.zero)
            {
                _targetData.NavMeshAgent.ResetPath();
                return;
            }
            
            _cancellationToken = new();
            Move(point, _cancellationToken.Token);
        }

        private async void Move(Vector3 point, CancellationToken token)
        {
            var navAgent = _targetData.NavMeshAgent;

            try
            {
                await UniTask.WaitWhile(() => !navAgent.isActiveAndEnabled, cancellationToken: token);

                navAgent.SetDestination(point);

                await UniTask.WaitWhile(() => navAgent.pathPending, cancellationToken: token);

                await UniTask.WaitWhile(() => (navAgent.hasPath || navAgent.velocity.sqrMagnitude > 0) &&
                                              navAgent.remainingDistance > navAgent.stoppingDistance,
                    cancellationToken: token);

                if (navAgent.isActiveAndEnabled) navAgent.ResetPath();

                _movePointField.SetValue(Vector3.zero);
            }
            catch (OperationCanceledException)
            {
                
            }
        }
    }
}