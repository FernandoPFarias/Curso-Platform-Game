# Sistema de Checkpoint

## Visão Geral

O sistema de checkpoint foi dividido em **duas responsabilidades**:

1. **`CampfireController`** - Controla apenas os efeitos visuais e sonoros da fogueira
2. **`Checkpoint`** - Detecta interação do player, salva spawn point e coordena a fogueira

## Como Funciona

### CampfireController
- **Responsabilidade**: Controlar efeitos da fogueira (chama, brilho, faíscas, partículas, som)
- **Não altera**: Posição, rotação, velocidade ou qualquer transform
- **Métodos**:
  - `LightFire()` - Acende a fogueira
  - `ExtinguishFire()` - Apaga a fogueira
  - `IsLit()` - Verifica se está acesa

### Checkpoint
- **Responsabilidade**: Detectar interação do player, salvar spawn point e coordenar a fogueira
- **Métodos**:
  - `ActivateCheckpoint()` - Ativa o checkpoint, salva spawn point e acende a fogueira
  - `IsActivated()` - Verifica se está ativado
  - `ResetCheckpoint()` - Reseta o checkpoint e apaga a fogueira

## Como Configurar

### 1. Configurar CampfireController
1. **Adicione o script** `CampfireController` ao GameObject da fogueira
2. **Configure os efeitos** via Inspector:
   - `flameEffect` - GameObject da chama
   - `glowEffect` - GameObject do brilho
   - `sparkEffect` - GameObject das faíscas
   - `fireParticleSystems` - Array de ParticleSystems
   - `fireSound` - AudioClip do som da fogueira
   - `audioSource` - AudioSource para tocar o som

### 2. Configurar Checkpoint
1. **Adicione o script** `Checkpoint` ao GameObject do checkpoint
2. **Configure via Inspector**:
   - `spawnPoint` - Transform onde o player vai renascer
   - `playerLayer` - LayerMask do player (padrão: Default)
   - `campfireController` - Referência para o CampfireController
   - `interactionCollider` - Referência para o Collider2D filho (opcional - detecta automaticamente)
   - `checkpointSound` - AudioClip do som de checkpoint
   - `audioSource` - AudioSource para tocar o som
   - `checkpointUI` - Referência para CheckpointUI (opcional)

### 3. Configurar Collider
1. **Adicione um Collider2D** ao GameObject filho (fogueira)
2. **Configure como Trigger**
3. **Ajuste o tamanho** para detectar o player
4. **O script detecta automaticamente** o collider nos filhos, ou configure manualmente via `interactionCollider`

## Estrutura Recomendada

```
Checkpoint GameObject
├── Checkpoint.cs
├── Collider2D (Trigger)
└── Campfire GameObject (filho)
    ├── CampfireController.cs
    ├── Sprite da fogueira
    ├── Flame Effect (filho)
    ├── Glow Effect (filho)
    ├── Spark Effect (filho)
    └── Particle Systems (filhos)
```

## Fluxo de Funcionamento

1. **Início do jogo**: Fogueira apagada, checkpoint desativado
2. **Player entra no range**: Checkpoint detecta presença
3. **Player pressiona E/Interact**: 
   - Salva spawn point no GameManager
   - Chama `campfireController.LightFire()`
   - Toca som de checkpoint
   - Mostra UI de feedback
4. **Player morre**: Renasce no último checkpoint ativado

## Vantagens da Separação

- ✅ **Responsabilidades claras**: Cada script tem uma função específica
- ✅ **Reutilização**: CampfireController pode ser usado em outras fogueiras
- ✅ **Manutenção**: Mais fácil de debugar e modificar
- ✅ **Flexibilidade**: Pode ter fogueiras sem checkpoint e vice-versa
- ✅ **Performance**: Sem detecção automática, apenas referências via Inspector
- ✅ **Simplicidade**: Apenas 2 scripts para gerenciar tudo 