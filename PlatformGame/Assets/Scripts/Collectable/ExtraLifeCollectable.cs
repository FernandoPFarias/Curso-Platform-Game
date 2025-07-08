using UnityEngine;

public class ExtraLifeCollectable : Collectable
{
    [Header("Extra Life Settings")]
    public int extraLivesToAdd = 1;
    public bool playSound = true;
    public bool showParticleEffect = true;
    
    [Header("Optional Effects")]
    public GameObject collectEffect; // Prefab de efeito visual
    public AudioClip collectSound; // Som de coleta
    
    public override void Collect(GameObject collector)
    {
        if (GameManager.Instance != null)
        {
            // Adiciona vidas extras
            GameManager.Instance.AddExtraLife(extraLivesToAdd);
            
            // Efeitos visuais
            if (showParticleEffect && collectEffect != null)
            {
                Instantiate(collectEffect, transform.position, Quaternion.identity);
            }
            
            // Efeito sonoro
            if (playSound && collectSound != null)
            {
                AudioSource.PlayClipAtPoint(collectSound, transform.position);
            }
            
            Debug.Log($"Vida extra coletada! +{extraLivesToAdd} vida(s)");
        }
        
        // Destrói o item
        Destroy(gameObject);
    }
    
    // Método para mostrar informações no Inspector
    void OnValidate()
    {
        if (extraLivesToAdd < 1)
            extraLivesToAdd = 1;
    }
} 