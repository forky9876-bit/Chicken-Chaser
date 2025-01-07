using UnityEngine;

namespace ScriptableObjects
{
    [CreateAssetMenu(fileName = "ChickenStats", menuName = "ChickenChaser/ChickenStats")]
    public class ChickenStats : ScriptableObject
    {
        [SerializeField] private float moveSpeed = 20f;
        [SerializeField] private float maxSpeed = 20f;
        [SerializeField] private float footDistance;
        [SerializeField] private float footRadius;

        //Sometimes called Properties Getters (Readonly variables)
        public float MoveSpeed => moveSpeed;
        public float FootDistance => footDistance;
        public float FootRadius => footRadius;
        public float MaxSpeed => maxSpeed;
    }
}
