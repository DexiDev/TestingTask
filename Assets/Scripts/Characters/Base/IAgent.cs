using Game.Data;
using UnityEngine.AI;

namespace Game.Characters
{
    public interface IAgent : ICharacter, IDataController
    {
        NavMeshAgent NavMeshAgent { get; }
    }
}