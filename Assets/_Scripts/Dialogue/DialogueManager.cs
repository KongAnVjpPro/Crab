using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class DialogueManager : UIComponent
{
    public Image characterIcon;
    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
    public CanvasGroup canvasGroup;


    public Queue<DialogueLine> lines = new Queue<DialogueLine>();
    public bool isDialogueActive = false;
    public float typingSpeed = 0.2f;

    [Header("Pivot Animation")]
    public RectTransform outPos;
    public RectTransform inPos;
    public RectTransform exactPos;
    Sequence s;
    [SerializeField] bool isSkip = false;
    [SerializeField] bool isTyping = false;
    void Update()
    {
        isSkip = Input.GetButtonDown("Interact") || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter);
        if (isSkip && isDialogueActive)
        {
            if (isTyping)
            {

                isSkip = true;
            }
            else
            {

                DisplayNextDialogueLine();
            }
        }
    }
    void ShowDialogue()
    {
        if (this.s != null && this.s.IsActive())
        {
            this.s.Kill();
        }
        s = DOTween.Sequence();
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        s.Join(canvasGroup.DOFade(1, 1));
        s.Join(canvasGroup.GetComponent<RectTransform>().DOAnchorPos(inPos.anchoredPosition, 0.5f));
        s.Join(canvasGroup.GetComponent<RectTransform>().DOAnchorPos(exactPos.anchoredPosition, 0.5f));
    }
    void HideDialogue()
    {
        if (this.s != null && this.s.IsActive())
        {
            this.s.Kill();
        }
        s = DOTween.Sequence();
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        s.Join(canvasGroup.DOFade(0, 1));
        s.Join(canvasGroup.GetComponent<RectTransform>().DOAnchorPos(outPos.anchoredPosition, 1f)).SetEase(Ease.OutQuad);
        // seq.Join(canvasGroup.GetComponent<RectTransform>().DOAnchorPos(exactPos.anchoredPosition,0.5f));
    }
    public void StartDialogue(Dialogue dialogue)
    {
        isDialogueActive = true;
        GameController.Instance.isBlockPlayerControl = true;

        ShowDialogue();
        lines.Clear();
        foreach (DialogueLine dialogueLine in dialogue.dialogueLines)
        {
            lines.Enqueue(dialogueLine);
        }
        DisplayNextDialogueLine();
    }
    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
        DialogueLine currentLine = lines.Dequeue();
        // characterIcon.sprite = currentLine.character.icon;
        characterName.text = currentLine.character.name;
        currentLine.ActionWhileTalking?.Invoke();

        StopAllCoroutines();
        StartCoroutine(TypeSentence(currentLine));
    }
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        isTyping = true;
        isSkip = false;
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            if (isSkip)
            {
                dialogueArea.text = dialogueLine.line;
                // break;
                isSkip = false;
                break;
            }
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }
    void EndDialogue()
    {
        GameController.Instance.isBlockPlayerControl = false;

        isDialogueActive = false;
        HideDialogue();
    }
}