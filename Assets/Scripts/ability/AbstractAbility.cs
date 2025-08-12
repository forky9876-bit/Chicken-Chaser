using System.Collections;
using System.Threading;
using UnityEngine;

public abstract class AbstractAbility : MonoBehaviour
{
    #region words
    [SerializeField] Sprite iconicIcon;
    [SerializeField] bool canBeHeld;
    [SerializeField] float cooldown;
    protected Animator whatAreWeGoingToCallIt;
    protected Checkin owner;
    private bool _isReady = true;
    private bool _isBeingHeld;
    private float _currentCooldownTime;
    #endregion
    #region this is the code
    void Start()
    {
        owner = GetComponentInParent<Checkin>();
        whatAreWeGoingToCallIt = GetComponentInChildren<Animator>();
    }
    public void StartAbility()
    {
        _isBeingHeld = true;
        if (_isReady)
        {
            StartCoroutine(ChatCooldownever());
        }
        if (IsBoolAnimation())
        {
            whatAreWeGoingToCallIt.SetBool(AbilityBoolID(), true);
        }
    }
    public void StopAbility()
    {
        _isBeingHeld = false;
        if (IsBoolAnimation())
        {
            whatAreWeGoingToCallIt.SetBool(AbilityBoolID(), false);
        }
    }
    public float GetCooldownPercentage()
    {
        return _currentCooldownTime / cooldown;
    }
    private IEnumerator ChatCooldownever()
    {
        do
        {
            yield return new WaitUntil(CanActivate);
            if (!_isBeingHeld)
            {
                yield break;
            }
            Activate();
            if (IsTriggerAnimation())
            {
                whatAreWeGoingToCallIt.SetTrigger(AbilityTriggerID());
            }
            _currentCooldownTime = 0;
            _isReady = false;
            while (_currentCooldownTime < cooldown)
            {
                _currentCooldownTime += Time.deltaTime;
                yield return null;
            }
            _currentCooldownTime = cooldown;
            _isReady = true;
        } while (_isBeingHeld && canBeHeld);
        StopAbility();
    }
    public Sprite GetIconicIcon()
    {
        return iconicIcon;
    }
    private bool IsTriggerAnimation()
    {

        return AbilityTriggerID() != 0;
    }
    private bool IsBoolAnimation()
    {
        return AbilityBoolID() != 0;
    }
    #endregion
    #region inheritance
    //a few more methods
    public virtual bool CanActivate()
    {
        return _isReady;
    }
    protected abstract void Activate();
    public virtual void ForceCancelAbility()
    {
        _currentCooldownTime = cooldown;
        _isReady = true;
        StopAllCoroutines();
        StopAbility();
    }
    protected virtual int AbilityBoolID()
    {
        return 0;
    }
    protected virtual int AbilityTriggerID()
    {
        return 0;
    }
    #endregion
}
