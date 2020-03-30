using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleButtons : MonoBehaviour
{
    //! Starts game :)
    public void OnStartClick()
    {
        SceneManager.LoadScene("PetRoom");
    }
}