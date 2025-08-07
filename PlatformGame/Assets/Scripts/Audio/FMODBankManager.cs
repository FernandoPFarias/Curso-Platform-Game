using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using System.Collections.Generic;

public class FMODBankManager : MonoBehaviour
{
    [Header("Banks para Carregar")]
    public string[] bankNames = { "Master", "Music", "SFX" };
    
    private List<Bank> loadedBanks = new List<Bank>();
    
    void Start()
    {
        Debug.Log("=== CARREGAMENTO MANUAL DE BANKS ===");
        LoadAllBanks();
    }
    
    void LoadAllBanks()
    {
        foreach (string bankName in bankNames)
        {
            LoadBank(bankName);
        }
        
        // Verifica se os eventos estão disponíveis após carregar os banks
        CheckEventAvailability();
    }
    
    void LoadBank(string bankName)
    {
        try
        {
            Debug.Log($"Tentando carregar bank: {bankName}");
            
            // Carrega o bank
            Bank bank;
            FMOD.RESULT result = RuntimeManager.StudioSystem.loadBankFile(bankName + ".bank", LOAD_BANK_FLAGS.NORMAL, out bank);
            
            if (result == FMOD.RESULT.OK)
            {
                loadedBanks.Add(bank);
                Debug.Log($"✅ Bank {bankName} carregado com sucesso");
            }
            else
            {
                Debug.LogError($"❌ Erro ao carregar bank {bankName}: {result}");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Exceção ao carregar bank {bankName}: {e.Message}");
        }
    }
    
    void CheckEventAvailability()
    {
        Debug.Log("=== VERIFICANDO DISPONIBILIDADE DE EVENTOS ===");
        
        // Lista de eventos para testar
        string[] eventsToTest = {
            "event:/Musica_Exploração",
            "event:/Musica_Santuario 2"
        };
        
        foreach (string eventPath in eventsToTest)
        {
            try
            {
                EventDescription eventDescription;
                FMOD.RESULT result = RuntimeManager.StudioSystem.getEvent(eventPath, out eventDescription);
                
                if (result == FMOD.RESULT.OK)
                {
                    Debug.Log($"✅ Evento {eventPath} está disponível");
                }
                else
                {
                    Debug.LogError($"❌ Evento {eventPath} não encontrado: {result}");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"❌ Erro ao verificar evento {eventPath}: {e.Message}");
            }
        }
    }
    
    void Update()
    {
        // Pressione L para recarregar banks
        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Recarregando banks...");
            UnloadAllBanks();
            LoadAllBanks();
        }
        
        // Pressione T para testar evento
        if (Input.GetKeyDown(KeyCode.T))
        {
            TestEvent("event:/Musica_Exploração");
        }
    }
    
    void TestEvent(string eventPath)
    {
        try
        {
            Debug.Log($"Testando evento: {eventPath}");
            RuntimeManager.PlayOneShot(eventPath);
            Debug.Log("✅ Evento reproduzido com sucesso!");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"❌ Erro ao reproduzir evento: {e.Message}");
        }
    }
    
    void UnloadAllBanks()
    {
        foreach (Bank bank in loadedBanks)
        {
            if (bank.isValid())
            {
                bank.unload();
            }
        }
        loadedBanks.Clear();
        Debug.Log("Banks descarregados");
    }
    
    void OnDestroy()
    {
        UnloadAllBanks();
    }
} 