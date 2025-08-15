using UnityEngine;
[DefaultExecutionOrder(-1000)]
public class HUD : MonoBehaviour
{
    [Header("earactables")]
    [SerializeField] private AbilityUIBind jump;
    [SerializeField] private AbilityUIBind dash;
    [SerializeField] private AbilityUIBind cluck;
    [SerializeField] private AbilityUIBind glih;
    public static HUD Instance {
        get;private set;
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
}
