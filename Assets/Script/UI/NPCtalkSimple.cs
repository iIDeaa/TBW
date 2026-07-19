using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class NPCtalkSimple : MonoBehaviour
{
    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text nameText;
    public TMP_Text dialogueText;
    public GameObject prompt;


    [Header("NPC")]
    public string npcName;


    [Header("Dialogue")]
    public DialogueData defaultDialogue;


    [Header("Portrait")]
    public Image portraitImage;


    [Header("Tell Image")]
    public Image tellImage;


    [Header("Typing")]
    public float typingSpeed = 0.04f;
    [Header("Minigame")]
    [SerializeField] private string minigameSceneName;


    private bool playerInRange;
    private bool isTalking;
    private bool isTyping;

    private int currentLine;

    private DialogueData.Line[] currentLines;



    void Start()
    {
        dialoguePanel.SetActive(false);

        if(prompt != null)
            prompt.SetActive(false);
    }



    void Update()
    {
        if(!playerInRange)
            return;


        if(Input.GetKeyDown(KeyCode.E))
        {
            if(!isTalking)
                StartDialogue();
            else
                NextLine();
        }
    }



    void StartDialogue()
    {
        if(defaultDialogue == null)
        {
            Debug.LogWarning("No dialogue assigned");
            return;
        }


        isTalking = true;
        currentLine = 0;


        dialoguePanel.SetActive(true);


        PlayerMove player = FindObjectOfType<PlayerMove>();

        if(player != null)
            player.canMove = false;



        currentLines = defaultDialogue.lines;


        StartCoroutine(TypeLine());
    }



    IEnumerator TypeLine()
    {
        isTyping = true;

        dialogueText.text = "";


        DialogueData.Line line = currentLines[currentLine];


        // ชื่อคนพูด
        if(nameText != null)
            nameText.text = line.speaker;



        // Portrait
        if(portraitImage != null)
        {
            portraitImage.sprite = line.portrait;
            portraitImage.gameObject.SetActive(line.portrait != null);
        }



        // Tell Image
        if(tellImage != null)
        {
            tellImage.sprite = line.tellimage;

            tellImage.gameObject.SetActive(
                line.tellimage != null
            );
        }



        foreach(char c in line.text)
        {
            dialogueText.text += c;

            yield return new WaitForSeconds(typingSpeed);
        }


        isTyping = false;
    }



    void NextLine()
    {
        if(isTyping)
        {
            StopAllCoroutines();

            dialogueText.text =
                currentLines[currentLine].text;

            isTyping = false;

            return;
        }


        currentLine++;


        if(currentLine < currentLines.Length)
        {
            StartCoroutine(TypeLine());
        }
        else
        {
            EndDialogue();
        }
    }



        void EndDialogue()
    {
        isTalking = false;

        dialoguePanel.SetActive(false);

        PlayerMove player = FindObjectOfType<PlayerMove>();

        if (player != null)
            player.canMove = true;

        // ถ้ามี Minigame
        if (!string.IsNullOrWhiteSpace(minigameSceneName))
        {
            MinigameManager.Instance.StartMinigame(minigameSceneName);
        }
    }
    IEnumerator LoadMinigame()
    {
        yield return new WaitForSeconds(0.2f);

        SceneManager.LoadScene(minigameSceneName);
    }



    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;


            if(prompt != null)
                prompt.SetActive(true);
        }
    }



    void OnTriggerExit2D(Collider2D other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;


            if(isTalking)
                EndDialogue();


            if(prompt != null)
                prompt.SetActive(false);
        }
    }
}