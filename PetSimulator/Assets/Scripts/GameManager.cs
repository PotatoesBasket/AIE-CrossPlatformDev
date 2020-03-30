using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public Pet currentPet;

    public int money = 0; //!< Current amount of money player has.
    public int noOfPets = 1; //!< Current number of pets player has.
    const int maxPets = 25; //!< Maximum allowed number of pets.

    private void Awake()
    {
        KeepGameManager();
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
            Application.Quit();
    }

    public void SetCurrentPet(Pet pet)
    {
        HUDManager hud = HUDManager.Instance;

        currentPet = pet;
        currentPet.ToggleLight(true);

        hud.GetComponent<StatUI>().SetPet(pet.GetComponent<PetStatus>());
        hud.statPanel.SetActive(true);
    }

    public void UnselectCurrentPet()
    {
        HUDManager hud = HUDManager.Instance;

        if (currentPet != null)
        {
            currentPet.ToggleLight(false);
            currentPet = null;
        }

        hud.statPanel.SetActive(false);
    }

    /*! Returns true and increases pet count if allowed to spawn new pet,
     otherwise does nothing and returns false.*/
    public bool AddNewPet()
    {
        if (noOfPets != maxPets)
        {
            ++noOfPets;
            return true;
        }

        return false;
    }

    private void KeepGameManager()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this) // destroys object holding manager script if it's a dummy version (used for testing)
            Destroy(gameObject);
    }
}