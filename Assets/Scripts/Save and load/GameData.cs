using System.Collections.Generic;

public class GameData
{
    public int currency;
    public SerializableDictionary <string, int> inventory;
    public List<string> equipment;
    public SerializableDictionary<string, bool> skills;

    public GameData()
    {
        currency = 0;
        inventory = new SerializableDictionary<string, int>();
        equipment = new List<string>();
        skills = new SerializableDictionary<string, bool>();
    }
}
