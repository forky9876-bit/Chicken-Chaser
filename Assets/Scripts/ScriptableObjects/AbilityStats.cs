using UnityEngine;

[CreateAssetMenu(fileName = "AbilityStats", menuName = "ChickenChaser/AbilityStats", order = 10)]
public class AbilityStats : ScriptableObject
{
    //We need to know how we appear visually
    [SerializeField] private Sprite icon;
    
    //We need to know how we function
    [SerializeField] private float cooldown;
    [SerializeField] private bool canBeHeld;

    public Sprite Icon => icon;
    public float Cooldown => cooldown;
    public bool CanBeHeld => canBeHeld;
}
