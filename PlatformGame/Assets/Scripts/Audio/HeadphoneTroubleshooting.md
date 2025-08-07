# Troubleshooting de Fone de Ouvido

## Problema: Unity não reconhece o fone de ouvido

### Verificações Rápidas

#### 1. Configurações do Windows
1. **Clique com botão direito** no ícone de som (canto inferior direito)
2. **Vá em "Configurações de Som"**
3. **Em "Som" → "Dispositivos de saída"**
4. **Verifique se seu fone está listado e selecionado**
5. **Clique em "Testar"** para verificar se funciona

#### 2. Volume do Sistema
- **Verifique o volume do Windows** (ícone de alto-falante)
- **Verifique se não está no mudo**
- **Teste com outros programas** (YouTube, Spotify, Windows Media Player)

#### 3. Conexão do Fone
- **Verifique se está conectado corretamente**
- **Teste em outras portas** (se for USB)
- **Verifique se não está quebrado** (teste em outros dispositivos)

#### 4. Drivers de Áudio
- **Atualize os drivers de áudio**
- **Reinicie o computador** após conectar o fone
- **Verifique se o Windows reconhece o dispositivo**

### Teste com AudioDeviceChecker

1. **Adicione o script `AudioDeviceChecker.cs`** a um GameObject
2. **Execute a cena** e verifique os logs
3. **Pressione T** para testar tom de teste
4. **Pressione V** para verificar volume
5. **Pressione D** para ver informações de dispositivo

### Problemas Comuns

#### 1. Fone não selecionado como padrão
```
Solução: 
1. Configurações de Som → Dispositivos de saída
2. Selecione seu fone como padrão
3. Clique em "Definir como padrão"
```

#### 2. Unity usando dispositivo errado
```
Solução:
1. Verifique se o Windows está usando o fone
2. Reinicie o Unity
3. Verifique Project Settings → Audio
```

#### 3. Driver de áudio desatualizado
```
Solução:
1. Atualize drivers de áudio
2. Reinstale drivers se necessário
3. Reinicie o computador
```

#### 4. Fone com problema
```
Solução:
1. Teste em outros dispositivos
2. Teste outros fones no computador
3. Verifique se não está quebrado
```

### Teste Manual

#### 1. Teste no Windows
1. **Conecte o fone**
2. **Vá em Configurações de Som**
3. **Clique em "Testar"** no seu fone
4. **Deve ouvir um som de teste**

#### 2. Teste no Unity
1. **Execute AudioDeviceChecker**
2. **Pressione T** para testar tom
3. **Deve ouvir um beep de 440Hz**

#### 3. Teste com outros programas
1. **Abra YouTube**
2. **Tente reproduzir um vídeo**
3. **Verifique se o som sai pelo fone**

### Configurações Recomendadas

#### Windows:
```
Configurações de Som:
- Dispositivo de saída: Seu fone
- Volume: > 50%
- Não mudo
```

#### Unity:
```
Project Settings → Audio:
- Disable Unity Audio: ❌ (desmarcado)
- System Sample Rate: 48000 Hz
- DSP Buffer Size: Best Performance
```

### Próximos Passos

1. **Teste o fone no Windows primeiro**
2. **Execute AudioDeviceChecker**
3. **Verifique configurações do sistema**
4. **Teste com tom de teste (T)**
5. **Se não funcionar, teste outros dispositivos**

### Comandos de Teste

```csharp
// Teste básico
AudioSource.PlayOneShot(audioClip);

// Teste com tom
AudioClip.Create("Test", 44100, 1, 44100, false);
```

### Verificação Final

Após todas as verificações:
- ✅ Fone funciona no Windows
- ✅ Unity reconhece o dispositivo
- ✅ Tom de teste funciona
- ✅ Áudio do jogo funciona 