using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private SFXEntry[] sfxClips;

    private Dictionary<string, AudioClip> sfxTable;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else { Destroy(gameObject); return; }

        sfxTable = new Dictionary<string, AudioClip>();
        if (sfxClips != null)
        {
            foreach (var e in sfxClips)
            {
                if (e.clip != null && !sfxTable.ContainsKey(e.key))
                    sfxTable[e.key] = e.clip;
            }
        }
    }

    public void PlaySFX(string key)
    {
        if (sfxSource == null) return;
        if (sfxTable.TryGetValue(key, out var clip))
            sfxSource.PlayOneShot(clip, Random.Range(0.85f, 1f));
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource == null || clip == null) return;
        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }
}

[System.Serializable]
public class SFXEntry
{
    public string key;
    public AudioClip clip;
}
