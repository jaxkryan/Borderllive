using UnityEngine;
using UnityEngine.EventSystems;

public class OptionScreenController : MonoBehaviour
{
    private LevelController levelController;

    private void Start()
    {
        // Find the LevelController instance
        levelController = FindObjectOfType<LevelController>();
    }

    public void OnActionOptionClicked()
    {
        levelController.LoadSelectedScene(true);
    }

    public void OnEventOptionClicked()
    {
        levelController.LoadSelectedScene(false);
    }
}
