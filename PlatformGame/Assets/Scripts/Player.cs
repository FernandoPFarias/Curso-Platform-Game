using UnityEngine;

public class Player : MonoBehaviour
{

    private Rigidbody2D rig;
    public float speed;

    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
       

    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void Move()
    {
        float moveInput = Input.GetAxis("Horizontal");

        rig.velocity = new Vector2(moveInput * speed, rig.velocity.y);




    }


}
