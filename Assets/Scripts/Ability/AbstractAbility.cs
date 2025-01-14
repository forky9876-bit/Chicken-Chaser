using System.Collections;
using UnityEngine;

public abstract class AbstractAbility : MonoBehaviour
{
    [SerializeField] private AbilityStats stats;
    
    //We need to who we belong to
    protected Chicken Owner;
    
    //We need to know what animations to control
    protected Animator AnimatorController;
    
    //We need to track our state
    private bool _isReady = true; // Default to true because we should start as ready.
    private bool _isBeingHeld;
    private float _currentCooldownTime;
    
    //Getter Functions so we can access data in other files
    public Sprite GetIcon()
    {
        return stats.Icon;
    }

    public float GetCooldownPercent()
    {
        return _currentCooldownTime / stats.Cooldown;
    }

    private bool IsTriggerAnimation()
    {
        return AbilityTriggerID() != 0;
    }

    private bool IsBooleanAnimation()
    {
        return AbilityBoolID() != 0;
    }
    
    // Base Functionality
    private void Start()
    {
        //Use start because we don't own these components
        Owner = GetComponentInParent<Chicken>();
        AnimatorController = GetComponentInChildren<Animator>();
    }

    //IEnumerators are timer functions
    private IEnumerator BeginCooldown()
    {
        do
        {
            //Wait until we can activate
            yield return new WaitUntil(CanActivate);
            
            //If we've let go, then we should leave the loop
            if(!_isBeingHeld) yield break; 
           
            //Activate and Animate
            Activate();
            if(IsTriggerAnimation()) AnimatorController.SetTrigger(AbilityTriggerID());
            
            //Refresh the cooldown
            _currentCooldownTime = 0;
            _isReady = false;
            
            while (_currentCooldownTime < stats.Cooldown)
            {
                _currentCooldownTime += Time.deltaTime;
                //Wait until the next frame
                yield return null;
            }

            //Mark as ready, and set the currentTime to cooldownTime so that it's 100%
            _currentCooldownTime = stats.Cooldown;
            _isReady = true;
            
            //If we're still holding and we can be held, then repeat the loop
        } while (_isBeingHeld && stats.CanBeHeld);

        StopUsingAbility();
    }

    //Accessibility functions
    public void StartUsingAbility()
    {
        _isBeingHeld = true;
        //Only start if we're ready... If we're not ready, then a loop must be running still
        if(_isReady) StartCoroutine(BeginCooldown());
        if(IsBooleanAnimation()) AnimatorController.SetBool(AbilityBoolID(), true);
    }

    public void StopUsingAbility()
    {
        _isBeingHeld = false;
        if(IsBooleanAnimation()) AnimatorController.SetBool(AbilityBoolID(), false);
    }
    
    //Modified in Child Classes
    public virtual bool CanActivate()
    {
        //NOTE: we don't need to return the time, because they're updated together
        return _isReady;
    }

    public virtual void ForceCancelAbility()
    {
        _currentCooldownTime = stats.Cooldown;
        _isReady = true;
        StopAllCoroutines();
        StopUsingAbility();
    }
    
    protected virtual int AbilityBoolID()
    {
        return 0;
    }

    protected virtual int AbilityTriggerID()
    {
        return 0;
    }

    protected abstract void Activate();

}
