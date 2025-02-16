using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

[System.Serializable]
public class PlayerResource
{
    public ResourceWallet rw;
    public KeyWallet kw;

    public ResourceWallet Resources { get => rw; set => rw = value; }
    public KeyWallet Keys { get => kw; set => kw = value; }

    #region Save & Load data
    public virtual void CreateNewData()
    {
        Resources = new ResourceWallet(new List<ResourceData>
        {
            new ResourceData(ResourceData.ResourceType.GOLD,100,"UpdateCoin"),
            new ResourceData(ResourceData.ResourceType.GEM,100,"UpdateGem"),
            new ResourceData(ResourceData.ResourceType.ARTIFACT_STONE,2000,"UpdateArtifactStone"),
            new ResourceData(ResourceData.ResourceType.ITEM,0,"UpdateItem"),
            new ResourceData(ResourceData.ResourceType.MINE_STONE,10000000,"UpdateMineStone"),
        });
        Keys = new KeyWallet(new List<DungeonKeyData>
        {
            new DungeonKeyData(DungeonType.BOSS_RUSH,
                new List<KeyData>
                {
                    new KeyData(KeyType.FREE,2, "Update" + DungeonType.BOSS_RUSH.ToString()+ KeyType.FREE.ToString()),
                    new KeyData(KeyType.ADS,2,"Update" + DungeonType.BOSS_RUSH.ToString()+ KeyType.ADS.ToString()),
                    new KeyData(KeyType.BONUS, 0,"Update" + DungeonType.BOSS_RUSH.ToString()+ KeyType.BONUS.ToString()),
                }),
            new DungeonKeyData(DungeonType.GOLD_RUSH,
                new List<KeyData>
                {
                    new KeyData(KeyType.FREE,2,"Update" + DungeonType.GOLD_RUSH.ToString()+ KeyType.FREE.ToString()),
                    new KeyData(KeyType.ADS,2,"Update" + DungeonType.GOLD_RUSH.ToString()+ KeyType.ADS.ToString()),
                    new KeyData(KeyType.BONUS, 0,"Update" + DungeonType.GOLD_RUSH.ToString()+ KeyType.BONUS.ToString()),
                }),
            new DungeonKeyData(DungeonType.MONSTER_NEST,
                new List<KeyData>
                {
                    new KeyData(KeyType.FREE,1,"Update" + DungeonType.MONSTER_NEST.ToString()+ KeyType.FREE.ToString()),
                    new KeyData(KeyType.ADS,1,"Update" + DungeonType.MONSTER_NEST.ToString()+ KeyType.ADS.ToString()),
                    new KeyData(KeyType.BONUS, 0,"Update" + DungeonType.MONSTER_NEST.ToString()+ KeyType.BONUS.ToString()),
                }),
            new DungeonKeyData(DungeonType.CHALLENGE_KING,
                new List<KeyData>
                {
                    new KeyData(KeyType.FREE,1,"Update" + DungeonType.CHALLENGE_KING.ToString()+ KeyType.FREE.ToString()),
                    new KeyData(KeyType.ADS,1,"Update" + DungeonType.CHALLENGE_KING.ToString()+ KeyType.ADS.ToString()),
                    new KeyData(KeyType.BONUS, 0,"Update" + DungeonType.CHALLENGE_KING.ToString()+ KeyType.BONUS.ToString()),
                })
        });
        //Resources.AddOnValueChangeCallBack();
        //Keys.AddOnValueChangeCallback();
    }
    public virtual void LoadData()
    {
        for (int i = 0; i < Resources.Count; i++)
        {
            Resources[i].LoadDataFromValueResolve();
        }
        Keys.AddAllKeyValueChangeCallback();
    }
    #endregion
    #region Resource Function
    public BigNumber GetResourceValue(ResourceData.ResourceType resourceType)
    {
        return Resources.GetResourceValue(resourceType);
    }
    public void AddGameResource(ResourceData.ResourceType resourceType, BigNumber value)
    {
        Resources.AddGameResource(resourceType, value);
    }
    public void AddGameResource(ResourceData resource)
    {
        Resources.AddGameResource(resource);
    }
    public void AddGameResource(ResourceWallet resourceList)
    {
        for (int i = 0; i < resourceList.Count; i++)
        {
            Resources.AddGameResource(resourceList[i]);
        }
    }
    public void ConsumeGameResource(ResourceData.ResourceType resourceType, BigNumber value)
    {
        Resources.ConsumeGameResource(resourceType, value);
    }
    public void ConsumeGameResource(ResourceData resource)
    {
        Resources.ConsumeGameResource(resource);
    }
    public void ConsumeGameResource(ResourceWallet resourceList)
    {
        for (int i = 0; i < resourceList.Count; i++)
        {
            Resources.ConsumeGameResource(resourceList[i]);
        }
    }
    public bool IsEnoughGameResource(ResourceData.ResourceType resourceType, BigNumber value)
    {
        return Resources.IsEnoughGameResource(resourceType, value);
    }
    public bool IsEnoughGameResource(ResourceData resource)
    {
        return Resources.IsEnoughGameResource(resource);
    }
    public bool IsEnoughGameResource(ResourceWallet resourceList)
    {
        for (int i = 0; i < resourceList.Count; i++)
        {
            if (!Resources.IsEnoughGameResource(resourceList[i]))
            {
                return false;
            }
        }

        return true;
    }
    #endregion
    #region Key function
    public int GetKeyValue(DungeonType dungeonType, KeyType keyType)
    {
       return Keys.GetKeyValue(dungeonType, keyType);
    }
    public void AddKey(DungeonType dungeonType,KeyType keyType, int value)
    {
        Keys.AddKey(dungeonType, keyType, value);
    }
    public void AddKey(DungeonKeyData dungeonKeyData, KeyData keyData)
    {
        Keys.AddKey(dungeonKeyData, keyData);
    }
    public void AddKey(KeyWallet keyList)
    {
        for (int i = 0; i < keyList.Count; i++)
        {
            for(int j = 0;j< keyList[i].Count;j++)
                Keys.AddKey(keyList[i],keyList[i][j]);
        }
    }
    public void ConsumeKey(DungeonType dungeonType, KeyType keyType, int value)
    {
        Keys.ConsumeKey(dungeonType, keyType, value);
    }
    public void ConsumeKey(DungeonKeyData dungeonKeyData, KeyData keyData)
    {
        Keys.ConsumeKey(dungeonKeyData, keyData);
    }
    public void ConsumeKey(KeyWallet keyList)
    {
        for (int i = 0; i < keyList.Count; i++)
        {
            for (int j = 0; j < keyList[i].Count; j++)
                Keys.ConsumeKey(keyList[i], keyList[i][j]);
        }
    }
    public bool IsEnoughKey(DungeonType dungeonType, KeyType keyType, int value)
    {
        return Keys.IsEnoughKey(dungeonType, keyType, value);
    }
    public bool IsEnoughKey(DungeonKeyData dungeonKeyData, KeyData keyData)
    {
        return Keys.IsEnoughKey(dungeonKeyData, keyData);
    }
    public bool IsEnoughKey(KeyWallet keyList)
    {
        for (int i = 0; i < keyList.Count; i++)
        {
            for (int j = 0; j < keyList[i].Count; j++)
            {
                if (!Keys.IsEnoughKey(keyList[i], keyList[i][j]))
                {
                    return false;
                }
            }
        }
        return true;
    }
    #endregion
    #region Data String
    public string ToJsonString() {
        return LitJson.JsonMapper.ToJson(this, true, false);
    }
    #endregion

}
