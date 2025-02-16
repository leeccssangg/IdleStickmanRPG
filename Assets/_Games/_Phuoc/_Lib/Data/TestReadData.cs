using System.Collections.Generic;
using UnityEngine;

public class TestReadData : MonoBehaviour
{
    public const string INDEX = "INDEX";
    public const string UNIT_ID = "UNIT_ID";
    public const string STANDING_GROUP = "STANDING_GROUP";
    public const string HP = "HP";
    public const string MOVE_SPEED = "MOVE_SPEED";
    public const string FIND_TARGET_TYPE = "FIND_TARGET_TYPE";
    public const string PRICE = "PRICE";
    public const string BODY_SIZE = "BODY_SIZE";
    public const string SPACE = "SPACE";
    public const string SPAWN_TIME = "SPAWN_TIME";

    public const string PRODUCTION = "PRODUCTION";
    public const string CAPACITY = "CAPACITY";
    public const string MINING_CAPACITY = "MINING_CAPACITY";

    public const string DAMAGE = "DAMAGE";
    public const string ATTACK_SPEED = "ATTACK_SPEED";
    public const string ATTACK_RANGE = "ATTACK_RANGE";
    public const string TRIGGER_DAMAGE = "TRIGGER_DAMAGE";
    public const string VIEW_RANGE = "VIEW_RANGE";

            

    public TextAsset[] assets;

    // Start is called before the first frame update
    void Start()
    {
        ReadMiner(assets[0]);

        for (int i = 1; i < assets.Length; i++)
        {
            Debug.LogError(i);
            ReadSoldier(assets[i]);
        }

    }

    public void ReadMiner(TextAsset asset)
    {
        List<Dictionary<string, string>> itemConfig = CSVReader.Read(asset);

        for (int i = 0; i < itemConfig.Count; i++)
        {
            Debug.Log(
                itemConfig[i][INDEX] +
                itemConfig[i][UNIT_ID] +
                itemConfig[i][STANDING_GROUP] +
                itemConfig[i][HP] +
                itemConfig[i][MOVE_SPEED] +
                itemConfig[i][FIND_TARGET_TYPE] +
                itemConfig[i][PRICE] +
                itemConfig[i][BODY_SIZE] +
                itemConfig[i][SPACE] +
                itemConfig[i][SPAWN_TIME] +
                itemConfig[i][PRODUCTION] +
                itemConfig[i][CAPACITY] +
                itemConfig[i][MINING_CAPACITY]
                );
        }
    }   
    
    public void ReadSoldier(TextAsset asset)
    {
        List<Dictionary<string, string>> itemConfig = CSVReader.Read(asset);

        for (int i = 0; i < itemConfig.Count; i++)
        {
            Debug.Log(
                itemConfig[i][INDEX] +
                itemConfig[i][UNIT_ID] +
                itemConfig[i][STANDING_GROUP] +
                itemConfig[i][HP] +
                itemConfig[i][MOVE_SPEED] +
                itemConfig[i][FIND_TARGET_TYPE] +
                itemConfig[i][PRICE] +
                itemConfig[i][BODY_SIZE] +
                itemConfig[i][SPACE] +
                itemConfig[i][SPAWN_TIME] +
                itemConfig[i][DAMAGE] +
                itemConfig[i][ATTACK_SPEED] +
                itemConfig[i][ATTACK_RANGE] +
                itemConfig[i][TRIGGER_DAMAGE] +
                itemConfig[i][VIEW_RANGE]
                );
        }
    }

}
