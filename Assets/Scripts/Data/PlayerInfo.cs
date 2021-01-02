using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class PlayerInfo 
{
    public int LevelUnlock = 10;
    public List<LevelDataSave> lstLevelDataSave = new List<LevelDataSave>();
}
