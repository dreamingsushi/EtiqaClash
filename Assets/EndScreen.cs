using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public void ReturnToMenu()
    {
        if(PlayerPrefs.GetInt("DailyGame") != 1)
        {
            PlayerPrefs.SetInt("DailyGame", 1);
        }
        SceneManager.LoadScene(0);
    }

    public void ExitTutorial()
    {
        SceneManager.LoadScene(0);
    }
}
