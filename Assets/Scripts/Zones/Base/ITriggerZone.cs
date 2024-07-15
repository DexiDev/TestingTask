using System;

namespace Game.Zones
{
    public interface ITriggerZone
    {
        public event Action OnEnter;
        public event Action OnExit;

        public bool HasControllers();
    }
}