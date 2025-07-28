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
        // se a ação tiver criada no input system ela vem pra c
        playerControls.Player.Attack.performed += OnBasicAttack;


    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
        playerControls.Player.Attack.performed -= OnBasicAttack;
    }
    // Vamos analisar o OnBasicAttack para possíveis erros que podem travar o ataque:

    // 1. Verifique se o cooldown está correto e se lastAttackTime está sendo atualizado sempre que o ataque é executado.
    // 2. Certifique-se de que o controller.podeAtacar está correto e não está ficando permanentemente falso.
    // 3. Adicione logs detalhados para entender o fluxo.

    // Atualize o método para logs detalhados:
    private void OnBasicAttack(InputAction.CallbackContext context)
    {
        var controller = GetComponent<PlayerController>();
        Debug.Log($"[OnBasicAttack] chamado em {Time.time}. podeAtacar: {(controller != null ? controller.podeAtacar.ToString() : "controller nulo")}, lastAttackTime: {lastAttackTime}, cooldown: {basicAttack.attackCooldown}");

        // Não arremessa mais a pedra aqui! Só ataca normalmente
        if (controller != null && !controller.podeAtacar)
        {
            Debug.Log("Não pode atacar enquanto empurra!");
            return;
        }

        if (Time.time >= lastAttackTime + basicAttack.attackCooldown)
        {
            Debug.Log("[OnBasicAttack] Executando ataque!");
            CurrentAttack = basicAttack;
            if (basicAttack != null)
            {
                basicAttack.Execute(this.gameObject);
            }
            else
            {
                Debug.LogWarning("basicAttack está nulo!");
            }
            lastAttackTime = Time.time;
        }
        else
        {
            float restante = (lastAttackTime + basicAttack.attackCooldown) - Time.time;
            Debug.Log($"ATAQUE EM CDR. Tempo restante: {restante:F2}s");
        }
    }

    private void OnDrawGizmos()
    {
        if (basicAttack == null) return;

        Gizmos.color = Color.blue;

        Vector2 hitBoxCenter = (Vector2)transform.position + new Vector2(basicAttack.hitboxOffSet.x * transform.localScale.x, basicAttack.hitboxOffSet.y * transform.localScale.y);

        Gizmos.DrawWireCube(hitBoxCenter, basicAttack.hitboxSize);


    }




}
