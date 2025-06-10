using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Listen to Event Channels")]
    public GameEvent OnGroundJumpEvent;
    public GameEvent OnAirJumpEvent;
    public GameEvent OnLandedEvent;

    // --- Referências ---
    private Animator animator;
    private PlayerController playerController;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        playerController = GetComponentInParent<PlayerController>();
    }

    private void OnEnable()
    {
        // Inscreve cada método ao seu respectivo canal de evento
        OnGroundJumpEvent?.RegisterListener(HandleGroundJump);
        OnAirJumpEvent?.RegisterListener(HandleAirJump);
        OnLandedEvent?.RegisterListener(HandleLanded);
    }

    private void OnDisable()
    {
        // Se desinscreve de todos os canais
        OnGroundJumpEvent?.UnregisterListener(HandleGroundJump);
        OnAirJumpEvent?.UnregisterListener(HandleAirJump);
        OnLandedEvent?.UnregisterListener(HandleLanded);
    }

    private void Update()
    {
        // Atualiza os parâmetros contínuos a cada frame
        animator.SetFloat("HorizontalSpeed", Mathf.Abs(playerController.CurrentVelocity.x));
        animator.SetBool("isGrounded", playerController.IsGrounded);
        animator.SetFloat("VerticalSpeed", playerController.CurrentVelocity.y);
    }

    // --- MÉTODOS QUE REAGEM AOS EVENTOS ---

    private void HandleGroundJump()
    {
        animator.SetTrigger("GroundJump");
    }

    private void HandleAirJump()
    {
        animator.SetTrigger("DoubleJump");
    }



    private void HandleLanded()
    {
        animator.SetTrigger("Land");
    }
}