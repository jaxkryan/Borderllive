using UnityEngine;
using UnityEngine.SceneManagement;

public class NextScene : MonoBehaviour
{
    public string nextSceneName = "StoryScene2";
    public void LoadNextScene()
    {
        SceneManager.LoadScene(nextSceneName);
    }
}