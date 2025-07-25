# Guia de Áudio do Projeto

## Estrutura de Áudio

- **Scripts/Audio/**: Scripts de áudio do projeto (FMODAudioManager, FadeController, etc).
- **Audio/SFX/**: Efeitos sonoros locais (WAV, OGG, etc).
- **Audio/Music/**: Músicas locais (se usar).
- **Audio/FMOD/**: Bancos exportados do FMOD Studio (se usar).

## FMODAudioManager
- Singleton para tocar músicas e efeitos do FMOD.
- Arraste eventos do FMOD Studio para os campos públicos no Inspector.
- Exemplo de uso no código:
  ```csharp
  FMODAudioManager.Instance.PlaySFX(FMODAudioManager.Instance.jumpSFX);
  FMODAudioManager.Instance.PlayMusic(FMODAudioManager.Instance.mainTheme);
  FMODAudioManager.Instance.StopMusic();
  ```

## FadeController
- Controla transições de fade in/out na tela.
- Pode ser usado junto com eventos de áudio para transições suaves.

## Como Integrar Novos Áudios
1. Crie o evento no FMOD Studio.
2. Exporte os bancos para `Assets/Audio/FMOD/`.
3. Arraste o evento para o campo correspondente no Inspector do FMODAudioManager.
4. Chame o método desejado no código ou via UnityEvent.

## Dicas para o Time de Áudio
- Sempre exporte os bancos após alterações no FMOD Studio.
- Use nomes claros para eventos.
- Consulte o programador para dúvidas de integração.

---
Dúvidas? Consulte este README ou fale com o responsável técnico. 