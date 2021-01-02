using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
[CreateAssetMenu(fileName = "LevelData", menuName = "Data/LevelData")]
public class LevelData : ScriptableObject {
    [SerializeField] private int index;
    [SerializeField] private List<CellData> celldatas;
    [SerializeField] private int indexTaget;
    public int Index => index;

    public IEnumerable<CellData> CellDatas => celldatas;

    public int IndexTaget => indexTaget;

    public LevelData() {
        index = 0;
        celldatas = new List<CellData>();
        for(int row = 0; row < 13; row ++) {
            for(int col = 0; col < 10; col ++) {
                celldatas.Add(new CellData(row,col));
            }
        }
        indexTaget = 9;
    }
}

[Serializable]
public class CellData {
    [SerializeField,Range(0,12)] private int row;
    [SerializeField,Range(0,10)] private int col;
    [Header("Border")]
    [SerializeField] private bool left;
    [SerializeField] private bool top;
    [SerializeField] private bool right;
    [SerializeField] private bool botton;

    public int Row => row;

    public int Col => col;

    public bool Left => left;

    public bool Top => top;

    public bool Right => right;

    public bool Botton => botton;


    public CellData() {

    }

    public CellData(int row, int col) {
        this.row = row;
        this.col = col;
    }
}
