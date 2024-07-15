namespace Game.Spaces
{
    public interface ISpawnBehaviour
    {
        void Initialize(UnitSpaceController unitSpaceController);

        void OnEnable();

        void OnDisable();
    }
}