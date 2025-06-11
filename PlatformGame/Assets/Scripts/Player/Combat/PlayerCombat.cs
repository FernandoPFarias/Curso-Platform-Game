using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCombat : MonoBehaviour
{
    [Header("Attacks")]
    public AttackData basicAttack;
    //No futuro, posso por mais tipos de attack

    private InputSystem_Actions playerControls;

    private float lastAttackTime; // segura o tempo do ultimo ataque

    public AttackData CurrentAttack { get; private set; }

    private void Awake()
    {
        playerControls = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        // se a ação tiver criada no input system ela vem pra cá
        playerControls.Player.Attack.performed += OnBasicAttack;


    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
        playerControls.Player.Attack.performed -= OnBasicAttack;
    }

    private void OnBasicAttack(InputAction.CallbackContext context)
    {

        if (Time.time >= lastAttackTime + basicAttack.attackCooldown)
        {


            CurrentAttack = basicAttack;
            basicAttack?.Execute(this.gameObject);


            lastAttackTime = Time.time;

        }
        else
        {
            Debug.Log("ATAQUE EM CDR");
        }

    }



    private void OnDrawGizmosSelected()
    {
        if (basicAttack == null) return;

        Gizmos.color = Color.blue;

        Vector2 hitBoxCenter = (Vector2)transform.position + new Vector2(basicAttack.hitboxOffSet.x * transform.localScale.x, basicAttack.hitboxOffSet.y * transform.localScale.y);

        Gizmos.DrawSphere(hitBoxCenter, basicAttack.hitboxRadius);


    }




}
