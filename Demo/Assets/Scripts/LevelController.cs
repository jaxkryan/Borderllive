using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

    private void Start()
    {
        if (tempAction == null || tempEvent == null)
        {
            InitializeLists();
        }
    }

    private void SavePlayerState()
    {
        // TODO: Implement saving player state
        Debug.Log("Saving player state...");
    }

    public void ShowRoomOptions()
    {
        roomsVisited++;

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
        SavePlayerState();
        previousScene = bossRoomName;
        SceneManager.LoadScene(bossRoomName);
    }

    public void LoadSelectedScene(bool isAction)
    {
        string sceneName = isAction ? selectedActionRoom : selectedEventRoom;
        List<string> list = isAction ? tempAction : tempEvent;
        SavePlayerState();
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