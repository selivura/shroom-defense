using System.Collections.Generic;
using UnityEngine;
using VContainer;

namespace ShroomDefense.Gameplay
{
    [RequireComponent(typeof(Unit))]
    public class StatusEffectsController : MonoBehaviour
    {
        [SerializeField] private List<StatusEffect> _appliedEffects = new();
        public IReadOnlyList<StatusEffect> AppliedEffects => _appliedEffects;

        private Unit _unit;
        private IObjectResolver _resolver;

        [Inject]
        public void Construct(IObjectResolver resolver)
        {
            _resolver = resolver;
        }
        private void Awake()
        {
            _unit = GetComponent<Unit>();
        }
        private void OnEnable()
        {
            _unit.OnBeforeExecutingActions.AddListener(BeforeExecutingActions);
            _unit.OnAfterExecutingActions.AddListener(AfterExecutingActions);
        }
        private void OnDisable()
        {
            _unit.OnAfterExecutingActions.RemoveListener(AfterExecutingActions);
            _unit.OnBeforeExecutingActions.RemoveListener(BeforeExecutingActions);
        }

        private void AfterExecutingActions()
        {
            foreach (var effect in _appliedEffects.ToArray())
            {
                effect.OnUnitAfterActionsExecuted();
                effect.DurationTurns--;
                if (effect.DurationTurns <= 0)
                {
                    RemoveEffect(effect);
                }
            }
        }

        private void BeforeExecutingActions()
        {
            foreach (var effect in _appliedEffects.ToArray())
            {
                effect.OnUnitBeforeActionsExecuted();
            }
        }

        public void AddEffect(StatusEffect statusEffect)
        {
            if(_appliedEffects.Exists(e => e.GetType() == statusEffect.GetType()))
            {
                var existingEffect = _appliedEffects.Find(e => e.GetType() == statusEffect.GetType());
                if (existingEffect.HandleAnotherInstance(statusEffect))
                {
                    return;
                }
            }

            _resolver.Inject(statusEffect);
            statusEffect.Apply(_unit);
            _appliedEffects.Add(statusEffect);
        }

        public void RemoveEffect(StatusEffect statusEffect)
        {
            statusEffect.OnExpire();
            _appliedEffects.Remove(statusEffect);
        }
    }
}
