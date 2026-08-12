using UnityEngine;
using System.Collections.Generic;
using System.Collections;

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
    // 🟢 Slider Damage
    // =============================
    public void TakeSliderDamage(int damage)
    {
        currentHP -= damage;

        if (currentHP <= 1)
            currentHP = 1;

        CancelInvoke();
        Invoke(nameof(RegenFull), 1f);

        Debug.Log("โดน Slider | HP: " + Mathf.RoundToInt(currentHP));
    }

    void RegenFull()
    {
        currentHP = maxHP;
    }

    public void OnComboSuccess()
    {
        Debug.Log("💥 Combo สำเร็จ! Core แตก");
        DestroyCore();
    }

    public void OnComboFail()
    {
        hasFailedCombo = true;
        Debug.Log("⚠️ Core: เคยพลาด Combo");
    }

    void DestroyCore()
    {
        float remainPercent = 0f;

        if (hasFailedCombo)
        {
            if (coreType == CoreType.Weak)
                remainPercent = 0.4f;
            else
                remainPercent = 0.5f;
        }

        List<GameObject> aliveTrash = new List<GameObject>();

        foreach (var t in spawnedTrash)
        {
            if (t != null)
            {
                Trash trashComp = t.GetComponent<Trash>();

                if (trashComp != null && !trashComp.isCollected)
                {
                   aliveTrash.Add(t);
                }
            }
        }

        int remainCount = Mathf.RoundToInt(aliveTrash.Count * remainPercent);

        for (int i = 0; i < aliveTrash.Count; i++)
        {
            if (i >= remainCount)
            {
                Destroy(aliveTrash[i]);
            }
        }

        TrashCoreManager.Instance.RemoveCore();

        Destroy(gameObject);

        TrashCoreManager.Instance.CheckEndGameSafe();
    }

    IEnumerator CheckAfterFrame()
    {
        yield return null; // รอ 1 frame

        TrashCoreManager.Instance.CheckEndGame();
    }
}