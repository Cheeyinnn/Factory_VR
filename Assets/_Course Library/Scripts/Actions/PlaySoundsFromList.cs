using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Play from a list of sounds using next, previous, and random
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class PlaySoundsFromList : MonoBehaviour
{
    [Tooltip("Loop the currently playing sound")]
    public bool shouldLoop = false;

    [Tooltip("The list of audio clips to play from")]
    public List<AudioClip> audioClips = new List<AudioClip>();

    private AudioSource audioSource = null;
    private int index = 0;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void NextClip()
    {
        if (audioClips == null || audioClips.Count == 0) return;
        index = (index + 1) % audioClips.Count;
        PlayClip();
    }

    public void PreviousClip()
    {
        if (audioClips == null || audioClips.Count == 0) return;
        index = (index - 1 + audioClips.Count) % audioClips.Count;
        PlayClip();
    }

    public void RandomClip()
    {
        if (audioClips == null || audioClips.Count == 0) return;
        index = Random.Range(0, audioClips.Count);
        PlayClip();
    }

    public void PlayAtIndex(int value)
    {
        if (audioClips == null || audioClips.Count == 0) return;
        // Fix: Clamp upper bound to Count - 1 so it never points past the last element
        index = Mathf.Clamp(value, 0, audioClips.Count - 1);
        PlayClip();
    }

    public void PauseClip()
    {
        if (audioSource != null)
        {
            audioSource.Pause();
        }
    }

    public void StopClip()
    {
        if (audioSource != null)
        {
            audioSource.Stop();
        }
    }

    public void PlayCurrentClip()
    {
        PlayClip();
    }

    private void PlayClip()
    {
        // Guard against empty lists or unassigned components
        if (audioClips == null || audioClips.Count == 0 || audioSource == null) return;

        // Ensure index is always valid and in bounds
        index = Mathf.Clamp(Mathf.Abs(index), 0, audioClips.Count - 1);

        if (audioClips[index] != null)
        {
            audioSource.clip = audioClips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"Audio clip at index {index} is missing or null on {gameObject.name}.", this);
        }
    }

    private void OnValidate()
    {
        AudioSource audioSource = GetComponent<AudioSource>();
        if (audioSource != null)
        {
            audioSource.loop = shouldLoop;
        }
    }
}