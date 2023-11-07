using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class O_Telephone : O_MotherInteractable
{
    [SerializeField] private AudioClip ring;
    private AudioSource _phoneAudioSource;

    private void Awake()
    {
        _phoneAudioSource = GetComponent<AudioSource>();
    }

    protected override void PopUp()
    {
        base.PopUp();
        _phoneAudioSource.clip = ring;
        _phoneAudioSource.loop = true;
        _phoneAudioSource.Play();
    }

    public override void OnPlayerInteract()
    {
        base.OnPlayerInteract();
        _phoneAudioSource.Stop();
    }
}
