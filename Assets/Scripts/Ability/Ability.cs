using System.Collections;
using Characters.Chicken;
using ScriptableObjects;
using UnityEngine;

namespace Ability
{
    public abstract class Ability : MonoBehaviour
    {
        [SerializeField] private AbilityStats stats;
        
       protected Chicken Owner;
       protected Animator AnimationController;
       
       private bool _isReady = true; // Default to true because we should start as ready.
       private bool _isBeingHeld;
       private float _currentCooldownTime;

       private Coroutine _cooldownTimer;
       private WaitUntil _cachedCanActivate;

       private void Start()
       {
           Owner = GetComponentInParent<Chicken>();
           AnimationController = GetComponentInChildren<Animator>();
           _cachedCanActivate = new WaitUntil(CanActivate);
       }

        public void InputStateChanged(bool state)
        {
            _isBeingHeld = state;
            
            if(_isBeingHeld && _cooldownTimer == null) _cooldownTimer = StartCoroutine(HandleCooldown());
        }

        protected virtual void ActivateAbility()
        {
            //Update animations, called in HandleCooldown
        }
        
        protected virtual void EndAbility()
        {
            //Update animations, called in HandleCooldown
        }

        protected abstract void ExecuteAbility();

        private IEnumerator HandleCooldown()
        {
            ActivateAbility();
            do
            {
                yield return _cachedCanActivate;
                if(!_isBeingHeld) break;
                ExecuteAbility();
                

                _isReady = false;
                _currentCooldownTime = 0;
                while (_currentCooldownTime < stats.Cooldown)
                {
                    _currentCooldownTime += Time.deltaTime;
                    yield return null;
                }
                _currentCooldownTime = stats.Cooldown;

                _isReady = true;
            } while (_isBeingHeld && stats.CanBeHeld);
            EndAbility();
            _cooldownTimer = null;
        }

        public Sprite GetSprite()
        {
            return stats.Icon;
        }

        public float GetCooldownPercent()
        {
            return _currentCooldownTime / stats.Cooldown;
        }


        public virtual bool CanActivate()
        {
            return _isReady;
        }

        public virtual void ForceCancelAbility()
        {
            _currentCooldownTime = stats.Cooldown;
            _isReady = true;
            StopAllCoroutines();
            EndAbility();
            _cooldownTimer = null;
        }
        
    }
}
