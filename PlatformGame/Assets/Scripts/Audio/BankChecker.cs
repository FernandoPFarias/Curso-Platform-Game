using UnityEngine;
using System.IO;

public class BankChecker : MonoBehaviour
{
    void Start()
    {
        Debug.Log("=== VERIFICAÇÃO DE ARQUIVOS .BANK ===");
        
        // Verifica se há arquivos .bank na pasta FMOD
        string fmodPath = "Assets/Plugins/FMOD/";
        if (Directory.Exists(fmodPath))
        {
            string[] bankFiles = Directory.GetFiles(fmodPath, "*.bank", SearchOption.AllDirectories);
            string[] stringsFiles = Directory.GetFiles(fmodPath, "*.strings", SearchOption.AllDirectories);
            
            Debug.Log($"Arquivos .bank encontrados: {bankFiles.Length}");
            Debug.Log($"Arquivos .strings encontrados: {stringsFiles.Length}");
            
            if (bankFiles.Length == 0)
            {
                Debug.LogError("❌ NENHUM ARQUIVO .BANK ENCONTRADO!");
                Debug.LogError("Você precisa gerar os arquivos .bank no FMOD Studio");
                Debug.LogError("1. Abra o FMOD Studio");
                Debug.LogError("2. Vá em Build → Build All Platforms");
                Debug.LogError("3. Copie os arquivos .bank para Assets/Plugins/FMOD/");
            }
            else
            {
                Debug.Log("✅ Arquivos .bank encontrados:");
                foreach (string file in bankFiles)
                {
                    Debug.Log($"  - {Path.GetFileName(file)}");
                }
            }
        }
        else
        {
            Debug.LogError($"❌ Pasta FMOD não encontrada: {fmodPath}");
        }
        
        // Verifica se há arquivos .bank em outras pastas
        string[] allBankFiles = Directory.GetFiles("Assets", "*.bank", SearchOption.AllDirectories);
        Debug.Log($"Total de arquivos .bank no projeto: {allBankFiles.Length}");
    }
} 