# Configuração Completa do FMOD

## Passo a Passo para Configurar FMOD

### 1. Instalar FMOD Studio
1. **Baixe o FMOD Studio**: https://www.fmod.com/download
2. **Instale o FMOD Studio** (versão gratuita)
3. **Crie uma conta** se necessário

### 2. Criar Projeto FMOD
1. **Abra o FMOD Studio**
2. **Crie um novo projeto**
3. **Configure o projeto**:
   - Nome: "PlatformGame"
   - Sample Rate: 48000 Hz
   - Platform: Windows

### 3. Criar Eventos de Áudio
1. **Crie eventos para seu jogo**:
   - `event:/Musica_Exploração` (música de fundo)
   - `event:/Musica_Santuario` (música de santuário)
   - `event:/Jump` (som de pulo)
   - `event:/Coin` (som de moeda)
   - `event:/Attack` (som de ataque)
   - `event:/Death` (som de morte)

### 4. Configurar Eventos
Para cada evento:
1. **Arraste um arquivo de áudio** para o evento
2. **Configure o tipo**:
   - Música: Loop, 2D
   - SFX: One-shot, 2D
3. **Ajuste volume e efeitos** conforme necessário

### 5. Organizar em Banks
1. **Crie banks separados**:
   - `Master.bank` (eventos principais)
   - `Music.bank` (músicas)
   - `SFX.bank` (efeitos sonoros)

2. **Organize os eventos** nos banks apropriados

### 6. Gerar Arquivos .bank
1. **Vá em Build → Build All Platforms**
2. **Isso vai gerar**:
   - `Master.bank`
   - `Master.strings`
   - `Music.bank`
   - `SFX.bank`

### 7. Copiar para Unity
1. **Copie os arquivos .bank** para:
   ```
   PlatformGame/Assets/Plugins/FMOD/
   ```

2. **Certifique-se de que os arquivos estão na pasta correta**

### 8. Configurar Unity
1. **Abra o Unity**
2. **Vá em Window → FMOD → Settings**
3. **Configure**:
   - Live Update: habilitado
   - Live Update Port: 9264
   - Build Directory: Assets/Plugins/FMOD/

### 9. Configurar Project Settings
```
Project Settings → Audio:
- Disable Unity Audio: ❌ (desmarcado)
- System Sample Rate: 48000 Hz
- DSP Buffer Size: Best Performance
```

### 10. Testar Configuração
1. **Adicione o script `FMODBankManager.cs`** a um GameObject
2. **Execute a cena**
3. **Pressione T** para testar eventos
4. **Verifique os logs** no Console

## Scripts Necessários

### FMODBankManager.cs
- Carrega banks manualmente
- Testa eventos
- Verifica disponibilidade

### FMODAudioManager.cs
- Gerenciador principal de áudio
- Métodos para tocar música e SFX
- Singleton para acesso global

## Estrutura de Arquivos

```
PlatformGame/Assets/Plugins/FMOD/
├── Resources/
│   └── FMODStudioSettings.asset
├── Master.bank
├── Master.strings
├── Music.bank
├── SFX.bank
└── [outros arquivos .bank]
```

## Teste de Configuração

### 1. Verificar Banks
```csharp
// No FMODBankManager
LoadBank("Master");
LoadBank("Music");
LoadBank("SFX");
```

### 2. Testar Eventos
```csharp
// Testar eventos
RuntimeManager.PlayOneShot("event:/Musica_Exploração");
RuntimeManager.PlayOneShot("event:/Jump");
```

### 3. Verificar Logs
- ✅ Banks carregados
- ✅ Eventos disponíveis
- ✅ Áudio reproduzindo

## Próximos Passos

1. **Instale o FMOD Studio**
2. **Crie o projeto FMOD**
3. **Configure os eventos**
4. **Gere os banks**
5. **Copie para Unity**
6. **Teste com FMODBankManager**

## Troubleshooting

### Problemas Comuns:
- ❌ Banks não encontrados
- ❌ Eventos não disponíveis
- ❌ Áudio não reproduz
- ❌ Configuração incorreta

### Soluções:
- Verificar caminho dos banks
- Verificar configurações do FMOD
- Verificar Project Settings
- Testar com scripts de diagnóstico 