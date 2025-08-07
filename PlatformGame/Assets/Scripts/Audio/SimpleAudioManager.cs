using UnityEngine;

// Sistema de áudio simples como alternativa ao FMOD
public class SimpleAudioManager : MonoBehaviour
{
    public static SimpleAudioManager Instance;
    
    [Header("Áudio Clips")]
    [Tooltip("Som de pulo do player")]
    public AudioClip jumpSound;
    
    [Tooltip("Som de coleta de moeda")]
    public AudioClip coinSound;
    
    [Tooltip("Música de fundo")]
    public AudioClip backgroundMusic;
    
    [Tooltip("Som de ataque")]
    public AudioClip attackSound;
    
    [Tooltip("Som de morte")]
    public AudioClip deathSound;
    
    [Header("Configurações")]
    [Range(0f, 1f)]
    public float musicVolume = 0.5f;
    
    [Range(0f, 1f)]
    public float sfxVolume = 0.8f;
    
    private AudioSource musicSource;
    private AudioSource sfxSource;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SetupAudioSources();
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void SetupAudioSources()
    {
        // Fonte para música
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource.loop = true;
        musicSource.volume = musicVolume;
        musicSource.playOnAwake = false;
        
        // Fonte para SFX
        sfxSource = gameObject.AddComponent<AudioSource>();
        sfxSource.volume = sfxVolume;
        sfxSource.playOnAwake = false;
    }
    
    // Métodos para tocar áudio
    public void PlayMusic(AudioClip musicClip)
    {
        if (musicClip != null)
        {
            musicSource.clip = musicClip;
            musicSource.Play();
            Debug.Log($"🎵 Tocando música: {musicClip.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ AudioClip de música é null");
        }
    }
    
    public void StopMusic()
    {
        musicSource.Stop();
        Debug.Log("🔇 Música parada");
    }
    
    public void PlaySFX(AudioClip sfxClip)
    {
        if (sfxClip != null)
        {
            sfxSource.PlayOneShot(sfxClip);
            Debug.Log($"🔊 Tocando SFX: {sfxClip.name}");
        }
        else
        {
            Debug.LogWarning("⚠️ AudioClip de SFX é null");
        }
    }
    
    // Métodos específicos para o jogo
    public void PlayJumpSound()
    {
        PlaySFX(jumpSound);
    }
    
    public void PlayCoinSound()
    {
        PlaySFX(coinSound);
    }
    
    public void PlayAttackSound()
    {
        PlaySFX(attackSound);
    }
    
    public void PlayDeathSound()
    {
        PlaySFX(deathSound);
    }
    
    public void PlayBackgroundMusic()
    {
        PlayMusic(backgroundMusic);
    }
    
    // Controles de volume
    public void SetMusicVolume(float volume)
    {
        musicVolume = Mathf.Clamp01(volume);
        musicSource.volume = musicVolume;
    }
    
    public void SetSFXVolume(float volume)
    {
        sfxVolume = Mathf.Clamp01(volume);
        sfxSource.volume = sfxVolume;
    }
    
    void Update()
    {
        // Testes com teclas
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            PlayJumpSound();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            PlayCoinSound();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            PlayAttackSound();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            PlayDeathSound();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            PlayBackgroundMusic();
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            StopMusic();
        }
    }
} 