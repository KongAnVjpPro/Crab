using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class DialogueCharacter
{
    public string name;
    public Sprite icon;
}
[System.Serializable]
public class DialogueLine
{
    public DialogueCharacter character;
    [TextArea(3, 10)]
    public string line;
}
[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}
public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;
    [SerializeField] bool interact = false;
    public void TriggerDialogue()
    {
        UIEntity.Instance.dialogueManager.StartDialogue(dialogue);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {

            interact = true;
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            interact = false;
        }
    }
    void Update()
    {
        if (interact)
        {
            if (!PlayerEntity.Instance.playerInput.interact) return;
            if (UIEntity.Instance.dialogueManager.isDialogueActive)
                return;
            TriggerDialogue();
        }
    }
}