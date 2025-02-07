using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages background music (BGM) and sound effects (SFX) in the game.
/// </summary>
public class AudioManager : Singleton<AudioManager>
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSourcePrefab; 
    [SerializeField] private AudioSource sfxSourcePrefab;

    private Coroutine bgmCoroutine;
    private Queue<AudioSource> sfxPool;

    [Header("Audio Settings")]
    [SerializeField] private int sfxPoolSize = 10; 

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        InitializeSFXPool();
    }

    private void InitializeSFXPool()
    {
        sfxPool = new Queue<AudioSource>();

        for (int i = 0; i < sfxPoolSize; i++)
        {
            AudioSource sfxSource = Instantiate(sfxSourcePrefab, transform);
            sfxSource.gameObject.SetActive(false);
            sfxPool.Enqueue(sfxSource);
        }
    }

    public void PlayBGM(AudioClip clip, bool loop = true)
    {
        if (bgmSourcePrefab.isPlaying && bgmSourcePrefab.clip == clip) return;

        if (bgmCoroutine != null)
        {
            StopCoroutine(bgmCoroutine);
        }

        bgmCoroutine = StartCoroutine(PlayBGMCoroutine(clip, loop));
    }

    private IEnumerator PlayBGMCoroutine(AudioClip clip, bool loop)
    {
        if (bgmSourcePrefab.isPlaying)
        {
            bgmSourcePrefab.Stop();
        }

        bgmSourcePrefab.clip = clip;
        bgmSourcePrefab.loop = loop;
        bgmSourcePrefab.Play();

        yield return null;
    }

    public void StopBGM()
    {
        if (bgmCoroutine != null)
        {
            StopCoroutine(bgmCoroutine);
            bgmCoroutine = null;
        }
        bgmSourcePrefab.Stop();
    }

    public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f)
    {
        if (clip == null) return;

        StartCoroutine(PlaySFXCoroutine(clip, volume, pitch));
    }

    private IEnumerator PlaySFXCoroutine(AudioClip clip, float volume, float pitch)
    {
        AudioSource sfxSource = GetSFXSource();
        sfxSource.clip = clip;
        sfxSource.volume = volume;
        sfxSource.pitch = pitch;
        sfxSource.Play();

        yield return new WaitForSeconds(clip.length);

        ReleaseSFXSource(sfxSource);
    }

    private AudioSource GetSFXSource()
    {
        if (sfxPool.Count > 0)
        {
            AudioSource source = sfxPool.Dequeue();
            source.gameObject.SetActive(true);
            return source;
        }

        return Instantiate(sfxSourcePrefab, transform);
    }

    private void ReleaseSFXSource(AudioSource source)
    {
        source.Stop();
        source.clip = null;
        source.gameObject.SetActive(false);
        sfxPool.Enqueue(source);
    }
}