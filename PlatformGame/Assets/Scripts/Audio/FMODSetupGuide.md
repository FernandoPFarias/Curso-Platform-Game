# Guia de Configuração do FMOD

## Problema Identificado
O áudio não está funcionando porque **não há arquivos .bank** do FMOD no projeto.

## Solução Passo a Passo

### 1. Instalar FMOD Studio
- Baixe o FMOD Studio em: https://www.fmod.com/download
- Instale o FMOD Studio

### 2. Criar um Projeto FMOD
1. Abra o FMOD Studio
2. Crie um novo projeto
3. Adicione eventos de áudio (música, SFX)
4. Configure os eventos conforme necessário

### 3. Gerar Arquivos .bank
1. No FMOD Studio, vá em **Build** → **Build All Platforms**
2. Isso vai gerar arquivos `.bank` e `.strings`

### 4. Copiar Arquivos para Unity
1. Copie os arquivos `.bank` e `.strings` para:
   ```
   PlatformGame/Assets/Plugins/FMOD/
   ```

### 5. Configurar no Unity
1. Abra o Unity
2. Vá em **Window** → **FMOD** → **Settings**
3. Configure o caminho dos arquivos .bank

### 6. Testar
1. Adicione o script `AudioTest.cs` a um GameObject
2. Arraste um evento do FMOD para o campo `Test Event`
3. Execute a cena e pressione ESPAÇO para testar

## Estrutura de Arquivos Necessária

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

## Scripts de Teste

### AudioTest.cs
- Testa se o FMOD está funcionando
- Verifica se os eventos estão configurados
- Permite testar áudio manualmente

### FMODAudioManager.cs
- Gerenciador principal de áudio
- Contém referências para música e SFX
- Métodos para tocar áudio

## Próximos Passos

1. **Instale o FMOD Studio**
2. **Crie eventos de áudio**
3. **Gere os arquivos .bank**
4. **Copie para o Unity**
5. **Teste com AudioTest.cs**

## Verificações

- ✅ FMOD Unity Plugin instalado
- ❌ Arquivos .bank não encontrados
- ❌ Eventos não configurados
- ❌ Áudio não funciona

## Comandos Úteis

Para verificar se há arquivos .bank:
```bash
find PlatformGame/Assets -name "*.bank" -type f
```

Para verificar se o FMOD está funcionando:
- Execute a cena com `AudioTest.cs`
- Verifique os logs no Console
- Pressione ESPAÇO para testar 