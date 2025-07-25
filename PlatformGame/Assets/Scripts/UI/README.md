# Sistema UI

## O que é?
Scripts responsáveis pela interface do usuário: HUD, menus, feedback visual, etc.

## Como configurar
- Adicione os prefabs de UI na cena (Canvas, HealthBar, etc).
- Configure referências no Inspector (ex: HeartUIController, CoinUI, etc).

## Integração
- Conecta com GameManager para atualizar vida, moedas, etc.
- Usa eventos para mostrar/ocultar telas (Game Over, Pause, etc).
- Integração com FMODAudioManager para sons de UI.

## Dicas
- Mantenha a UI em um Canvas dedicado.
- Teste responsividade em diferentes resoluções. 