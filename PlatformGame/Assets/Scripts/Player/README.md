# Sistema Player

## O que é?
Scripts responsáveis pelo controle do jogador, saúde, interação com objetos e combate.

## Como configurar
- Adicione o prefab do Player na cena.
- Certifique-se de que os scripts PlayerController, PlayerHealth, PlayerInteraction e PlayerCombat estão no GameObject do Player.
- Configure referências no Inspector conforme necessário (ex: pontos de spawn, UI, etc).

## Integração
- Conecta com GameManager para controle global de vida e respawn.
- Usa eventos para interagir com objetos (alavancas, portais, etc).
- Integração com FMODAudioManager para sons de ações do jogador.

## Dicas
- Sempre teste colisões e interações após mudanças.
- Consulte o script PlayerHealth para lógica de respawn e penalidades. 