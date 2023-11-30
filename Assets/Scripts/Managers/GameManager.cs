using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISavePoint
{
    public static GameManager Instance { get; private set; }
    public bool IsGamePause { get; private set; }

    [SerializeField] private Checkpoint[] checkpoints;

    [Header("Lost currency")]
    [SerializeField] private GameObject lostCurrencyPrefab;
    private int lostCurrencyAmount;
    private float lostCurrencyX;
    private float lostCurrencyY;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        checkpoints = FindObjectsOfType<Checkpoint>();
    }

    public void PauseGame(bool isPause)
    {
        if (isPause)
        {
            IsGamePause = isPause;
            Time.timeScale = 0;
        }
        else
        {
            IsGamePause = isPause;
            Time.timeScale = 1;
        }
    }

    public void RestartScene()
    {
        SaveManager.Instance.SaveGame();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void ExitGame()
    {
        SaveManager.Instance.SaveGame();
        Application.Quit();
    }

    public void SaveData(ref GameData data)
    {
        data.checkpoints.Clear();

        data.lostCurrencyAmount = lostCurrencyAmount;
        data.lostCurrencyX = PlayerManager.Instance.Player.transform.position.x;
        data.lostCurrencyY = PlayerManager.Instance.Player.transform.position.y;

        data.closestCheckpointId = FindClosestCheckpoint()?.id;

        foreach (Checkpoint checkpoint in checkpoints)
        {
            data.checkpoints.Add(checkpoint.id, checkpoint.Activated);
        }
    }

    public void LoadData(GameData data)
    {
        LoadLostCurrency(data);

        if (data.closestCheckpointId != null)
        {
            LoadCheckpoint(data);
        }
    }

    private void LoadCheckpoint(GameData data)
    {
        foreach (KeyValuePair<string, bool> kvp in data.checkpoints)
        {
            foreach (Checkpoint checkpoint in checkpoints)
            {
                if (checkpoint.id == kvp.Key && kvp.Value)
                {
                    checkpoint.ActivateCheckpoint();
                    break;
                }
            }
        }

        foreach (Checkpoint checkpoint in checkpoints)
        {
            if (data.closestCheckpointId == checkpoint.id)
            {

                PlayerManager.Instance.Player.transform.position = checkpoint.transform.position;
            }
        }
    }

    private void LoadLostCurrency(GameData data)
    {
        lostCurrencyAmount = data.lostCurrencyAmount;
        lostCurrencyX = data.lostCurrencyX;
        lostCurrencyY = data.lostCurrencyY;

        if (lostCurrencyAmount > 0)
        {
            GameObject newLostCurrency = Instantiate(lostCurrencyPrefab, new Vector3(lostCurrencyX, lostCurrencyY), Quaternion.identity);
            newLostCurrency.GetComponent<LostCurrencyController>().AddCurrency(lostCurrencyAmount);
        }

        lostCurrencyAmount = 0;
    }
    private Checkpoint FindClosestCheckpoint()
    {
        float closestDistance = Mathf.Infinity;
        Checkpoint closestCheckpoint = null;

        foreach (var checkpoint in checkpoints)
        {
            float distanceToCheckpoint = Vector2.Distance(PlayerManager.Instance.Player.transform.position, checkpoint.transform.position);

            if (distanceToCheckpoint < closestDistance && checkpoint.Activated)
            {
                closestDistance = distanceToCheckpoint;
                closestCheckpoint = checkpoint;
            }
        }

        return closestCheckpoint;
    }
    public void SetLostCurrency(int currency)
    {
        lostCurrencyAmount = currency;
    }
}
