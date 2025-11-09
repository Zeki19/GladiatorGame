using System;
using UnityEngine;

[Serializable]
public class Sound
{
    [SerializeField] public string name;
    
    [Header("Single Clip Mode")]
    [Tooltip("Use this for sounds that always play the same clip")]
    [SerializeField] public AudioClip clip;
    
    [Header("Multiple Clips Mode (Random Variation)")]
    [Tooltip("Use this for sounds with variations (like Hit sounds). Leave empty to use single clip.")]
    [SerializeField] public AudioClip[] clipVariations;
    
    [SerializeField] public bool loop;
    [Range(0f, 1f)] public float volume = 1f;

    public AudioClip GetClip()
    {
        if (clipVariations != null && clipVariations.Length > 0)
        {
            AudioClip[] validClips = System.Array.FindAll(clipVariations, c => c != null);
            
            if (validClips.Length > 0)
            {
                return validClips[UnityEngine.Random.Range(0, validClips.Length)];
            }
        }

        return clip;
    }

    public bool HasValidClip()
    {
        if (clip != null)
            return true;
            
        if (clipVariations != null && clipVariations.Length > 0)
        {
            foreach (var c in clipVariations)
            {
                if (c != null)
                    return true;
            }
        }
        
        return false;
    }
}
