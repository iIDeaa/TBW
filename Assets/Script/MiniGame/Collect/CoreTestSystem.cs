using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A self-contained Core gameplay prototype for TestTrashCollect.
/// It deliberately leaves the existing trash collection scripts untouched.
/// Remove this file when the prototype is replaced by the production system.
/// </summary>
public sealed class CoreTestSystem : MonoBehaviour
{
    private enum Phase { Exploring, Question, Counter, Resolved }

    private static readonly KeyCode[] Combo =
    {
        KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.Space
    };

    private readonly List<Trash> knownTrash = new List<Trash>();
    private TrashSpawner spawner;
    private TrashCoreManager legacyCoreManager;
    private Transform player;
    private GameObject activeCore;
    private Phase phase;
    private int collected;
    private float chance;
    private bool strongCore;
    private int comboIndex;
    private int normalHits;
    private int ignoreTrashChangesThroughFrame;
    private float comboDeadline;
    private string status = "Collect trash. Carbon Core may appear as trash accumulates.";

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
    private static void CreateForTestScene()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name != "TestTrashCollect")
            return;

        new GameObject("Core Test System (Temporary)").AddComponent<CoreTestSystem>();
    }

    private void Start()
    {
        spawner = FindObjectOfType<TrashSpawner>();
        legacyCoreManager = FindObjectOfType<TrashCoreManager>();
        if (legacyCoreManager != null)
            legacyCoreManager.enabled = false;

        GameObject playerObject = GameObject.FindGameObjectWithTag("Player");
        if (playerObject == null)
            playerObject = FindObjectOfType<PlayerInteract>()?.gameObject;
        player = playerObject != null ? playerObject.transform : null;

        RefreshKnownTrash();
        status = "Core test active: collect trash using the existing E + Space minigame.";
    }

    private void Update()
    {
        // The existing collection minigame calls its legacy manager directly. Start it at -1:
        // its two old AddTrash calls then reach only 0 and 1, never its first threshold (2).
        // Keep chance at zero so the Inspector does not display a misleading negative chance.
        if (legacyCoreManager != null)
        {
            legacyCoreManager.collectedTrash = -1;
            legacyCoreManager.coreChance = 0f;
        }

        if (phase == Phase.Exploring)
        {
            DetectCollectedTrash();
            TryStartCoreInteraction();
        }
        else if (phase == Phase.Question)
        {
            // Temporary keyboard substitute for Speech-to-Text.
            if (Input.GetKeyDown(KeyCode.Y)) StartCounter(true);
            if (Input.GetKeyDown(KeyCode.N)) StartCounter(false);
        }
        else if (phase == Phase.Counter)
        {
            UpdateCounter();
        }
    }

    private void DetectCollectedTrash()
    {
        int before = knownTrash.Count;
        knownTrash.RemoveAll(trash => trash == null);
        int removed = before - knownTrash.Count;
        if (Time.frameCount > ignoreTrashChangesThroughFrame)
            for (int i = 0; i < removed; i++) RegisterCollectedTrash();

        // Trash is spawned by the existing test scene; include newly spawned objects too.
        Trash[] allTrash = FindObjectsOfType<Trash>();
        foreach (Trash trash in allTrash)
            if (!knownTrash.Contains(trash)) knownTrash.Add(trash);
    }

    private void RegisterCollectedTrash()
    {
        if (activeCore != null) return;

        collected++;
        if (collected == 2) chance += 10f;
        else if (collected == 5) chance += 20f;
        else if (collected == 8) chance += 20f;
        else if (collected == 12) chance += 20f;
        else if (collected >= 15) chance = 100f;

        if (Random.Range(0f, 100f) > chance) return;

        // Weak before/at 8 collected; Strong after that. This follows the test design.
        strongCore = collected > 8;
        SpawnCore();
        chance = 0f;
        collected = 0;
    }

    private void SpawnCore()
    {
        if (spawner == null || legacyCoreManager == null) return;
        GameObject prefab = strongCore ? legacyCoreManager.strongCorePrefab : legacyCoreManager.weakCorePrefab;
        if (prefab == null) return;

        Bounds bounds = spawner.GetComponent<BoxCollider2D>().bounds;
        Vector2 position = new Vector2(Random.Range(bounds.min.x, bounds.max.x), Random.Range(bounds.min.y, bounds.max.y));
        // Copy only the visual. Instantiating the prefab itself would also add CarbonCore,
        // which PlayerInteract would send into the unfinished legacy flow.
        SpriteRenderer prefabRenderer = prefab.GetComponent<SpriteRenderer>();
        activeCore = new GameObject(strongCore ? "Strong Core (Test)" : "Weak Core (Test)");
        activeCore.transform.position = position;
        SpriteRenderer renderer = activeCore.AddComponent<SpriteRenderer>();
        if (prefabRenderer != null)
        {
            renderer.sprite = prefabRenderer.sprite;
            renderer.color = prefabRenderer.color;
            renderer.sortingLayerID = prefabRenderer.sortingLayerID;
            renderer.sortingOrder = prefabRenderer.sortingOrder;
        }

        status = (strongCore ? "Strong" : "Weak") + " Carbon Core appeared! Move close and press E.";
    }

    private void TryStartCoreInteraction()
    {
        if (activeCore == null || player == null || !Input.GetKeyDown(KeyCode.E)) return;
        if (Vector2.Distance(player.position, activeCore.transform.position) > 1.5f) return;

        phase = Phase.Question;
        status = "Question: Does separating waste reduce environmental impact?  Y = yes, N = no";
    }

    private void StartCounter(bool answerCorrect)
    {
        phase = Phase.Counter;
        comboIndex = 0;
        normalHits = 0;
        comboDeadline = Time.time + (strongCore ? 2.5f : 4f);
        status = answerCorrect
            ? "Correct. Counter Phase: W A S D Space for Perfect, or Space 3 times for a normal clear."
            : "Incorrect. Counter Phase: W A S D Space for Perfect, or Space 3 times for an imperfect clear.";
    }

    private void UpdateCounter()
    {
        if (Time.time > comboDeadline)
        {
            Resolve(false, "Counter time expired: Core breaks imperfectly; some trash remains.");
            return;
        }

        if (Input.GetKeyDown(KeyCode.Space) && comboIndex == 0)
        {
            normalHits++;
            if (normalHits >= 3)
                Resolve(false, "Normal clear: Core breaks, but some trash remains.");
            return;
        }

        if (Input.GetKeyDown(Combo[comboIndex]))
        {
            comboIndex++;
            if (comboIndex == Combo.Length)
                Resolve(true, "PERFECT! Core destroyed and all remaining trash is cleared.");
        }
        else if (Input.anyKeyDown)
        {
            comboIndex = 0;
        }
    }

    private void Resolve(bool perfect, string result)
    {
        phase = Phase.Resolved;
        // Core cleanup is an outcome, not player collection; do not feed it back into Core accumulation.
        ignoreTrashChangesThroughFrame = Time.frameCount + 1;
        if (perfect)
        {
            foreach (Trash trash in FindObjectsOfType<Trash>())
                Destroy(trash.gameObject);
            ScoreManager.Instance?.AddScore(strongCore ? 200 : 100);
        }
        else
        {
            Trash[] trash = FindObjectsOfType<Trash>();
            for (int i = 0; i < trash.Length / 2; i++)
                Destroy(trash[i].gameObject);
            ScoreManager.Instance?.AddScore(strongCore ? 50 : 25);
        }

        if (activeCore != null) Destroy(activeCore);
        activeCore = null;
        RefreshKnownTrash();
        status = result + " Continue collecting trash.";
        phase = Phase.Exploring;
    }

    private void RefreshKnownTrash()
    {
        knownTrash.Clear();
        knownTrash.AddRange(FindObjectsOfType<Trash>());
    }

    private void OnGUI()
    {
        GUI.Box(new Rect(16, 16, 620, 74), "Carbon Core Test");
        GUI.Label(new Rect(28, 42, 590, 42), status);
        if (activeCore != null)
            GUI.Label(new Rect(28, 68, 590, 20), "Core type: " + (strongCore ? "Strong" : "Weak") + " | accumulated chance: " + chance.ToString("0") + "%");
    }
}
