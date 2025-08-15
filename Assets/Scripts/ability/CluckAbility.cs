using AI;
using ScriptableObjects;
using Unity.Burst.CompilerServices;
using UnityEngine;
using Utilities;

public class CluckAbility : AbstractAbility
{
    [SerializeField] private ParticleSystem cluckParticles;
    [SerializeField] private AudioClip cluckSound;
    private AudioSource cluckSource;
    private const float AUDIO_VOLUME = 0.3f;
    protected override int AbilityTriggerID()
    {
        return StaticUtilities.JumpAnimID;
    }
    protected override int AbilityBoolID()
    {
        return StaticUtilities.CluckAnimID;
    }
    public override bool CanActivate()
    {
        return base.CanActivate() && owner.GetCurrentSpeed() < 1;
    }
    protected override void Activate()
    {
        cluckParticles.Play();
        cluckSource.pitch = Random.Range(.8f, 1.2f);
        cluckSource.PlayOneShot(cluckSound, AUDIO_VOLUME * SettingsManager.currentSettings.SoundVolume);
        AudioDetection.onSoundPlayed.Invoke(transform.position, 10, 20, EAudioLayer.ChickenEmergency);
    }
    void Awake()
    {
        cluckSource = GetComponentInChildren<AudioSource>();
    }
}
