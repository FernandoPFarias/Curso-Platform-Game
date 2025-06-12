using UnityEngine;


// ... (RequireComponent continua o mesmo)
public class Enemy : MonoBehaviour
{
    [Header("Asset do Inimigo aqui")]
    public EnemyData _enemyData;

    [Header("Patrol Points")]
    public Transform _pointA;
    public Transform _pointB;

    // Propriedades públicas para que os estados possam acessar os componentes do inimigo
    public Rigidbody2D Rb { get; private set; }
    public Transform PlayerTarget { get; private set; }

    public Animator Animator {  get; private set; } 


    public EnemyData EnemyData => _enemyData; // Forma curta de expor a variável
    public Transform PointA => _pointA;
    public Transform PointB => _pointB;

    private AIState currentState; // O especialista que está trabalhando agora
    
    private void Awake()
    {

        Rb = GetComponent<Rigidbody2D>();
        Animator = GetComponentInChildren<Animator>();
    }   

    private void Start()
    {
        Rb.gravityScale = 2f;
        Rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (_enemyData == null )
        {
            Debug.LogError($"O Inimigo {gameObject.name} não tem um EnemyData instalado!!!!", this);
            this.enabled = false; // vai desativar esse script caso ele não tenha dados
            return;
        }


        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if(playerObject != null)
        {
            PlayerTarget = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Inimigo não conseguiu encontrar o Jogador. Verifique se o jogador tem a tag 'Player'.", this);
            this.enabled=false;
        }

        ChangeState(new PatrolState(this));

    }


    private void Update()
    {
        Animator.SetFloat("StartPatrol", Mathf.Abs(Rb.linearVelocity.x));
    }

    private void FixedUpdate()
    {
        currentState?.Tick();

        

    }

    public void ChangeState(AIState newState)
    {

        currentState?.OnExit();


        currentState = newState;


        currentState?.OnEnter();

    }

    #region -- Métodos de Utilidade (Ações que o corpo do inimigo pode fazer) --
    public void MoveTowards(Vector2 targetPosition)
    {

        if (PlayerTarget == null) return;
        Vector2 direction = (targetPosition - (Vector2) transform.position) . normalized;
        Rb.linearVelocity = new Vector2(direction.x * _enemyData.moveSpeed, Rb.linearVelocity.y);
        FlipTowards(targetPosition);

    }

    public void FlipTowards(Vector2 targetPosition)
    {

        float directionX = targetPosition.x - transform.position.x;

        if (Mathf.Abs(directionX) > 0.1f )
        {
            transform.localScale = new Vector3(Mathf.Sign(directionX), 1f, 1f);

            float facingDirection = Mathf.Sign(directionX);


            //aqui só se os personagens ja nascerem virados pra esquerda se eu quiser pra direita é só tirar o negativo
            transform.localScale = new Vector3(-facingDirection, 1f,1f);





        }




    }

    #endregion


    private void OnDrawGizmosSelected()
    {
        if (_enemyData == null) return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, _enemyData.detectionRange);

        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, _enemyData.giveUpRange);

        if (_enemyData is MeleeEnemyData meleeData)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, meleeData.attackRadius);
        }
    }



}