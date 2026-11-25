using NaughtyAttributes;
using ShroomDefense.Gameplay;
using ShroomDefense.UI;
using UnityEngine;
using UnityEngine.Events;
using VContainer;

namespace ShroomDefense.Gameplay
{
    public enum ToolTargetSelectType
    {
        Any,
        OccupiedCell,
        EmptyCell,
    }
    public enum FieldTargetSelectType
    {
        Player,
        Enemy,
        Any,
    }

    public class ToolTargetSelector : MonoBehaviour
    {
        [SerializeField] private ToolTargetSelectType _selectorType = ToolTargetSelectType.Any;
        [SerializeField] private FieldTargetSelectType _selectorFieldType = FieldTargetSelectType.Any;
        [SerializeField] [Required] private TargetSelector _targetSelector;
        public UnityEvent<CardTargetData> OnTargetSelected;
        public UnityEvent<CardTargetData> OnInvalidTargetSelected;
        public UnityEvent OnCancelSelect;

        private BattleMap _battleMap;
        private PopupFactory _popupFactory;

        [Inject]
        public void Construct(BattleMap battleMap, PopupFactory popupFactory)
        {
            _battleMap = battleMap;
            _popupFactory = popupFactory;
        }

        public void BeginSelectTarget()
        {
            _targetSelector.StartSelecting();
            _targetSelector.OnTargetPicked.AddListener(SelectTarget);
        }

        /// <summary>
        /// Pass empty targetData to cancel selecting
        /// </summary>
        /// <param name="targetData"></param>
        /// <returns></returns>
        public void SelectTarget(CardTargetData targetData)
        {
            _targetSelector.OnTargetPicked.RemoveListener(SelectTarget);
            string failureReason = "";
            if (targetData == null)
            {
                OnCancelSelect.Invoke();
                return;
            }
            if (!_battleMap.ValidatePosition(targetData.MapPostion))
            {
                OnInvalidTargetSelected.Invoke(targetData);
                return;
            }
            if(!CheckFieldType(targetData, ref failureReason))
            {
                OnInvalidTargetSelected.Invoke(targetData);
                _popupFactory.Create(failureReason, Color.red, _targetSelector.transform.position);
                return;
            }
            if (!CheckSelectorType(targetData, ref failureReason))
            {
                OnInvalidTargetSelected.Invoke(targetData);
                _popupFactory.Create(failureReason, Color.red, _targetSelector.transform.position);
                return;
            }

            OnTargetSelected.Invoke(targetData);
            _popupFactory.Create("Target selected!", Color.green, _targetSelector.transform.position);
        }

        private bool CheckFieldType(CardTargetData targetData, ref string failureReason)
        {
            if (_selectorFieldType != FieldTargetSelectType.Any)
            {
                if (_selectorFieldType == FieldTargetSelectType.Enemy)
                {
                    if (targetData.MapPostion.x < 0)
                    {
                        failureReason = "Target must be on enemy field";
                        return false;
                    }
                }
                if (_selectorFieldType == FieldTargetSelectType.Player)
                {
                    if (targetData.MapPostion.x > 0)
                    {
                        failureReason = "Target must be on player field";
                        return false;
                    }
                }
            }

            return true;
        }

        private bool CheckSelectorType(CardTargetData targetData, ref string failureReason)
        {
            switch (_selectorType)
            {
                case ToolTargetSelectType.Any:
                    return true;

                case ToolTargetSelectType.OccupiedCell:
                    if (!_battleMap.IsOccupied(targetData.MapPostion))
                    {
                        failureReason = "Target cell must be occupied";
                        return false;
                    }
                    return true;

                case ToolTargetSelectType.EmptyCell:
                    if (_battleMap.IsOccupied(targetData.MapPostion))
                    {
                        failureReason = "Target cell must be empty";
                        return false;
                    }
                    return true;

                default:
                    return false;
            }
        }
    }
}
