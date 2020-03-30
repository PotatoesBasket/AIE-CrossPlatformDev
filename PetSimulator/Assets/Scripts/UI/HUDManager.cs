using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    GameManager manager;

    public Text currentMoney;
    public Text currentPets;

    public GameObject HUDPanel;
    public GameObject itemPanel;
    public GameObject statPanel;

    public static HUDManager Instance;

    private void Start()
    {
        Singleton();
        manager = GameManager.Instance;
    }

    //! Updates text on HUD
    private void Update()
    {
        currentMoney.text = "$" + manager.money.ToString();
        currentPets.text = ": " + manager.noOfPets.ToString();
    }

    private void Singleton()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);
    }
}