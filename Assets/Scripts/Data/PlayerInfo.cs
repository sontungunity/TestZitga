using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

[Serializable]
public class PlayerInfo 
{
    public int LevelUnlock = 10;
    public List<LevelDataSave> lstLevelDataSave = new List<LevelDataSave>();

    public PlayerInfo() {
         
    }

    public void Random() {
        this.LevelUnlock = UnityEngine.Random.Range(3, 10);
        lstLevelDataSave = new List<LevelDataSave>();
        for(int i = 0; i < LevelUnlock; i++) {
            int Amout_Star =   UnityEngine.Random.Range(1,4);
            lstLevelDataSave.Add(new LevelDataSave(i, Amout_Star));
        }
    }

    public void Uplevel(LevelDataSave save) {
        LevelDataSave saveBefor = lstLevelDataSave.Find(x=>x.Level == save.Level);
        if(saveBefor == null) {
            LevelUnlock++;
            lstLevelDataSave.Add(save);
        }
        else {
            if(saveBefor.AmountStar < save.AmountStar) {
                saveBefor.AmountStar = save.AmountStar;
            }
        }

    }

    public int GetAmoutStar() {
        return lstLevelDataSave.Sum(x => x.AmountStar);
    }
}
