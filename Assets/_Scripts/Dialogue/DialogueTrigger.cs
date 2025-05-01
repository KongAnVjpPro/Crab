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
    public void TriggerDialogue()
    {
        UIEntity.Instance.dialogueManager.StartDialogue(dialogue);
    }
    void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player" && PlayerEntity.Instance.playerInput.interact)
        {
            if (UIEntity.Instance.dialogueManager.isDialogueActive)
                return;
            TriggerDialogue();
        }
    }
}