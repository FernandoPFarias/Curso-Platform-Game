using UnityEngine;

public class BossBarrierController : MonoBehaviour
{
    [Header("Referência do Boss (EnemyHealth)")]
    public EnemyHealth boss;
    [Header("OU Evento de Morte (GameEvent)")]
    public GameEvent bossDeathEvent;

    [Header("Componentes da Barreira")]
    public Animator anim;
    public Collider2D barrierCollider;
    [Header("Parâmetro do Animator para abrir")]
    public string openParameter = "B_BarrieDown";

    private bool isOpen = false;

    private void OnEnable()
    {
        if (boss != null)
            boss.onDeathEvent.RegisterListener(OnBossDeath);
        if (bossDeathEvent != null)
            bossDeathEvent.RegisterListener(OnBossDeath);
    }

    private void OnDisable()
    {
        if (boss != null)
            boss.onDeathEvent.UnregisterListener(OnBossDeath);
        if (bossDeathEvent != null)
            bossDeathEvent.UnregisterListener(OnBossDeath);
    }

    private void OnBossDeath()
    {
        AbrirBarreira();
    }

    public void AbrirBarreira()
    {
        if (isOpen) return;
        isOpen = true;
        if (anim != null && !string.IsNullOrEmpty(openParameter))
            anim.SetBool(openParameter, true);
        if (barrierCollider != null)
            barrierCollider.enabled = false;
    }
} 