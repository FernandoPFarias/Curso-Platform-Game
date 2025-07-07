using UnityEngine;

public class ParallaxLoopXMulti : MonoBehaviour
{
    public Transform cameraTransform;
    public float parallaxMultiplier = 0.5f;
    public Transform[] backgrounds; // Arraste aqui os 3+ backgrounds, em ordem no eixo X

    private float spriteWidth;
    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;

        // Assume que todos os backgrounds têm o mesmo tamanho
        SpriteRenderer sr = backgrounds[0].GetComponent<SpriteRenderer>();
        spriteWidth = sr.bounds.size.x;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        foreach (var bg in backgrounds)
        {
            bg.position += new Vector3(deltaMovement.x * parallaxMultiplier, 0, 0);
        }
        lastCameraPosition = cameraTransform.position;

        // Looping: se um background saiu totalmente da tela, manda ele pro final da fila
        for (int i = 0; i < backgrounds.Length; i++)
        {
            float camRelativeX = cameraTransform.position.x - backgrounds[i].position.x;
            if (Mathf.Abs(camRelativeX) >= spriteWidth * (backgrounds.Length / 2f))
            {
                // Descobre o mais à direita/esquerda e reposiciona
                float direction = camRelativeX > 0 ? 1 : -1;
                backgrounds[i].position += new Vector3(spriteWidth * backgrounds.Length * direction, 0, 0);
            }
        }
    }
} 