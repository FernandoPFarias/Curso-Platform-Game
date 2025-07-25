# Sistema Enemies

## O que é?
Scripts responsáveis pelo comportamento dos inimigos, IA, ataques e integração com o GameManager.

## Como configurar
- Adicione o prefab do inimigo na cena.
- Configure EnemyData, AIBehaviour e AttackData no Inspector.
- Para bosses, configure BossBehaviour e eventos de morte.

## Integração
- Conecta com GameManager para reset e controle de chefes.
- Usa eventos para abrir barreiras, ativar portais, etc.
- Integração com FMODAudioManager para sons de ataques e morte.

## Dicas
- Ajuste ranges e velocidades para balanceamento.
- Use ScriptableObjects para facilitar ajustes de IA e ataques. 