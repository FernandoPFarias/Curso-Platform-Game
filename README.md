# Curso Platform Game - Boss Behaviour

## Histórico de Melhorias no Boss

### 1. Anti-Grude (Distância Mínima)
- Implementada lógica para o boss nunca ficar sobreposto ao player.
- O boss detecta quando o player entra na `minDistanceToPlayer` (zona azul) e foge para o lado oposto até sair dessa zona.
- A distância mínima pode ser ajustada no Inspector do EnemyData do boss.

### 2. Ataques Inteligentes
- O boss só ataca se o player está **fora da distância mínima** e **dentro do range de ataque** (círculos magenta/ciano).
- O boss nunca ataca grudado no player.

### 3. Corrotina de IA
- Toda a lógica do boss foi migrada para uma corrotina principal (`BossAI`), facilitando manutenção e expansão.
- Estados controlados: Perseguir, Fugir, Atacar, Cooldown.
- Tempos de ataque e cooldown são facilmente ajustáveis.

### 4. Fuga Robusta
- Durante a fuga, o boss:
  - Calcula um ponto alvo de fuga (lado oposto ao player).
  - Move-se rapidamente até sair da zona mínima.
  - Ignora colisão com o player durante a fuga para nunca ficar travado.
  - Reativa colisão ao final da fuga.

### 5. Ajustes e Dicas Futuras
- Para ajustar o game feel:
  - Edite `minDistanceToPlayer` para calibrar a zona de anti-grude.
  - Ajuste `escapeSpeed` para controlar a velocidade de fuga.
  - Modifique os ranges e offsets dos ataques para polir a área de ataque.
- Para adicionar novos comportamentos:
  - Crie novas corrotinas para ataques especiais, padrões de movimento, etc.
  - Use a estrutura de estados já pronta para transições suaves.

---

## Como funciona o Boss

1. **Persegue o player** até entrar no range de ataque.
2. **Se o player chega muito perto**, o boss foge até estar seguro.
3. **Ataca** apenas se está na distância correta.
4. **Nunca fica sobreposto ao player**.

---

## Pontos de partida para próximos dias
- Adicionar novos padrões de ataque usando corrotinas.
- Implementar teleporte, invocação de minions, ou ataques de área.
- Polir animações e feedbacks visuais/sonoros.
- Testar diferentes valores de distância mínima e ranges para balanceamento.

---

**Qualquer dúvida ou ajuste, consulte este README ou peça ajuda!**

