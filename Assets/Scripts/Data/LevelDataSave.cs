using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class LevelDataSave
{
    public int Level;
    public int AmountStar;

    public LevelDataSave() {

    }

    public LevelDataSave(int level, int AmountStar) {
        this.Level = level;
        this.AmountStar = AmountStar;
    }
}
