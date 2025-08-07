# Troubleshooting de Áudio

## Problema: Áudio toca mas não sai som

### Verificações Rápidas

#### 1. Volume do Sistema
- **Verifique o volume do Windows** (ícone de alto-falante)
- **Verifique se não está no mudo**
- **Teste com outros programas** (YouTube, Spotify)

#### 2. Volume do Unity
- **Verifique AudioListener.volume** (deve ser 1)
- **Verifique AudioSource.volume** (deve ser > 0)
- **Verifique se não está no mudo**

#### 3. Configurações do Projeto
```
Project Settings → Audio:
- Disable Unity Audio: ❌ (desmarcado)
- System Sample Rate: 48000 Hz
- DSP Buffer Size: Best Performance
```

#### 4. AudioListener na Cena
- **Deve haver um AudioListener** na cena
- **Geralmente está na Main Camera**
- **Verifique se não está desabilitado**

### Teste com AudioDiagnostic

1. **Adicione o script `AudioDiagnostic.cs`** a um GameObject
2. **Arraste um AudioClip** para o campo `Test Clip`
3. **Execute a cena** e verifique os logs
4. **Pressione T** para testar áudio
5. **Pressione V** para verificar volume
6. **Pressione M** para testar com tom de teste

### Possíveis Problemas

#### 1. AudioListener Desabilitado
```
Solução: Habilitar AudioListener na Main Camera
```

#### 2. Volume Zerado
```
Solução: Verificar AudioSource.volume e AudioListener.volume
```

#### 3. Configuração Incorreta
```
Solução: Verificar Project Settings → Audio
```

#### 4. Driver de Áudio
```
Solução: Atualizar drivers de áudio do sistema
```

### Teste Manual

1. **Crie um GameObject vazio**
2. **Adicione AudioSource**
3. **Configure um AudioClip**
4. **Teste com Play()**

### Comandos de Teste

```csharp
// Teste básico
AudioSource.PlayOneShot(audioClip);

// Teste com volume
audioSource.volume = 1f;
audioSource.Play();

// Teste com tom
AudioClip.Create("Test", 44100, 1, 44100, false);
```

### Próximos Passos

1. **Execute AudioDiagnostic**
2. **Verifique os logs**
3. **Teste com tom de teste (M)**
4. **Verifique configurações do sistema** 