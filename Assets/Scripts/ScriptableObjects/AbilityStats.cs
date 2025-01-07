using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "AbilityStats", menuName = "ChickenChaser/AbilityStats")]
    public class AbilityStats : ScriptableObject
    {
        [SerializeField] private Sprite icon;
        [SerializeField] private float cooldown;
        [SerializeField] private bool canBeHeld;
        
        public Sprite Icon => icon;
        public float Cooldown => cooldown;
        public bool CanBeHeld => canBeHeld;
    }
}
