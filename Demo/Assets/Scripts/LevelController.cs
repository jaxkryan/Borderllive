using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    public List<string> listBattleRooms;
    public List<string> listEventRooms;
    public List<string> listShopRooms;
    public List<string> listFunRooms;

    private static string previousScene;
    private static string selectedPortal1;
    private static string selectedPortal2;
    private static int roomsVisited = 0;

    private PlayerController playerController; // Reference to PlayerController

    private void Start()
    {
        playerController = FindObjectOfType<PlayerController>(); // Get PlayerController instance
        if (playerController != null)
        {
            Debug.Log("PlayerController found successfully.");
        }
        else
        {
            Debug.LogWarning("PlayerController is null. Make sure it's in the scene.");
        }
    }

    private void SavePlayerState()
    {
        playerController = FindObjectOfType<PlayerController>(); // Get PlayerController instance
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
        ResetStaticData();
        ClearPlayerData();
        Debug.Log("Player data cleared on application quit.");
    }

    private void ClearPlayerData()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    public void ShowOption()
    {
        SceneManager.LoadScene("Option_Screen");

    }

    public void ShowRoomOptions()
    {
        roomsVisited++;
        SavePlayerState();

        if (roomsVisited == 1)
        {
            // Room 1: Fixed battle or event room
            SpawnRoom("battle", "event");
        }
        else if (roomsVisited == 5)
        {
            // Room 5: Fixed battle room
            //the illusion of free choice
            SpawnRoom("battle", "battle");
        }
        else if (roomsVisited == 6 || roomsVisited == 9)
        {
            // Room 6: Fixed shop room
            LoadSpecificRoom(listShopRooms);
        }
        else if (roomsVisited == 10)
        {
            // Room 10: Boss room
            SceneManager.LoadScene("Room_Boss");
        }
        else
        {
            // Randomly select a spawn way
            SpawnRandomWay();
        }
    }

    private void SpawnRandomWay()
    {
        // Possible combinations of portals with corresponding weights
        (string combination, int weight)[] spawnWays = new (string, int)[]
        {
        ("battle or event", 30),  // High probability
        ("battle or battle", 15), // High probability
        ("event or event", 15),   // Medium probability
        ("shop or battle", 7),   // Lower probability
        ("shop or event", 7),    // Lower probability
        ("fun or event", 10),      // Low probability
        ("fun or battle", 10),     // Low probability
        ("fun or shop", 5)        // Very low probability
        };

        // Calculate the total weight
        int totalWeight = 0;
        foreach (var way in spawnWays)
        {
            totalWeight += way.weight;
        }

        // Generate a random number between 0 and totalWeight
        int randomValue = Random.Range(0, totalWeight);
        string selectedWay = null;

        // Select the spawn way based on the random value
        int cumulativeWeight = 0;
        foreach (var way in spawnWays)
        {
            cumulativeWeight += way.weight;
            if (randomValue < cumulativeWeight)
            {
                selectedWay = way.combination;
                break;
            }
        }

        Debug.Log("Selected spawn way: " + selectedWay);

        // Spawn rooms based on the selected way
        switch (selectedWay)
        {
            case "battle or event":
                SpawnRoom("battle", "event");
                break;
            case "battle or battle":
                SpawnRoom("battle", "battle");
                break;
            case "event or event":
                SpawnRoom("event", "event");
                break;
            case "shop or battle":
                SpawnRoom("shop", "battle");
                break;
            case "shop or event":
                SpawnRoom("shop", "event");
                break;
            case "fun or event":
                SpawnRoom("fun", "event");
                break;
            case "fun or battle":
                SpawnRoom("fun", "battle");
                break;
            case "fun or shop":
                SpawnRoom("fun", "shop");
                break;
        }
    }


    private void SpawnRoom(string roomType1, string roomType2)
    {
        selectedPortal1 = GetRandomRoom(roomType1);
        selectedPortal2 = GetRandomRoom(roomType2);
        Debug.Log("Selected Portal 1: " + selectedPortal1);
        Debug.Log("Selected Portal 2: " + selectedPortal2);

        // Load the portal selection scene
        SceneManager.LoadScene("Option_Screen");
    }

    private string GetRandomRoom(string roomType)
    {
        List<string> roomList = GetRoomList(roomType);
        if (roomList.Count > 0)
        {
            return roomList[Random.Range(0, roomList.Count)];
        }
        else
        {
            Debug.LogWarning("Room list for type " + roomType + " is empty.");
            return null;
        }
    }

    private List<string> GetRoomList(string roomType)
    {
        switch (roomType)
        {
            case "battle": return listBattleRooms;
            case "event": return listEventRooms;
            case "shop": return listShopRooms;
            case "fun": return listFunRooms;
            default: return new List<string>();
        }
    }

    private void LoadSpecificRoom(List<string> roomList)
    {
        if (roomList.Count > 0)
        {
            string roomName = roomList[Random.Range(0, roomList.Count)];
            previousScene = roomName;
            SceneManager.LoadScene(roomName);
        }
        else
        {
            Debug.LogWarning("Specified room list is empty.");
        }
    }

    public void OnPortal1Selected()
    {
        LoadSelectedRoom(true);
    }

    public void OnPortal2Selected()
    {
        LoadSelectedRoom(false);
    }

    private void LoadSelectedRoom(bool isPortal1)
    {
        string sceneName = isPortal1 ? selectedPortal1 : selectedPortal2;
        previousScene = sceneName;
        SceneManager.LoadScene(sceneName);
    }

    public static void ResetStaticData()
    {
        previousScene = null;
        selectedPortal1 = null;
        selectedPortal2 = null;
        roomsVisited = 0;
    }
}
