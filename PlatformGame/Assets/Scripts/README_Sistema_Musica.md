# Sistema de Música - Configuração e Uso

## Visão Geral
Sistema completo de música com transições suaves, que mantém a música entre fases e permite triggers automáticos para áreas especiais (boss, santuário, etc.).

## Componentes do Sistema

### 1. AudioManager
- Gerencia transições suaves entre músicas
- Crossfade automático (sem cortes)
- Dois AudioSources para transições contínuas

### 2. MusicTrigger
- Detecta entrada/saída do player em áreas especiais
- Transição automática para música específica
- Volta para música de fundo ao sair da área

### 3. GameMusicController
- Garante que a música não pare entre fases
- Persiste entre cenas (DontDestroyOnLoad)
- Gerencia música de fundo global

---

## Configuração Inicial

### Passo 1: Criar AudioManager
1. Crie um GameObject vazio na cena
2. Nomeie como "AudioManager"
3. Adicione o script `AudioManager`
4. Configure no Inspector:
   - **Crossfade Time:** 2f (tempo da transição)
   - **Fade Time:** 1f (tempo do fade in/out)
5. O AudioManager será automaticamente persistente entre cenas

### Passo 2: Criar GameMusicController
1. Crie um GameObject vazio na cena
2. Nomeie como "GameMusicController"
3. Adicione o script `GameMusicController`
4. Configure no Inspector:
   - **Default Background Music:** Arraste a música de fundo principal
   - **Fade In Time:** 2f

### Passo 3: Configurar Música de Fundo
1. No `GameMusicController`, arraste a música de fundo para o campo "Default Background Music"
2. A música começará automaticamente e continuará entre fases

---

## Configuração de Áreas Especiais

### Para Área de Boss:
1. Crie um GameObject vazio na área do boss
2. Adicione um Collider2D (BoxCollider2D ou CircleCollider2D)
3. Marque "Is Trigger"
4. Adicione o script `MusicTrigger`
5. Configure no Inspector:
   - **Area Music:** Arraste a música do boss
   - **Fade Time:** 2f (tempo da transição)
   - **Return To Background On Exit:** ✓ (volta para música de fundo ao sair)
   - **Trigger On Enter:** ✓
   - **Trigger On Exit:** ✓

### Para Área de Santuário:
1. Repita os passos acima
2. Configure a música específica do santuário
3. Ajuste o tamanho do trigger conforme necessário

### Para Outras Áreas:
- Use o mesmo padrão para qualquer área que precise de música específica
- Configure a música desejada no campo "Area Music"

---

## Uso via Código

### Transições Manuais:
```csharp
// Transição para música específica
AudioManager.Instance.CrossfadeToMusic(novaMusica, 2f);

// Voltar para música de fundo
AudioManager.Instance.ReturnToBackgroundMusic(2f);

// Fade out da música atual
AudioManager.Instance.FadeOutCurrentMusic(1f);

// Fade in de uma música
AudioManager.Instance.FadeInMusic(musica, 1f);
```

### Controle de Pausa:
```csharp
// Quando o jogo pausa
GameMusicController.Instance.OnGamePaused();

// Quando o jogo despausa
GameMusicController.Instance.OnGameResumed();
```

### Triggers Programáticos:
```csharp
// Forçar transição via script
MusicTrigger trigger = GetComponent<MusicTrigger>();
trigger.TriggerMusic();

// Forçar retorno à música de fundo
trigger.ReturnToBackground();
```

---

## Configurações Avançadas

### Ajustar Tempos de Transição:
- **Crossfade Time:** Tempo da transição entre músicas (padrão: 2f)
- **Fade Time:** Tempo do fade in/out (padrão: 1f)
- **Fade In Time:** Tempo do fade in inicial (padrão: 2f)

### Configurar AudioSources:
- O sistema cria automaticamente dois AudioSources
- Ambos configurados para loop e sem playOnAwake
- Se necessário, pode configurar manualmente no AudioManager

### Múltiplos Triggers:
- Pode ter vários MusicTriggers na mesma cena
- Cada um com sua música específica
- O sistema gerencia automaticamente as transições

---

## Troubleshooting

### Problema: Música não toca
**Solução:**
1. Verifique se o AudioManager existe na cena
2. Confirme se o AudioClip está atribuído
3. Verifique se o volume não está em 0
4. Confirme se o AudioSource está ativo

### Problema: Transição não funciona
**Solução:**
1. Verifique se o MusicTrigger tem Collider2D configurado como "Is Trigger"
2. Confirme se o player tem a tag "Player"
3. Verifique se o AudioClip está atribuído no MusicTrigger
4. Confirme se o AudioManager está funcionando

### Problema: Música para entre fases
**Solução:**
1. Verifique se o GameMusicController está na cena inicial
2. Confirme se tem "DontDestroyOnLoad" configurado
3. Verifique se a música de fundo está atribuída

### Problema: Múltiplas músicas tocando
**Solução:**
1. Verifique se não há múltiplos AudioManagers
2. Confirme se os triggers não estão sobrepostos
3. Verifique se o sistema de crossfade está funcionando

---

## Exemplos de Uso

### Exemplo 1: Boss Fight
```
1. Player entra na área do boss
2. MusicTrigger detecta entrada
3. Crossfade automático para música do boss
4. Player sai da área
5. Volta automaticamente para música de fundo
```

### Exemplo 2: Santuário
```
1. Player entra no santuário
2. Música muda para tema do santuário
3. Player permanece no santuário
4. Música continua até sair
5. Volta para música de fundo ao sair
```

### Exemplo 3: Transição Manual
```csharp
// Em um script de boss
public void OnBossDefeated()
{
    AudioManager.Instance.CrossfadeToMusic(victoryMusic, 1f);
}
```

---

## Dicas Importantes

1. **Sempre teste as transições** antes de finalizar
2. **Ajuste os tempos** conforme necessário para seu jogo
3. **Use triggers pequenos** para áreas específicas
4. **Configure volumes** adequados para cada música
5. **Teste entre fases** para garantir continuidade

---

## Estrutura de Arquivos
```
Assets/Scripts/
├── AudioManager.cs          # Gerencia transições
├── MusicTrigger.cs          # Triggers de área
├── GameMusicController.cs   # Controle global
└── README_Sistema_Musica.md # Este arquivo
```

---

## Suporte
Para dúvidas ou problemas, verifique:
1. Console do Unity para erros
2. Configurações dos scripts no Inspector
3. Tags e layers dos objetos
4. Configurações de áudio do Unity 