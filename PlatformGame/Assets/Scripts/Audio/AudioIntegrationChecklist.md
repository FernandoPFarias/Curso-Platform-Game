# Checklist de Integração de Áudio (FMOD)

Este checklist serve para orientar o time de áudio sobre todos os pontos do jogo que precisam de eventos sonoros, facilitando a criação e organização dos eventos no FMOD Studio.

---

## 1. Player
- [ ] Pulo (`event:/SFX/Player/Jump`)
- [ ] Ataque básico (`event:/SFX/Player/Attack`)
- [ ] Dano recebido (`event:/SFX/Player/Hit`)
- [ ] Morte (`event:/SFX/Player/Death`)
- [ ] Coleta de item (`event:/SFX/Player/Collect`)
- [ ] Respawn (`event:/SFX/Player/Respawn`)

## 2. Inimigos
- [ ] Ataque de inimigo comum (`event:/SFX/Enemy/Attack`)
- [ ] Dano recebido (`event:/SFX/Enemy/Hit`)
- [ ] Morte de inimigo (`event:/SFX/Enemy/Death`)
- [ ] Ataque de boss (`event:/SFX/Boss/Attack`)
- [ ] Morte de boss (`event:/SFX/Boss/Death`)

## 3. Interações
- [ ] Portal ativado (`event:/SFX/Interact/Portal`)
- [ ] Checkpoint ativado (`event:/SFX/Interact/Checkpoint`)
- [ ] DeathZone (queda) (`event:/SFX/Interact/DeathZone`)
- [ ] Alavanca acionada (`event:/SFX/Interact/Lever`)
- [ ] Barreira abrindo (`event:/SFX/Interact/BarrierOpen`)
- [ ] Botão de puzzle pressionado (`event:/SFX/Interact/Button`)

## 4. Colecionáveis
- [ ] Moeda coletada (`event:/SFX/Collectable/Coin`)
- [ ] Vida extra coletada (`event:/SFX/Collectable/ExtraLife`)
- [ ] Coração de vida coletado (`event:/SFX/Collectable/Heart`)

## 5. UI
- [ ] Clique em botão (`event:/UI/Click`)
- [ ] Abrir menu (`event:/UI/MenuOpen`)
- [ ] Game Over (`event:/UI/GameOver`)
- [ ] Notificação de conquista (`event:/UI/Achievement`)

## 6. Música
- [ ] Música de fase normal (`event:/Music/Level`)
- [ ] Música de boss (`event:/Music/Boss`)
- [ ] Música de vitória (`event:/Music/Victory`)
- [ ] Música de game over (`event:/Music/GameOver`)

---

## Observações para o time de áudio
- Os nomes dos eventos são sugestões e podem ser adaptados conforme o padrão do projeto no FMOD Studio.
- Sempre exporte os bancos após criar/alterar eventos.
- Avise o time de programação ao adicionar novos eventos para integração.
- Para sons dinâmicos (ex: volume, pitch, variações), use parâmetros no FMOD Studio.

---
Dúvidas? Consulte o README de áudio ou fale com o responsável técnico. 