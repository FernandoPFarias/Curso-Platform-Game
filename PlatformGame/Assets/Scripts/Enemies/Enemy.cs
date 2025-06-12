using UnityEngine;
using UnityEngine.XR;

// ... (RequireComponent continua o mesmo)
public class Enemy : MonoBehaviour
{
    [Header("Asset do Inimigo aqui")]
    public EnemyData enemyData;

    [Header("Patrol Points")]
    public Transform _pointA;
    public Transform _pointB;

    // Propriedades públicas para que os estados possam acessar os componentes do inimigo
    public Rigidbody2D Rb { get; private set; }
    public Transform PlayerTarget { get; private set; }


    public EnemyData EnemyData => _enemyData; // Forma curta de expor a variável
    public Transform PointA => _pointA;
    public Transform PointB => _pointB;

    private AIState currentState; // O especialista que está trabalhando agora
    
    private void Awake()
    {

        Rb = GetComponent<Rigidbody2D>();

    }   

    private void Start()
    {
        Rb.gravityScale = 2f;
        Rb.constraints = Rigidbody2D.FreezeRotation;

        if (_enemyData == null )
        {
            Debug.LogError($"O Inimigo {GameObject.name} não tem um EnemyData instalado!!!!", this);
            this.enabled = false; // vai desativar esse script caso ele não tenha dados
            return;
        }


        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");

        if(playerObject == null)
        {
            PlayerTarget = playerObject.transform;
        }
        else
        {
            Debug.LogWarning("Inimigo não conseguiu encontrar o Jogador. Verifique se o jogador tem a tag 'Player'.", this);
        }

        ChangeState(new PatrolState(this));

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
}