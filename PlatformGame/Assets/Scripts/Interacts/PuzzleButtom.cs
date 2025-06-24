using UnityEngine;

public class PuzzleButtom : MonoBehaviour
{

    private Animator anim;
    public Animator barrierAnim;    
    public BarrierController barrierController;

    // IMPORTANTE: No Inspector, marque as layers 'Player' e 'Empurravel' no LayerMask 'layer' para que o botão detecte tanto o player quanto objetos empurráveis.
    public LayerMask layer;
    
    [SerializeField]
    private Vector2 boxSize = new Vector2(1, 1);

    [SerializeField]
    private float tempoParaAtivar = 0.5f; // Tempo em segundos para ativar o botão

    private float tempoAtual = 0f;
    private bool objetoEmCima = false;
    private bool isPressed = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
        
    }


    private void OnPressed()
    {   Debug.Log("Botão pressionado");
        anim.SetBool("B_IsPressed", true);
        if (barrierController != null)
        {
            barrierController.BotaoApertado();
        }
    }



    private void OnExit()
    {
        anim.SetBool("B_IsPressed", false);
        if (barrierController != null)
        {
            barrierController.BotaoSolto();
        }
    }

    



    private void FixedUpdate()
    {
        OnCollision();
        if (objetoEmCima && !isPressed)
        {
            tempoAtual += Time.fixedDeltaTime;
            if (tempoAtual >= tempoParaAtivar)
            {
                OnPressed();
                isPressed = true;
            }
        }
        else if (!objetoEmCima)
        {
            tempoAtual = 0f;
        }
    }

    private void OnCollision()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position, boxSize, 0, layer);

        if (hit != null)
        {
            objetoEmCima = true;
        }
        else
        {
            if (isPressed)
            {
                OnExit();
                isPressed = false;
            }
            objetoEmCima = false;
            tempoAtual = 0f;
        }
    }


   private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(transform.position, boxSize);
    }



}
