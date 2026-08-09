using UnityEngine;
using System.Collections.Generic;

public class CarbonCore : MonoBehaviour
{
    [Header("Core Setting")]
    public CoreType coreType;

    public int maxTrash = 8;
    public float spawnInterval = 5f;
    public GameObject trashPrefab;
    private bool hasFailedCombo = false;

    [Header("HP System")]
    public int maxHP = 100;
    private float currentHP;

    [Header("Regen")]
    public float regenRate = 1f;


    private int currentTrash = 0;
    private float timer;

    private List<GameObject> spawnedTrash = new List<GameObject>();

    public enum CoreType
    {
        Weak,
        Strong
    }

    void Start()
    {
        // 🔥 ตั้งค่าตามประเภท Core
        if (coreType == CoreType.Weak)
        {
            maxTrash = 8;
            spawnInterval = 5f;
            maxHP = 50;
        }
        else if (coreType == CoreType.Strong)
        {
            maxTrash = 12;
            spawnInterval = 3f;
            maxHP = 100;
        }

        currentHP = maxHP;
    }

    void Update()
    {
        SpawnTrashLoop();
        RegenHP();
    }

    // =============================
    // 🗑️ Spawn ขยะ
    // =============================
    void SpawnTrashLoop()
    {
        timer += Time.deltaTime;

        if (timer >= spawnInterval)
        {
            timer = 0;

            if (currentTrash < maxTrash)
            {
                SpawnTrash();
            }
        }
    }

    void SpawnTrash()
    {
        float radius = 2f;

        Vector2 offset = Random.insideUnitCircle * radius;

        Vector2 pos = (Vector2)transform.position + offset;

        // 🔥 กันหลุดแมพ (Clamp)
        pos.x = Mathf.Clamp(pos.x, -10f, 10f);
        pos.y = Mathf.Clamp(pos.y, -10f, 10f);

        GameObject trash = Instantiate(trashPrefab, pos, Quaternion.identity);

        spawnedTrash.Add(trash);

        currentTrash++;
    }

    // =============================
    // 🎮 Interact
    // =============================
    public void Interact()
    {
        Debug.Log("เริ่มสู้ Core");

        TrashMiniGameManager.Instance.StartCoreGame(this);
    }

    // =============================
    // 💚 Regen HP
    // =============================
    void RegenHP()
    {
        if (currentHP < maxHP)
        {
            currentHP += regenRate * Time.deltaTime;

            if (currentHP > maxHP)
                currentHP = maxHP;
        }
    }

    // =============================
    // 🟢 Slider Damage (ฆ่าไม่ได้)
    // =============================
    public void TakeSliderDamage(int damage)
    {
        currentHP -= damage;

        // ❗ ห้ามฆ่า Core ด้วย slider
        if (currentHP <= 1)
        {
            currentHP = 1;
        }

        Debug.Log("โดน Slider | HP: " + Mathf.RoundToInt(currentHP));
    }

    // 🔴 Combo 
    public void OnComboSuccess()
    {
        Debug.Log("💥 Combo สำเร็จ! Core แตก");

        DestroyCore();
    }

    // ⚠️ Combo Fail
    public void OnComboFail()
    {
        hasFailedCombo = true;

        Debug.Log("⚠️ Core: เคยพลาด Combo");
    }

    // Core แตก
    void DestroyCore()
    {
        float remainPercent = 0f;

        if (hasFailedCombo)
        {
            if (coreType == CoreType.Weak)
                remainPercent = 0.4f; // เหลือ 40%
            else
                remainPercent = 0.5f; // เหลือ 50%
        }

        int remainCount = Mathf.RoundToInt(spawnedTrash.Count * remainPercent);

        // 🔥 สุ่มลบ
        for (int i = spawnedTrash.Count - 1; i >= 0; i--)
        {
            if (spawnedTrash[i] != null)
            {
                if (i >= remainCount)
                {
                    Destroy(spawnedTrash[i]);
                }
            }
        }

        TrashCoreManager.Instance.RemoveCore();
        Destroy(gameObject);
    }

    public void TakeSliderDamage()
    {
        currentHP -= 5;

        CancelInvoke();
        Invoke(nameof(RegenFull), 1f); // 🔥 1 วิ
    }

    void RegenFull()
    {
        currentHP = maxHP;
    }
}