# Sistema de Câmera para Jogo de Plataforma 2D

Este sistema fornece uma configuração completa e automatizada da câmera Cinemachine para jogos de plataforma 2D.

## 📋 Pré-requisitos

1. **Unity 2021.3 ou superior**
2. **Pacote Cinemachine instalado**
   - Vá em `Window > Package Manager`
   - Procure por "Cinemachine" e instale

## 🚀 Configuração Rápida

### Método 1: Usando o Wizard (Recomendado)

1. Abra o Unity Editor
2. Vá em `Tools > Camera Setup Wizard`
3. Configure as opções desejadas
4. Clique em "Configurações Padrão para Plataforma" para usar valores otimizados
5. Clique em "Configurar Câmera"

### Método 2: Script Manual

1. Crie um GameObject vazio na cena
2. Adicione o componente `CameraSetup`
3. Configure as referências:
   - **Player Target**: Arraste o objeto do player
   - **Level Bounds**: Arraste um collider que delimite o nível (opcional)
4. Clique no botão "Setup Camera" no Inspector

## ⚙️ Configurações

### Dead Zone (Zona Morta)
- **Dead Zone Width/Height**: Área central onde o player pode se mover sem a câmera seguir
- **Valores recomendados**: 0.3 - 0.5
- **Menor valor**: Câmera mais responsiva
- **Maior valor**: Movimento mais suave

### Soft Zone (Zona Suave)
- **Soft Zone Width/Height**: Área de transição antes da câmera seguir totalmente
- **Valores recomendados**: 0.7 - 0.9
- **Menor valor**: Transição mais abrupta
- **Maior valor**: Transição mais suave

### Damping (Amortecimento)
- **Damping X/Y**: Velocidade de resposta da câmera
- **Valores recomendados**: 1.0 - 2.0
- **Menor valor**: Câmera mais rápida
- **Maior valor**: Câmera mais lenta

### Orthographic Size
- **Tamanho da câmera**: Quanto do cenário aparece na tela
- **Valores recomendados**: 4.0 - 6.0
- **Menor valor**: Zoom in (mais próximo)
- **Maior valor**: Zoom out (mais distante)

## 🎮 Efeitos de Câmera

### Shake (Tremor)
```csharp
// Encontrar o componente
CameraEffects cameraEffects = FindObjectOfType<CameraEffects>();

// Shake básico
cameraEffects.ShakeCamera();

// Shake personalizado
cameraEffects.ShakeCamera(1.5f, 0.5f);

// Shake para eventos específicos
cameraEffects.ShakeForDamage();
cameraEffects.ShakeForExplosion();
cameraEffects.ShakeForLanding();
```

### Zoom
```csharp
// Zoom básico
cameraEffects.ZoomIn();
cameraEffects.ZoomOut();

// Zoom personalizado
cameraEffects.ZoomTo(3.5f);

// Zoom para situações específicas
cameraEffects.ZoomForCombat();
cameraEffects.ZoomForExploration();
cameraEffects.ResetZoom();
```

### Flash Screen
```csharp
// Flash vermelho para dano
cameraEffects.FlashScreen(Color.red, 0.3f);

// Flash branco para explosão
cameraEffects.FlashScreen(Color.white, 0.5f);
```

## 🔧 Configurações Avançadas

### Screen Confiner
- **Use Screen Confiner**: Limita a câmera aos limites da tela
- **Screen Edge Buffer**: Margem adicional nas bordas da tela

### Level Bounds
- Crie um objeto com `Collider2D` (preferencialmente `CompositeCollider2D`)
- Configure o collider para delimitar a área jogável
- Arraste para o campo "Level Bounds" no CameraSetup

## 📁 Estrutura dos Scripts

```
Assets/Scripts/Game/
├── CameraSetup.cs          # Script principal de configuração
├── CameraSetupWizard.cs    # Interface do editor para configuração
├── CameraEffects.cs        # Efeitos de câmera (shake, zoom, flash)
└── README_CameraSetup.md   # Este arquivo
```

## 🎯 Exemplos de Uso

### Integração com Player
```csharp
public class PlayerController : MonoBehaviour
{
    private CameraEffects cameraEffects;
    
    void Start()
    {
        cameraEffects = FindObjectOfType<CameraEffects>();
    }
    
    void OnDamage()
    {
        cameraEffects.ShakeForDamage();
        cameraEffects.FlashScreen(Color.red, 0.2f);
    }
    
    void OnLanding()
    {
        cameraEffects.ShakeForLanding();
    }
}
```

### Integração com Inimigos
```csharp
public class Enemy : MonoBehaviour
{
    private CameraEffects cameraEffects;
    
    void Start()
    {
        cameraEffects = FindObjectOfType<CameraEffects>();
    }
    
    void OnExplosion()
    {
        cameraEffects.ShakeForExplosion();
        cameraEffects.FlashScreen(Color.white, 0.3f);
    }
}
```

## 🐛 Solução de Problemas

### Câmera não segue o player
- Verifique se o player tem a tag "Player"
- Confirme se o Cinemachine está instalado
- Verifique se a Virtual Camera está ativa

### Shake não funciona
- Verifique se o CinemachineImpulseSource está presente
- Confirme se a Virtual Camera tem CinemachineImpulseListener

### Zoom não funciona
- Verifique se a Virtual Camera foi encontrada
- Confirme se o Orthographic Size está sendo alterado

## 📞 Suporte

Para dúvidas ou problemas:
1. Verifique se o Cinemachine está instalado
2. Confirme se todos os scripts estão na pasta correta
3. Verifique o Console do Unity para erros
4. Teste com as configurações padrão primeiro

## 🔄 Atualizações

- **v1.0**: Configuração básica da câmera
- **v1.1**: Adicionado sistema de efeitos
- **v1.2**: Adicionado wizard de configuração
- **v1.3**: Melhorias na documentação e exemplos 