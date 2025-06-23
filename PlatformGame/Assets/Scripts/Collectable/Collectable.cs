using UnityEngine;

public abstract class Collectable : MonoBehaviour
{
    // Método chamado quando o item é coletado
    public abstract void Collect(GameObject collector);
} 