using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    public abstract void SetDirection(Vector2 direction);
    public abstract void SetDamage(float damage);
} 