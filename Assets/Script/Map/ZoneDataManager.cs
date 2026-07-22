using System.Collections.Generic;
using UnityEngine;

public class ZoneDataManager : MonoBehaviour
{
    public static ZoneDataManager Instance;

    [Header("Current Zone")]
    public int currentZone = 0;

    [Header("Progress Zone")]
    public float[] zoneProgress = { 1f, 1f, 1f, 1f };

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetCurrentZone(int zoneIndex)
    {
        currentZone = zoneIndex;
    }

    public float GetCurrentProgress()
    {
        return zoneProgress[currentZone];
    }

    public void DecreaseProgress(float amount)
    {
        zoneProgress[currentZone] -= amount;
        zoneProgress[currentZone] = Mathf.Clamp01(zoneProgress[currentZone]);
    }


    public List<float> GetAllProgress()
    {
        return new List<float>(zoneProgress);
    }

  
    public void LoadProgress(List<float> data)
    {
        if (data == null || data.Count == 0) return;

        zoneProgress = data.ToArray();
    }
    public void ResetAll()
    {
        for (int i = 0; i < zoneProgress.Length; i++)
        {
            zoneProgress[i] = 1f; 
        }

        currentZone = 0;
    }
}