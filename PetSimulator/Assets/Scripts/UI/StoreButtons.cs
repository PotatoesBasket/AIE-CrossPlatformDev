using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreButtons : MonoBehaviour
{
    GameManager manager;
    public Transform spawnPoint;

    public GameObject water;
    public GameObject banana;
    public GameObject cake;
    public GameObject pizza;
    public GameObject egg;

    private void Start()
    {
        manager = GameManager.Instance;
    }

    public void BuyWater()
    {
        if (manager.money - 150 >= 0)
        {
            manager.money -= 150;
            Instantiate(water, spawnPoint.position, Quaternion.identity, null);
        }
    }

    public void BuyBanana()
    {
        if (manager.money - 350 >= 0)
        {
            manager.money -= 350;
            Instantiate(banana, spawnPoint.position, Quaternion.identity, null);
        }
    }

    public void BuyCake()
    {
        if (manager.money - 650 >= 0)
        {
            manager.money -= 650;
            Instantiate(cake, spawnPoint.position, Quaternion.identity, null);
        }
    }

    public void BuyPizza()
    {
        if (manager.money - 900 >= 0)
        {
            manager.money -= 900;
            Instantiate(pizza, spawnPoint.position, Quaternion.identity, null);
        }
    }

    public void BuyEgg()
    {
        if (manager.money - 2000 >= 0 && manager.AddNewPet())
        {
            manager.money -= 2000;
            Instantiate(egg, spawnPoint.position, Quaternion.identity, null);
        }
    }
}