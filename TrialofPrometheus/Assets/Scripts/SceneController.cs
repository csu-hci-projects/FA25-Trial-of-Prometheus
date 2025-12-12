using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    
    public void PlayerDied(float delay)
    {
        StartCoroutine(RestartAfterDelay(delay));
    }
    IEnumerator RestartAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("EndMenu");
    }

    public void BossDied(float delay)
    {
        StartCoroutine(VictoryAfterDelay(delay));
    }
    IEnumerator VictoryAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("Victory");
    }

    
}
