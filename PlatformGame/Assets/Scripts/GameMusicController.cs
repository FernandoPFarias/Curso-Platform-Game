using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMusicController : MonoBehaviour
{
    public static GameMusicController Instance { get; private set; }

    [Header("Background Music")]
    public AudioClip defaultBackgroundMusic;
    public float fadeInTime = 2f;

    private bool musicStarted = false;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeMusic();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Garante que a música de fundo comece se ainda não começou
        if (!musicStarted && defaultBackgroundMusic != null)
        {
            StartBackgroundMusic();
        }
    }

    private void InitializeMusic()
    {
        // Se o AudioManager não existe, cria um
        if (AudioManager.Instance == null)
        {
            GameObject audioManagerObj = new GameObject("AudioManager");
            audioManagerObj.AddComponent<AudioManager>();
            DontDestroyOnLoad(audioManagerObj);
        }
    }

    public void StartBackgroundMusic()
    {
        if (AudioManager.Instance != null && defaultBackgroundMusic != null)
        {
            AudioManager.Instance.PlayBackgroundMusic(defaultBackgroundMusic);
            musicStarted = true;
            Debug.Log("[GameMusicController] Música de fundo iniciada");
        }
    }

    public void ChangeBackgroundMusic(AudioClip newMusic, float fadeTime = -1f)
    {
        if (AudioManager.Instance != null && newMusic != null)
        {
            AudioManager.Instance.CrossfadeToMusic(newMusic, fadeTime);
            defaultBackgroundMusic = newMusic;
            Debug.Log($"[GameMusicController] Música de fundo alterada para: {newMusic.name}");
        }
    }

    public void PauseMusic()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.StopAllMusic();
        }
    }

    public void ResumeMusic()
    {
        if (AudioManager.Instance != null && defaultBackgroundMusic != null)
        {
            AudioManager.Instance.PlayBackgroundMusic(defaultBackgroundMusic);
        }
    }

    // Método para ser chamado quando o jogo pausa
    public void OnGamePaused()
    {
        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.FadeOutCurrentMusic(0.5f);
        }
    }

    // Método para ser chamado quando o jogo despausa
    public void OnGameResumed()
    {
        if (AudioManager.Instance != null && defaultBackgroundMusic != null)
        {
            AudioManager.Instance.FadeInMusic(defaultBackgroundMusic, 0.5f);
        }
    }

    // Método para ser chamado ao trocar de cena (opcional)
    public void OnSceneChanged()
    {
        // A música continua automaticamente graças ao DontDestroyOnLoad
        Debug.Log("[GameMusicController] Cena trocada, música continua");
    }
} 