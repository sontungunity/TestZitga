using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CellView : MonoBehaviour
{
    private int indexCell;
    private List<CellView> lstCellView;
    private CellData cellData;

    //Set up border
    [SerializeField] private ObjectsDisplay borders; // 0 : left , 1: top , 2 : rigt , 3 : bot
    private List<int> indexBorder;

    //Set up fine way
    private List<int> lstIndexCellConnect;

    public CellData CellData => cellData;
    public void Init(int indexCell,List<CellView> lstCellView) {
        this.indexCell = indexCell;
        this.lstCellView = lstCellView;
    }

    public void Show(CellData cellData) {
        this.cellData = cellData;
        ActiveBorder();
    }


    public void ActiveBorder() {
        indexBorder = new List<int>();
        lstIndexCellConnect = new List<int>();
        if(cellData.Left) {
            indexBorder.Add(0);
        }
        else {
            lstIndexCellConnect.Add(indexCell - 1);
        }

        if(cellData.Top) {
            indexBorder.Add(1);
        }
        else {
            lstIndexCellConnect.Add(indexCell - 10);
        }

        if(cellData.Right) {
            indexBorder.Add(2);
        }
        else {
            lstIndexCellConnect.Add(indexCell + 1);
        }

        if(cellData.Botton) {
            indexBorder.Add(3);
        }
        else {
            lstIndexCellConnect.Add(indexCell + 10);
        }
        borders.SetIndexSelect(indexBorder.ToArray());
    }


    ///FIne Way
    public bool FinePoint(int beforIndex, int indexTarget,List<int> lstPassed) {
        if(indexCell == indexTarget) {
            lstPassed.Add(indexCell);
            return true;
        }
        Dictionary<int,int> dicIndex_distance = new Dictionary<int, int>();
        foreach(int index_Cont in lstIndexCellConnect) {
            if(index_Cont == beforIndex) { // check index befor
                continue;
            }

            foreach(int indexPassed in lstPassed ) { // check index da di qua
                if(index_Cont == indexPassed) {
                    continue;
                }
            }
            dicIndex_distance.Add(index_Cont, DictanceByIndex(index_Cont, indexTarget, 10));
        }
        if(dicIndex_distance.Count > 0) {
            foreach(KeyValuePair<int, int> kv in dicIndex_distance.OrderBy(key => key.Value) ) {
                bool canFineTarget = lstCellView[kv.Key].FinePoint(indexCell,indexTarget,lstPassed);
                if(canFineTarget) {
                    lstPassed.Add(indexCell);
                    return true;
                }
            }
        }
        return false;
    }

    public int DictanceByIndex(int to, int from, int col_amount) {
        int row_to = to / col_amount;
        int index_to = to % col_amount;

        int row_from = from / col_amount;
        int index_from = from % col_amount;

        return (row_from-row_to)*(row_from - row_to) + (index_from - index_to)*(index_from - index_to);
    }
}
