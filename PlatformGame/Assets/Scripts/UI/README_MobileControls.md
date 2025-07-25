# Sistema de Controles Mobile

## O que é?
Scripts e UI para controlar o player em dispositivos mobile usando toques na tela e botões.

## Como configurar
1. No Canvas, crie um GameObject chamado `MobileControls`.
2. Adicione um painel (Image transparente) cobrindo o lado esquerdo da tela e adicione o script `MobileTouchArea`.
   - Esse painel detecta toques/arrastos e armazena o movimento em `MobileInput.move`.
3. Adicione botões UI para ataque e interação no lado direito da tela.
   - Adicione o script `MobileButton` em cada botão e selecione o tipo (Attack ou Interact) no Inspector.
   - Os botões setam flags em `MobileInput.attackPressed` e `MobileInput.interactPressed`.
4. O `PlayerController` lê essas variáveis no Update (em mobile) e executa as ações correspondentes.

## Integração
- O `MobileTouchArea` atualiza `MobileInput.move`.
- Os botões setam flags em `MobileInput`.
- O sistema funciona junto com teclado/controle, sem conflito.

## Dicas
- O painel de toque pode ser invisível (Image com alpha 0).
- Ajuste o threshold de movimento no script para sensibilidade.
- Teste em build mobile ou com touch simulation no editor. 