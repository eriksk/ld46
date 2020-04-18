using UnityEngine;
using UnityEngine.SceneManagement;

namespace LD46.Scenes
{
    public class MenuScene : MonoBehaviour
    {
        public void StartGame()
        {
            Debug.Log("Loading Game Scene");
            SceneManager.LoadScene("Game", LoadSceneMode.Single);
        }

        public void ExitGame()
        {
            Debug.Log("Application Quitting");
            Application.Quit();
        }
    }
}