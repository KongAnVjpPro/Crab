using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartController : MyMonobehaviour
{
    PlayerMovement playerMovement;
    private GameObject[] heartContainers;
    private Image[] heartFills;
    public Transform heartsParent;
    public GameObject heartContainerPrefab;
    void Start()
    {
        playerMovement = PlayerController.Instance.PlayerMovement;
        heartContainers = new GameObject[PlayerController.Instance.PlayerMovement.maxHealth];
        heartFills = new Image[PlayerController.Instance.PlayerMovement.maxHealth];

        PlayerController.Instance.PlayerMovement.onHealthChangedCallback += UpdateHeartsHUD;
        InstantiateHeartContainers();
        UpdateHeartsHUD();
    }
    void SetHeartContainers()
    {
        for (int i = 0; i < heartContainers.Length; i++)
        {
            if (i < PlayerController.Instance.PlayerMovement.maxHealth)
            {
                heartContainers[i].SetActive(true);
            }
            else
            {
                heartContainers[i].SetActive(false);
            }
        }
    }
    void SetFilledHearts()
    {
        for (int i = 0; i < heartFills.Length; i++)
        {
            if (i < PlayerController.Instance.PlayerMovement.Health)
            {
                heartFills[i].fillAmount = 1;
            }
            else
            {
                heartFills[i].fillAmount = 0;
            }
        }
    }
    void InstantiateHeartContainers()
    {
        for (int i = 0; i < PlayerController.Instance.PlayerMovement.maxHealth; i++)
        {
            GameObject temp = Instantiate(heartContainerPrefab);
            temp.transform.SetParent(heartsParent, false);
            heartContainers[i] = temp;
            heartFills[i] = temp.transform.Find("HeartFill").GetComponent<Image>();
        }
    }
    void UpdateHeartsHUD()
    {
        SetHeartContainers();
        SetFilledHearts();
    }
}
