using UnityEngine;
using UnityEngine.SceneManagement;

public class EndMenuController : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
