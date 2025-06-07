using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
public class DoorVisual : MyMonobehaviour
{
    // public enum DoorType
    // {
    //     InScene = 0,
    //     AnotherScene = 1,
    // }
    [SerializeField] Animator anim;
    [SerializeField] Collider2D doorCollider;
    [SerializeField] string doorKey = "";
    [SerializeField] bool isOpened = false;
    [SerializeField] ParticleSystem particle;

    // [SerializeField] DoorType doorType;
    public void OpenDoor()
    {
        if (anim == null) return;
        anim.SetTrigger("Open");
        // doorCollider.enabled = false;
        StartCoroutine(WaitForDoorAnimationEnd(true));
        particle?.Play();
    }
    public void CloseDoor()
    {
        if (anim == null) return;
        anim.SetTrigger("Close");
        // AnimatorClipInfo clipInfo = anim.GetCurrentAnimatorClipInfo(0)[0];
        StartCoroutine(WaitForDoorAnimationEnd(false));
        // doorCollider.enabled = true;
        particle?.Play();
    }
    IEnumerator WaitForDoorAnimationEnd(bool isOpen)
    {
        if (anim == null) yield break;
        AnimatorClipInfo clipInfo = anim.GetCurrentAnimatorClipInfo(0)[0];
        float animationLength = clipInfo.clip.length;
        yield return new WaitForSeconds(animationLength);
        doorCollider.enabled = !isOpen;
        particle?.Stop();
    }
    public void OnDoorStateChanged(string key, bool state)
    {
        if (key != doorKey) return;
        if (state)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    void GetDoorState()
    {
        isOpened = SaveSystem.Instance.GetDoorState(doorKey);
    }
    void Start()
    {
        SaveSystem.Instance.OnDoorDataChanged += OnDoorStateChanged;
        GetDoorState();
        if (isOpened)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    protected override void Awake()
    {
        base.Awake();



    }
    private void OnDestroy()
    {
        if (SaveSystem.Instance != null)
            SaveSystem.Instance.OnDoorDataChanged -= OnDoorStateChanged;
    }
}