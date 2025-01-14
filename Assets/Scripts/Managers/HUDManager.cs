using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;

[DefaultExecutionOrder(-1000)]
public class HudManager : MonoBehaviour
{
    [Header("Interactables")]
    [SerializeField] private AbilityUIBind abilityA;
    [SerializeField] private AbilityUIBind abilityB;
    [SerializeField] private AbilityUIBind abilityC;
    
    private PlayerChicken _owner;

    [Header("HUD")] 
    [SerializeField] private Transform trappedParent;
    [SerializeField] private Transform freedParent;

    //Make sure to Import  using UnityEngine.UI; for images
    [SerializeField] private Sprite caughtImg;
    [SerializeField] private Sprite freedImg;
    
    [SerializeField] private Image chickenImgPrefab;

    private Dictionary<AiChicken, Image> _hudChickens = new();
    
    public static HudManager Instance { get; private set; } //static means there is only 1 (And it always exists without being instansiated)
    
#if (!UNITY_STANDALONE && !UNITY_WEBGL) || UNITY_EDITOR
    [SerializeField] private Joystick stick;
    [SerializeField] private Button settingsButton;
    [SerializeField] private float lookSpeed = 20; // Just to make looking around a bit easier
    [SerializeField, Range(0,1)] private float stickDeadZoneY = 0.02f; // Just to make looking around a bit easier
    [SerializeField] private bool editorPhoneMode = true; //Makes the UI visible in editor
    private float _lookMultiplier;
#endif


    void Awake()
    {
        //If there are two HUDs
        if (Instance && Instance != this)
        {
            //Remove one
            Destroy(Instance);
            return;
        }
        Instance = this;
        
#if !UNITY_STANDALONE && !UNITY_WEBGL
        //Enable the joystick and settings button, and make the settings button functional
        stick.gameObject.SetActive(true);
        settingsButton.gameObject.SetActive(true);
        settingsButton.onClick.AddListener(EnterSettings);
#endif
    }

    #region Registering Chickens

    public void BindPlayer(PlayerChicken player)
    {
        _owner = player;
            
        //Bind abilities
        abilityA.SetTargetAbility(player.GetCluckAbility());
        abilityB.SetTargetAbility(player.GetJumpAbility());
        abilityC.SetTargetAbility(player.GetDashAbility());
    }

    public void RegisterChicken(AiChicken chicken)
    {
        //Create a prefab and add it to the dictionary.
        Image clone = Instantiate(chickenImgPrefab);
        _hudChickens.Add(chicken, clone);

        //Bind our events to automate chicken captures and released
        chicken.OnCaught += () => CaughtChicken(clone);
        chicken.OnFreed += () => FreeChicken(clone);
        
        //Assume the chicken is caught (You do this in AiChicken if you prefer, and it may make more sense)
        CaughtChicken(clone);
    }
    
    public void DeRegisterChicken(AiChicken chicken)
    {
        //Destroy and remove the chicken
        Destroy(_hudChickens[chicken]);
        _hudChickens.Remove(chicken);
    }

    private void CaughtChicken(Image target)
    {
        target.transform.SetParent(trappedParent,false);
        target.sprite = caughtImg;
    }

    private void FreeChicken(Image target)
    {
        target.transform.SetParent(freedParent,false);
        target.sprite = freedImg;
    }

    #endregion
    
    
    #region Mobile
        

    //NOTE: Add the preprocessors at the end as they make development much harder.
#if !UNITY_STANDALONE && !UNITY_WEBGL

        private static Vector2 _displayScale;
        
        private void OnEnable()
        {
            //Bind the sensitivity settings
            SettingsManager.SaveFile.onLookSenseChanged += OnLookSensChanged;
            //Set current state
            OnLookSensChanged(SettingsManager.currentSettings.LookSensitivity);
        }

        private void OnDisable()
        {
            //Unbind the settings
            SettingsManager.SaveFile.onLookSenseChanged -= OnLookSensChanged;
        }

        #if UNITY_EDITOR
        
        #endif
        
        private void Update()
        {
            #if UNITY_EDITOR
            if (!editorPhoneMode) return;
            #endif
            _owner.SetMoveDirection(stick.Direction != Vector2.zero ? Vector2.up : Vector2.zero);
            //For looking, we should check to see if the magnitude is some really small number, if it is, we should actually just ignore it.
            Vector2 value = new Vector2(stick.Direction.x, Mathf.Abs(stick.Direction.y) > stickDeadZoneY?stick.Direction.y/((float)Screen.currentResolution.width/Screen.currentResolution.height): 0);
            //Force the owner to look as if its recieving mouse input.
            _owner.SetLookDirection(Vector2.Scale(value ,_displayScale));
        }
        
        private void OnLookSensChanged(float obj)
        {
            //When the sense is changed, double check the screen scale and recompute values (In case screen was rotated, there are other ways to check this though)
            _lookMultiplier = obj;
            _displayScale = new Vector2(1, (float)Screen.width / Screen.height) *(lookSpeed * _lookMultiplier);
        }
        
        private void EnterSettings()
        {
            //Open the settings menu
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(ExitSettings);
            Settings.OpenSettings(false);
        }

        private void ExitSettings()
        {
            //Close the settings menu
            settingsButton.onClick.RemoveAllListeners();
            settingsButton.onClick.AddListener(EnterSettings);
            Settings.CloseSettings();
        }
#endif
    
    #endregion
    
}
