using UnityEngine;

public class ShootingStar : MonoBehaviour
{
    private Vector2 velocity;
    private Rect area;
    private Animator animator;

    public void Init(Vector2 velocity, Rect area)
    {
        this.velocity = velocity;
        this.area = area;
        animator = GetComponent<Animator>();
       
    }

    void Update()
    {
        transform.position += (Vector3)velocity * Time.deltaTime;

        // Se sair da área, destrói
        if (!area.Contains((Vector2)transform.position))
            Destroy(gameObject);
    }
} 