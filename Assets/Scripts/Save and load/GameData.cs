using System.Collections.Generic;

public class GameData
{
    public int currency;
    public List<string> equipment;

    public SerializableDictionary <string, int> inventory;
    public SerializableDictionary<string, bool> skills;

    public SerializableDictionary<string, bool> checkpoints;
    public string closestCheckpointId;

    public float lostCurrencyX;
    public float lostCurrencyY;
    public int lostCurrencyAmount;

    public GameData()
    {
        currency = 0;
        lostCurrencyX = 0;
        lostCurrencyY = 0;
        lostCurrencyAmount = 0;

        inventory = new SerializableDictionary<string, int>();
        equipment = new List<string>();
        skills = new SerializableDictionary<string, bool>();

        checkpoints = new SerializableDictionary<string, bool>();
        closestCheckpointId = string.Empty;
    }
}
