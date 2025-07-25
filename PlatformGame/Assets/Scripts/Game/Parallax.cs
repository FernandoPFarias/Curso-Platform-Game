using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform cameraTransform; // Arraste a câmera principal aqui (ou deixe vazio para usar a Main Camera)
    public float parallaxMultiplier = 0.5f; // 0 = não move, 1 = move igual à câmera

    private Vector3 lastCameraPosition;

    void Start()
    {
        if (cameraTransform == null)
            cameraTransform = Camera.main.transform;

        lastCameraPosition = cameraTransform.position;
    }

    void LateUpdate()
    {
        Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
        transform.position += new Vector3(deltaMovement.x * parallaxMultiplier, deltaMovement.y * parallaxMultiplier, 0);
        lastCameraPosition = cameraTransform.position;
    }
} 