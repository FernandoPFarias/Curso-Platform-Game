# Sistema Game

## O que é?
Scripts responsáveis pela lógica geral do jogo: gerenciamento de cena, parallax, efeitos visuais, GameManager, etc.

## Como configurar
- Certifique-se de que o GameManager está presente na cena.
- Configure referências globais (player, câmera, UI, etc) no Inspector do GameManager.
- Adicione scripts de parallax e efeitos visuais conforme necessário.

## Integração
- Centraliza o controle de vida, respawn, moedas, etc.
- Conecta com todos os outros sistemas (Player, Enemies, UI, etc).
- Integração com FMODAudioManager para música global.

## Dicas
- Use o GameManager como fonte da verdade para dados globais.
- Documente dependências no Inspector. 