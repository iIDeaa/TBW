using UnityEngine;
using System.Collections.Generic;

public class CarbonCore : MonoBehaviour
{
    [Header("Core Setting")]
    public CoreType coreType;

    public int maxTrash = 8;
    public float spawnInterval = 5f;
    public GameObject trashPrefab;

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
        Vector2 pos = (Vector2)transform.position + Random.insideUnitCircle * 2f;

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

        TrashMiniGameManager.Instance.StartTrashGame(this);
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

    // =============================
    // 🔴 Combo Success (ฆ่าจริง)
    // =============================
    public void OnComboSuccess()
    {
        Debug.Log("💥 Combo สำเร็จ! Core แตก");

        DestroyCore();
    }

    // =============================
    // ⚠️ Combo Fail
    // =============================
    public void OnComboFail()
    {
        Debug.Log("❌ Combo พลาด!");

        float damage = maxHP * 0.3f;
        currentHP -= damage;

        Debug.Log("โดน Combo Fail | HP: " + Mathf.RoundToInt(currentHP));

        if (currentHP <= 0)
        {
            DestroyCore();
        }
    }

    // =============================
    // 💥 ทำลาย Core
    // =============================
    public void DestroyCore()
    {
        Debug.Log("Core ถูกทำลาย!");

        // 🔥 ลบขยะทั้งหมดที่ Core สร้าง
        foreach (var trash in spawnedTrash)
        {
            if (trash != null)
            {
                Destroy(trash);
            }
        }

        Destroy(gameObject);
    }
}