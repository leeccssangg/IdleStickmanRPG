using LitJson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileManager_Chien:SingletonFreeAlive<ProfileManager_Chien> {

    public static PlayerProfileChien LocalData {
        get { return instance.m_LocalData; }
    }
    [SerializeField] private PlayerProfileChien m_LocalData;


    protected override void Awake() {
        base.Awake();
        InitProfile();
    }
    public void InitProfile() {
        CreateOrLoadLocalProfile();
    }
    private void CreateOrLoadLocalProfile() {
        Debug.Log("Create Or Load Data");
        LoadDataFromPref();
    }
    private void LoadDataFromPref() {
        Debug.Log("Load Data");
        string dataText = PlayerPrefs.GetString("SuperFetch","");
        //Debug.Log(dataText);
        if(string.IsNullOrEmpty(dataText)) {
            // Dont have -> create new player and save;
            CreateNewPlayer();
        } else {
            // Have -> Load data
            LoadDataToPlayerProfile(dataText);
        }
    }
    private void CreateNewPlayer() {
        m_LocalData = new PlayerProfileChien();
        m_LocalData.CreateNewPlayer();
        m_LocalData.SaveDataToLocal(false);
    }
    private void LoadDataToPlayerProfile(string data) {
        m_LocalData = JsonMapper.ToObject<PlayerProfileChien>(data);
        m_LocalData.LoadLocalProfile();
    }

    public void SaveDataText(string data) {
        if(!string.IsNullOrEmpty(data)) {
            PlayerPrefs.SetString("SuperFetch",data);
        }
    }
    private void OnApplicationQuit() {
        
    }

}
