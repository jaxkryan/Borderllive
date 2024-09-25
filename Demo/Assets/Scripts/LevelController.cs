using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public List<string> listAction;
    public List<string> listEvent;
    private static List<string> tempAction;
    private static List<string> tempEvent;
    private static string previousScene;
    private static string selectedActionRoom;
    private static string selectedEventRoom;
    private static int roomsVisited = 0;
    private static int bossCounter = 0;

    private PlayerController playerController; // Reference to PlayerController

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>(); // Get PlayerController instance
        if (playerController != null)
        {
            //Debug.Log("PlayerController found successfully.");
        }
        else
        {
            Debug.LogWarning("PlayerController is null. Make sure it's in the scene.");
        }

        if (tempAction == null || tempEvent == null)
        {
            InitializeLists();
        }
    }


    private void SavePlayerState()
    {
        playerController = FindObjectOfType<PlayerController>(); // Get PlayerController instance
        Debug.Log(playerController);
        Debug.Log("Savinggggg");
        if (playerController != null)
        {
            playerController.SavePlayerState(); // Call PlayerController's SavePlayerState
            Debug.Log("Player state saved from LevelController.");
        }
        else
        {
            Debug.LogWarning("PlayerController reference is null. Cannot save player state.");
        }
    }

    private void OnApplicationQuit()
    {
        ClearPlayerData();
        Debug.Log("Player data cleared on application quit.");
    }

    private void ClearPlayerData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void ShowRoomOptions()
    {
        roomsVisited++;
        SavePlayerState();

        if (roomsVisited % 5 == 0)
        {
            LoadBossRoom();
        }
        else
        {
            if (tempAction.Count == 0 || tempEvent.Count == 0)
            {
                InitializeLists();
            }
            selectedActionRoom = tempAction[Random.Range(0, tempAction.Count)];
            selectedEventRoom = tempEvent[Random.Range(0, tempEvent.Count)];
            SceneManager.LoadScene("Option_Screen");
        }
    }

    private void LoadBossRoom()
    {
        string bossRoomName = (bossCounter % 2 == 0) ? "Room_Boss" : "Room_Boss2";
        bossCounter++;
        previousScene = bossRoomName;
        SceneManager.LoadScene(bossRoomName);
    }

    public void LoadSelectedScene(bool isAction)
    {
        string sceneName = isAction ? selectedActionRoom : selectedEventRoom;
        List<string> list = isAction ? tempAction : tempEvent;
        list.Remove(sceneName);
        previousScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    private void InitializeLists()
    {
        tempAction = new List<string>(listAction);
        tempEvent = new List<string>(listEvent);
    }

    public void OnActionOptionClicked()
    {
        LoadSelectedScene(true);
    }

    public void OnEventOptionClicked()
    {
        LoadSelectedScene(false);
    }
}
