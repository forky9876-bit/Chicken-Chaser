using UI.InGame;
using UnityEngine;

namespace Managers
{
    public class HUDManager : MonoBehaviour
    {
        [Header("Interactables")]
        [SerializeField] private AbilityUIBind abilityPrefab;
        [SerializeField] private Transform abilityContainer;
        public static HUDManager Instance { get; private set; }
        
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        

        public void BindAbilities(Ability.Ability[] abilities)
        {
            for (int i = abilityContainer.childCount - 1; i >= 0; i--)
            {
                Destroy(abilityContainer.GetChild(i).gameObject);
            }

            foreach (Ability.Ability ability in abilities)
            {
                AbilityUIBind abilityUI = Instantiate(abilityPrefab, abilityContainer);
                abilityUI.SetTargetAbility(ability);
            }
            
        }

    }
}
