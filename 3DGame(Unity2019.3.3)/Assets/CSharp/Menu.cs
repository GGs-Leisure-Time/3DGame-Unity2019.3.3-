using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    bool ControlSound;
    public void StartButton()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitButton()
    {
        Application.Quit();
    }
    public void SoundButton()
    {
        ControlSound = !ControlSound;
        AudioListener.pause = ControlSound;
    }

}
