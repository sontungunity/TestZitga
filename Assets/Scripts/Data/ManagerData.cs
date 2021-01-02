using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerData : SingletonBlin<ManagerData> {
    private const string playInfo = "playInfo";

    [SerializeField] private LevelDatas levelDatas;
    [SerializeField] private PlayerInfo playerInfo;
    public LevelDatas LevelDatas => levelDatas;
    public PlayerInfo PlayerInfo => playerInfo;
    private bool isLoaded = false;

    public int GetAmountLevel() {
        return 1000;
    }

    public LevelDataSave GetLevelSave(int index) {
        return playerInfo.lstLevelDataSave.Find(x=>x.Level == index);
    }

    protected override void Awake() {
        base.Awake();
        if(!isLoaded) {
            LoadData();
        }
        isLoaded = true;
    }

    private void OnApplicationPause(bool pause) {
        if(!isLoaded) {
            return;
        }
        if(pause) {
            SaveData();
        }
    }

    private void OnApplicationQuit() {
        if(!isLoaded) {
            return;
        }
        SaveData();
    }
    private void LoadData() {
        if(PlayerPrefs.HasKey(playInfo)) {
            string dataJson = PlayerPrefs.GetString(playInfo);
            playerInfo = JsonUtility.FromJson<PlayerInfo>(dataJson);
        }
        else {
            playerInfo = new PlayerInfo();
            playerInfo.Random();
        }
    }

    private void SaveData() {
        var dataJson = JsonUtility.ToJson(playerInfo);
        PlayerPrefs.SetString(playInfo, dataJson);
        PlayerPrefs.Save();
    }

}
