using UnityEngine;
using Utilities;

namespace Ability
{
    public class Shoot : Ability
    {
        [SerializeField] private Transform offset;
        [SerializeField] private Rigidbody prefab;

        protected override void ActivateAbility()
        {
            AnimationController.SetTrigger(StaticUtilities.JumpAnimID); 
        }

        protected override void ExecuteAbility()
        {
            Rigidbody spawned = Instantiate(prefab, offset.position, offset.rotation);
            spawned.AddForce(spawned.transform.forward * 10, ForceMode.Impulse);
        }

    }
}
