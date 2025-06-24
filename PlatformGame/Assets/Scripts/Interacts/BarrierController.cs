using UnityEngine;

public class BarrierController : MonoBehaviour
{
    public int botoesNecessarios = 0; // Quantos botões precisam ser apertados
    private int botoesApertados = 0;
    public Animator anim;
    public Collider2D barrierCollider;

    public void BotaoApertado()
    {
        Debug.Log("Botão apertado2");
        botoesApertados++;
        if (botoesApertados >= botoesNecessarios)
        {
            anim.SetBool("B_BarrieDown", true);
            barrierCollider.enabled = false;
        }
    }

    public void BotaoSolto()
    {
        botoesApertados--;
        if (botoesApertados < botoesNecessarios)
        {
            anim.SetBool("B_BarrieDown", false);
            barrierCollider.enabled = true;
        }
    }
} 