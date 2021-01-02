using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName ="LevelDatas",menuName = "Data/LevelDatas")]
public class LevelDatas : ScriptableObject
{
    [SerializeField] private List<LevelData> lstLevelDatas;

    public IEnumerable<LevelData> LstLevelDatas => lstLevelDatas;

    public LevelData GetlevelDataByIndex(int index) {
        return lstLevelDatas[0];
    }
}


