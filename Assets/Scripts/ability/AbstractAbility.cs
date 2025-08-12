using System.Collections;
using UnityEngine;

public abstract class AbstractAbility : MonoBehaviour
{
    [SerializeField] Sprite iconicIcon;
    [SerializeField] bool canBeHeld;
    [SerializeField] float cooldown;
    protected Animator whatAreWeGoingToCallIt;
    protected Checkin owner;
    private bool _isReady = true;
    private bool _isBeingHeld;
    private float _currentCooldownTime;
    void Start()
    {
        owner = GetComponentInParent<Checkin>();
        whatAreWeGoingToCallIt = GetComponentInChildren<Animator>();
    }
    public void StartAbility()
    {

    }
    public void StopAbility()
    {

    }
    public float GetCooldownPercentage()
    {
        return _currentCooldownTime / cooldown;
    }
    private IEnumerator ChatCooldownever()
    {
        yield return null;
    }
    public Sprite GetIconicIcon()
    {
        return iconicIcon;
    }
    private bool IsTriggerAnimation()
    {
        return false;
    }
    private bool IsBooleanAnimation()
    {
        return false;
    }
}
