using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

public class CameraEffects : MonoBehaviour
{
    [Header("Shake Settings")]
    public float defaultShakeIntensity = 1f;
    public float defaultShakeDuration = 0.3f;
    
    [Header("Zoom Settings")]
    public float defaultZoomInSize = 3f;
    public float defaultZoomOutSize = 7f;
    public float zoomSpeed = 2f;
    
    private CinemachineVirtualCamera virtualCamera;
    private CinemachineImpulseSource impulseSource;
    private float originalOrthographicSize;
    private Coroutine zoomCoroutine;
    
    private void Start()
    {
        // Encontrar Virtual Camera
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>();
        if (virtualCamera != null)
        {
            originalOrthographicSize = virtualCamera.m_Lens.OrthographicSize;
            
            // Adicionar ImpulseSource se não existir
            impulseSource = virtualCamera.GetComponent<CinemachineImpulseSource>();
            if (impulseSource == null)
            {
                impulseSource = virtualCamera.gameObject.AddComponent<CinemachineImpulseSource>();
            }
        }
    }
    
    #region Shake Effects
    
    public void ShakeCamera()
    {
        ShakeCamera(defaultShakeIntensity, defaultShakeDuration);
    }
    
    public void ShakeCamera(float intensity, float duration)
    {
        if (impulseSource != null)
        {
            StartCoroutine(ShakeCoroutine(intensity, duration));
        }
    }
    
    private IEnumerator ShakeCoroutine(float intensity, float duration)
    {
        impulseSource.GenerateImpulse(intensity);
        yield return new WaitForSeconds(duration);
    }
    
    // Shake para diferentes tipos de eventos
    public void ShakeForDamage()
    {
        ShakeCamera(0.5f, 0.2f);
    }
    
    public void ShakeForExplosion()
    {
        ShakeCamera(1.5f, 0.5f);
    }
    
    public void ShakeForLanding()
    {
        ShakeCamera(0.3f, 0.1f);
    }
    
    #endregion
    
    #region Zoom Effects
    
    public void ZoomIn()
    {
        ZoomTo(defaultZoomInSize);
    }
    
    public void ZoomOut()
    {
        ZoomTo(defaultZoomOutSize);
    }
    
    public void ZoomTo(float targetSize)
    {
        if (virtualCamera != null)
        {
            if (zoomCoroutine != null)
            {
                StopCoroutine(zoomCoroutine);
            }
            zoomCoroutine = StartCoroutine(ZoomCoroutine(targetSize));
        }
    }
    
    public void ResetZoom()
    {
        ZoomTo(originalOrthographicSize);
    }
    
    private IEnumerator ZoomCoroutine(float targetSize)
    {
        float startSize = virtualCamera.m_Lens.OrthographicSize;
        float elapsedTime = 0f;
        
        while (elapsedTime < 1f)
        {
            elapsedTime += Time.deltaTime * zoomSpeed;
            float currentSize = Mathf.Lerp(startSize, targetSize, elapsedTime);
            virtualCamera.m_Lens.OrthographicSize = currentSize;
            yield return null;
        }
        
        virtualCamera.m_Lens.OrthographicSize = targetSize;
        zoomCoroutine = null;
    }
    
    // Zoom para diferentes situações
    public void ZoomForCombat()
    {
        ZoomTo(originalOrthographicSize * 0.8f);
    }
    
    public void ZoomForExploration()
    {
        ZoomTo(originalOrthographicSize * 1.2f);
    }
    
    #endregion
    
    #region Screen Effects
    
    public void FlashScreen(Color flashColor, float duration)
    {
        StartCoroutine(FlashCoroutine(flashColor, duration));
    }
    
    private IEnumerator FlashCoroutine(Color flashColor, float duration)
    {
        // Criar overlay de flash
        GameObject flashObj = new GameObject("Screen Flash");
        Canvas canvas = FindObjectOfType<Canvas>();
        
        if (canvas == null)
        {
            // Criar canvas se não existir
            GameObject canvasObj = new GameObject("Flash Canvas");
            canvas = canvasObj.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvas.sortingOrder = 1000;
        }
        
        flashObj.transform.SetParent(canvas.transform);
        
        UnityEngine.UI.Image flashImage = flashObj.AddComponent<UnityEngine.UI.Image>();
        flashImage.color = flashColor;
        flashImage.rectTransform.anchorMin = Vector2.zero;
        flashImage.rectTransform.anchorMax = Vector2.one;
        flashImage.rectTransform.offsetMin = Vector2.zero;
        flashImage.rectTransform.offsetMax = Vector2.zero;
        
        // Animar flash
        float elapsedTime = 0f;
        Color startColor = flashColor;
        startColor.a = 0f;
        
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Sin((elapsedTime / duration) * Mathf.PI);
            flashImage.color = new Color(flashColor.r, flashColor.g, flashColor.b, alpha);
            yield return null;
        }
        
        Destroy(flashObj);
    }
    
    #endregion
    
    #region Utility Methods
    
    public void SetCameraTarget(Transform target)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Follow = target;
        }
    }
    
    public void SetCameraPriority(int priority)
    {
        if (virtualCamera != null)
        {
            virtualCamera.Priority = priority;
        }
    }
    
    public void EnableCamera(bool enable)
    {
        if (virtualCamera != null)
        {
            virtualCamera.enabled = enable;
        }
    }
    
    #endregion
} 