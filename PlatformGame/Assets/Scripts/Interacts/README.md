# Sistema Interacts

## O que é?
Scripts responsáveis por interações do jogador com o cenário: portais, checkpoints, deathzone, alavancas, barreiras, etc.

## Como configurar
- Adicione o prefab do objeto interativo na cena.
- Configure eventos e referências no Inspector.
- Para barreiras de boss, referencie o boss ou evento de morte.

## Integração
- Conecta com PlayerInteraction para acionar interações.
- Usa UnityEvents para acionar sons e efeitos.
- Integração com FMODAudioManager para sons de interação.

## Dicas
- Teste triggers e colisores após mudanças.
- Use eventos para desacoplar lógica de interação. 