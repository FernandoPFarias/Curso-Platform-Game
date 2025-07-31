using UnityEngine;

public class CampfireController : MonoBehaviour
{
    [Header("Fire Effects")]
    public GameObject flameEffect; // Efeito de chama
    public GameObject glowEffect; // Efeito de brilho (Glow)
    public GameObject sparkEffect; // Efeito de faísca (Spark)
    public ParticleSystem[] fireParticleSystems; // Array de sistemas de partículas
    
    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip fireSound; // Som da fogueira acesa
    
    [Header("State")]
    public bool isLit = false; // Se a fogueira está acesa
    
    void Start()
    {
        // Garante que a fogueira começa apagada
        DisableAllFireEffects();
    }
    
    // Ativa todos os efeitos da fogueira
    public void LightFire()
    {
        if (isLit) return;
        
        isLit = true;
        EnableAllFireEffects();
        
        // Toca som se configurado
        if (audioSource != null && fireSound != null)
        {
            audioSource.PlayOneShot(fireSound);
        }
        
        Debug.Log("CampfireController: Fogueira acesa!");
    }
    
    // Desativa todos os efeitos da fogueira
    public void ExtinguishFire()
    {
        if (!isLit) return;
        
        isLit = false;
        DisableAllFireEffects();
        
        Debug.Log("CampfireController: Fogueira apagada!");
    }
    
    // Desativa todos os efeitos da fogueira
    private void DisableAllFireEffects()
    {
        // Desativa APENAS os GameObjects configurados via Inspector
        if (flameEffect != null) flameEffect.SetActive(false);
        if (glowEffect != null) glowEffect.SetActive(false);
        if (sparkEffect != null) sparkEffect.SetActive(false);
        
        // Para APENAS os sistemas de partículas configurados via Inspector
        if (fireParticleSystems != null)
        {
            foreach (var ps in fireParticleSystems)
            {
                if (ps != null)
                {
                    ps.Stop();
                    ps.gameObject.SetActive(false);
                }
            }
        }
    }
    
    // Ativa todos os efeitos da fogueira
    private void EnableAllFireEffects()
    {
        // Ativa APENAS os GameObjects configurados via Inspector
        if (flameEffect != null) 
        {
            flameEffect.SetActive(true);
            Debug.Log("CampfireController: Flame ativado");
        }
        
        if (glowEffect != null) 
        {
            glowEffect.SetActive(true);
            Debug.Log("CampfireController: Glow ativado");
        }
        
        if (sparkEffect != null) 
        {
            sparkEffect.SetActive(true);
            Debug.Log("CampfireController: Spark ativado");
        }
        
        // Inicia APENAS os sistemas de partículas configurados via Inspector
        if (fireParticleSystems != null)
        {
            foreach (var ps in fireParticleSystems)
            {
                if (ps != null)
                {
                    ps.gameObject.SetActive(true);
                    ps.Play();
                    Debug.Log($"CampfireController: ParticleSystem {ps.name} ativado");
                }
            }
        }
    }
    
    // Método público para verificar se está acesa
    public bool IsLit()
    {
        return isLit;
    }
} 