using System;
using AI;
using ScriptableObjects;
using UnityEngine;
using Utilities;
using Random = UnityEngine.Random;

namespace Ability
{
    public class Cluck : Ability
    {
        [SerializeField] private ParticleSystem cluckParticle;
        [SerializeField] private AudioClip cluckSound;
        
        private const float AudioVolume = 0.3f;
        private AudioSource _source;

        private void Awake()
        {
            _source = GetComponent<AudioSource>();
        }

        protected override void ActivateAbility()
        {
            AnimationController.SetBool(StaticUtilities.CluckAnimID, true);
            AnimationController.SetTrigger(StaticUtilities.JumpAnimID);
        }

        protected override void ExecuteAbility()
        {
            cluckParticle.Play();
            _source.pitch = Random.Range(0.8f, 1.2f);
            _source.PlayOneShot(cluckSound, AudioVolume * SettingsManager.currentSettings.SoundVolume);
            AudioDetection.onSoundPlayed.Invoke(transform.position, 10, 20, EAudioLayer.ChickenEmergency);
        }

        public override bool CanActivate()
        {
            return base.CanActivate() && Owner.GetCurrentSpeed() < 1;
        }

        protected override void EndAbility()
        {
            AnimationController.SetBool(StaticUtilities.CluckAnimID, false);
        }
    }
}
