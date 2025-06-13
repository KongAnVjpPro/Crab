using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using DG.Tweening;
using UnityEngine;
public class Elevator : MyMonobehaviour

{
    [SerializeField] GameObject endPosPivot;
    [SerializeField] GameObject startPosPivot;
    [SerializeField] Transform elevator;
    [SerializeField] bool isActivate = false;
    [SerializeField] float elevateTime = 5f;
    [SerializeField] bool isUp = true;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] List<Animator> levelAnim;
    [SerializeField] GameObject shield;
    public void StartElevator()
    {
        shield.SetActive(true);
        if (isActivate) return;
        impulseSource?.GenerateImpulse();
        isActivate = true;
        // levelAnim?.SetBool("IsOpened", isUp);
        if (levelAnim.Count > 0)
        {
            foreach (var level in levelAnim)
            {
                level.SetBool("IsOpened", isUp);
            }
        }
        elevator.DOMove(isUp ? endPosPivot.transform.position : startPosPivot.transform.position, elevateTime).OnComplete(() =>
        {
            isActivate = false;
            isUp = !isUp;
            shield.SetActive(false);
        });
    }

}