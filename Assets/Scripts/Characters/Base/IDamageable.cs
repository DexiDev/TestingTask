using Game.Characters.Fields.Stats;
using Game.Data;
using Game.Data.Fields;
using UnityEngine;

namespace Game.Characters
{
    public interface IDamageable : IDataController, ICharacter
    {
        Transform Instance { get; }
        HealthField HealthField { get; }
        public IsBattleStateField IsBattleStateField { get; }

        bool IsAlive { get; }

        void Damage(float count);

        bool IsCanKill(float count);

        
        //TODO: bad name
        void SetBattleState(bool IsBattle);
    }
}