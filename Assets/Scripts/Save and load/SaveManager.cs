using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UIElements;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance { get; private set; }

    [SerializeField] private string fileName;
    [SerializeField] private bool encrypt;


    private GameData gameData;
    private FileDataHandler fileDataHandler;
    private List<ISavePoint> savePoints;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(Instance.gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encrypt);
        savePoints = FindAllSavePoints();

        LoadGame();
    }

    public void NewGame()
    {
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = fileDataHandler.Load();

        if (this.gameData == null)
        {
            NewGame();
        }

        foreach (var savePoint in savePoints)
        {
            savePoint.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        foreach (var savePoint in savePoints)
        {
            savePoint.SaveData(ref gameData);
        }

        fileDataHandler.Save(gameData);

    }

    [ContextMenu("Delete save file")]
    public void DeleteSavedData()
    {
        fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, encrypt);

        fileDataHandler.DeleteData();
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISavePoint> FindAllSavePoints()
    {
        IEnumerable<ISavePoint> savePoints = FindObjectsOfType<MonoBehaviour>().OfType<ISavePoint>();

        return new List<ISavePoint>(savePoints);
    }

    public bool HasSavedData() => fileDataHandler.Load() != null;
}
