using UnityEngine;
using System.Collections;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Audio Sources")]
    public AudioSource musicSource1;
    public AudioSource musicSource2;

    [Header("Settings")]
    public float crossfadeTime = 2f;
    public float fadeTime = 1f;

    private AudioSource currentSource;
    private AudioSource nextSource;
    private AudioClip backgroundMusic;
    private bool isCrossfading = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioSources()
    {
        if (musicSource1 == null)
        {
            musicSource1 = gameObject.AddComponent<AudioSource>();
            musicSource1.loop = true;
            musicSource1.playOnAwake = false;
        }
        if (musicSource2 == null)
        {
            musicSource2 = gameObject.AddComponent<AudioSource>();
            musicSource2.loop = true;
            musicSource2.playOnAwake = false;
        }

        currentSource = musicSource1;
        nextSource = musicSource2;
    }

    public void PlayBackgroundMusic(AudioClip music)
    {
        backgroundMusic = music;
        if (!currentSource.isPlaying)
        {
            currentSource.clip = music;
            currentSource.Play();
        }
    }

    public void CrossfadeToMusic(AudioClip newMusic, float fadeTime = -1f)
    {
        if (fadeTime < 0) fadeTime = this.crossfadeTime;
        if (isCrossfading) return;

        StartCoroutine(CrossfadeCoroutine(newMusic, fadeTime));
    }

    public void FadeInMusic(AudioClip music, float fadeTime = -1f)
    {
        if (fadeTime < 0) fadeTime = this.fadeTime;
        StartCoroutine(FadeInCoroutine(music, fadeTime));
    }

    public void FadeOutCurrentMusic(float fadeTime = -1f)
    {
        if (fadeTime < 0) fadeTime = this.fadeTime;
        StartCoroutine(FadeOutCoroutine(fadeTime));
    }

    public void ReturnToBackgroundMusic(float fadeTime = -1f)
    {
        if (fadeTime < 0) fadeTime = this.crossfadeTime;
        if (backgroundMusic != null)
        {
            CrossfadeToMusic(backgroundMusic, fadeTime);
        }
    }

    private IEnumerator CrossfadeCoroutine(AudioClip newMusic, float fadeTime)
    {
        isCrossfading = true;

        // Configura o próximo source
        nextSource.clip = newMusic;
        nextSource.volume = 0f;
        nextSource.Play();

        float startTime = Time.time;
        float startVolume = currentSource.volume;

        while (Time.time - startTime < fadeTime)
        {
            float progress = (Time.time - startTime) / fadeTime;
            
            // Diminui o volume do source atual
            currentSource.volume = Mathf.Lerp(startVolume, 0f, progress);
            
            // Aumenta o volume do próximo source
            nextSource.volume = Mathf.Lerp(0f, 1f, progress);

            yield return null;
        }

        // Finaliza a transição
        currentSource.Stop();
        currentSource.volume = 1f;
        nextSource.volume = 1f;

        // Troca as referências
        AudioSource temp = currentSource;
        currentSource = nextSource;
        nextSource = temp;

        isCrossfading = false;
    }

    private IEnumerator FadeInCoroutine(AudioClip music, float fadeTime)
    {
        currentSource.clip = music;
        currentSource.volume = 0f;
        currentSource.Play();

        float startTime = Time.time;

        while (Time.time - startTime < fadeTime)
        {
            float progress = (Time.time - startTime) / fadeTime;
            currentSource.volume = Mathf.Lerp(0f, 1f, progress);
            yield return null;
        }

        currentSource.volume = 1f;
    }

    private IEnumerator FadeOutCoroutine(float fadeTime)
    {
        float startTime = Time.time;
        float startVolume = currentSource.volume;

        while (Time.time - startTime < fadeTime)
        {
            float progress = (Time.time - startTime) / fadeTime;
            currentSource.volume = Mathf.Lerp(startVolume, 0f, progress);
            yield return null;
        }

        currentSource.Stop();
        currentSource.volume = 1f;
    }

    public void StopAllMusic()
    {
        currentSource.Stop();
        nextSource.Stop();
        isCrossfading = false;
    }

    public bool IsPlaying()
    {
        return currentSource.isPlaying || nextSource.isPlaying;
    }
} 