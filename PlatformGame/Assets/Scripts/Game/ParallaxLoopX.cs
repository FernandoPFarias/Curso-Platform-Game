using UnityEngine;

public class ParallaxLoopX : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxMultiplier = 0.5f;

    private float spriteWidth;
    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;

        // Calcula a largura do sprite
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr != null)
            spriteWidth = sr.bounds.size.x;
        else
            spriteWidth = 10f; // valor padrão se não achar o SpriteRenderer
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier, 0, 0);
        lastCameraPosition = cameraTransform.position;

        // Looping: se a câmera andar além do sprite, reposiciona para o outro lado
        float camRelativeX = cameraTransform.position.x - transform.position.x;
        if (Mathf.Abs(camRelativeX) >= spriteWidth)
        {
            float offset = (camRelativeX > 0) ? spriteWidth : -spriteWidth;
            transform.position += new Vector3(offset, 0, 0);
        }
    }
} 