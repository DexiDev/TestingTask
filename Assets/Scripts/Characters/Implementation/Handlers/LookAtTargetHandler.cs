using Game.Core;

namespace Game.Characters.Handlers
{
    public class LookAtTargetHandler : IHandler<UnitController>
    {
        // [SerializeField] private bool _isDefaultReset;
        // [SerializeField, ShowIf("@_isDefaultReset")] private float _speedReset;
        //
        // private TargetField _targetFieldField;
        // private Quaternion _defaultRotation;
        //
        // private void Awake()
        // {
        //     _targetFieldField = _targetData.GetDataField<TargetField>(true);
        //     _defaultRotation = _targetData.transform.localRotation;
        // }
        //
        // private void LateUpdate()
        // {
        //     if (_targetFieldField.Value != null)
        //     {
        //         var direction = _targetFieldField.Value.position - _targetData.transform.position;
        //
        //         direction.y = 0f;
        //
        //         var rotation = Quaternion.LookRotation(direction);
        //
        //         _targetData.SetRotation(rotation);
        //     }
        //     else if (_isDefaultReset)
        //     {
        //         _targetData.transform.localRotation = Quaternion.RotateTowards(_targetData.transform.localRotation, _defaultRotation, _speedReset * Time.deltaTime);
        //     }
        // }
    }
}