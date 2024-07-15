using Game.Assets;
using UnityEngine;

namespace Game.Characters
{
    public interface ICharacter : IAsset
    {
        Transform Transform { get; }
        
        void Move(Vector3 direction);
        
        void SetRotation(Quaternion targetRotation);

        public void SetMotionMove(bool isMoving, float speed = 0f);
    }
}