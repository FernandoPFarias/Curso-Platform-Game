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
        Vector2 movemet = Input.GetAxis("Horizontal");

        rig.linearVelocity = new Vector2(movemet * speed, rig.linearVelocity.y);




    }


}
