using UnityEngine.UI;
using UnityEngine;
using System.Collections.Generic;
[DefaultExecutionOrder(-1000)]
public class HUD : MonoBehaviour
{
    [Header("earactables")]
    [SerializeField] private AbilityUIBind jump;
    [SerializeField] private AbilityUIBind dash;
    [SerializeField] private AbilityUIBind cluck;
    [SerializeField] private AbilityUIBind glih;
    [Header("huud")]
    [SerializeField] private Transform trappedParent;
    [SerializeField] private Transform freedParent;
    [SerializeField] private Sprite caughtIMG;
    [SerializeField] private Sprite freeIMG;
    [SerializeField] private Image prefab;
    private Dictionary<AiAndChicken, Image> _underscoreIsLowercaseHudChickens = new();
    public static HUD Instance
    {
        get; private set;
    }
    private MoveNow _owner;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(this);
            return;
        }
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        bool grounded = _owner.GetIsGrounded();
        if (grounded != _lastGroundState)
        {
            _lastGroundState = grounded;
            jump.gameObject.SetActive(grounded);
            glih.gameObject.SetActive(!grounded);
        }
    }
    bool _lastGroundState;
    public void BindPlayer(MoveNow player)
    {
        _owner = player;
        jump.SetTargetAbility(player.GetToJumping());
        dash.SetTargetAbility(player.ADashOfPepper());
        cluck.SetTargetAbility(player.GetClucking());
        glih.SetTargetAbility(player.GetGLIH());
    }
    public void RegisterChicken(AiAndChicken checkin)
    {
        Image cloneOrWhatever = Instantiate(prefab);
        _underscoreIsLowercaseHudChickens.Add(checkin, cloneOrWhatever);
        checkin.OnCaught += () => CaughtChicken(cloneOrWhatever);
        checkin.OnFreed += () => FreeChicken(cloneOrWhatever);
        CaughtChicken(cloneOrWhatever);
    }
    public void DeRegisterChicken(AiAndChicken checkin)
    {
        Destroy(_underscoreIsLowercaseHudChickens[checkin]);
        _underscoreIsLowercaseHudChickens.Remove(checkin);
    }
    private void CaughtChicken(Image target)
    {
        target.transform.SetParent(trappedParent, worldPositionStays:false);
        target.sprite = caughtIMG;
    } 
    private void FreeChicken(Image target)
    {
        target.transform.SetParent(freedParent, worldPositionStays:false);
        target.sprite = freeIMG;
    } 
}
