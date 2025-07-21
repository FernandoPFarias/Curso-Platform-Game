# Curso Platform Game - README

## Visão Geral
Projeto de plataforma 2D com foco em game feel, sistemas de vida, respawn, chefes (bosses) inteligentes e integração robusta de UI, checkpoints e transições de cena.

---

## Mudanças Recentes (Diário)

### Último ciclo:
- Boss reseta para posição inicial após respawn do player e só volta a perseguir após nova ativação.
- Boss morre corretamente: animação de morte, scripts de ataque e hitboxes desativados, objeto destruído após a animação.
- Sistema de vida/respawn do player separado entre penalidade (DeathZone) e morte (vida zerada).
- Toda lógica de dano centralizada em `TakeDamage`.
- DeathZone usa `RespawnFromDeathZone(penalty)`.
- UI e GameManager sempre atualizados após respawn.
- Todos os bosses resetados após respawn do player.
- Ataques de inimigos e dano de contato usam apenas `TakeDamage`.
- DeathZone usa método correto para penalidade e respawn.
- Bosses não caçam o player até o checkpoint após respawn.

---

## Como Funciona

### Sistema de Vida e Respawn
- **Penalidade (DeathZone):** Player perde vida, respawna com a vida restante, não perde vida extra.
- **Morte (vida zerada):** Player perde uma vida extra, respawna com vida cheia.
- Toda a lógica de dano e respawn está centralizada no `PlayerHealth`.

### Boss
- Só persegue o player após ativação.
- Se o player morrer, reseta para a posição inicial e só volta a perseguir após nova ativação.
- Ao morrer, executa animação de morte, desativa scripts de ataque e hitboxes, e é destruído após a animação.

### UI e GameManager
- Sempre atualizados após respawn ou troca de cena.

---

## Boas Práticas
- Centralize lógica de dano e respawn.
- Desative scripts/hitboxes ao morrer para evitar bugs de dano pós-morte.
- Use métodos públicos para resetar inimigos especiais (ex: bosses).
- Atualize UI e referências do GameManager após respawn ou troca de cena.
- Sempre teste todos os fluxos após mudanças estruturais.

---

## Checklist para Testes
- [ ] Respawn após morte e penalidade.
- [ ] Morte do boss.
- [ ] Reset do boss após respawn do player.
- [ ] Troca de cena (corrigir se necessário).
- [ ] UI de vida e vidas extras sempre atualizada.
- [ ] Boss não ataca após morrer.

---

## Próximos Passos / Ideias para o Futuro
- [ ] Adicionar novos padrões de ataque para o boss (ex: ataques especiais, minions, teleporte).
- [ ] Melhorar feedback visual/sonoro das mortes e respawns.
- [ ] Polir transições de cena e loading screen.
- [ ] Implementar sistema de save/load de progresso.
- [ ] Adicionar achievements ou desafios extras.

---

## Bugs Conhecidos / Sugestões
- [ ] (Anote aqui qualquer bug encontrado ou sugestão para revisar no próximo ciclo)

---

**Dúvidas, sugestões ou bugs: anote aqui para revisar no próximo ciclo!**

