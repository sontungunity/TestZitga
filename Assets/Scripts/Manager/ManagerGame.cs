using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class ManagerGame : MonoBehaviour
{

    private LevelData levelData;
    [SerializeField] private List<CellView> lstCellView;

    [SerializeField] private ButtonBase btnFineWay;

    [SerializeField] private int indexTaget;
    [SerializeField] private List<int> lstPath;
    public void Awake() {
        btnFineWay.AddListener(OnFineWay);
    }

    public void Init() {
        for(int i = 0; i < lstCellView.Count; i++) {
            lstCellView[i].Init(i, lstCellView);
        }
    }

    public void Show(int level) {
        levelData = ManagerData.Instance.LevelDatas.GetlevelDataByIndex(level);
        indexTaget = levelData.IndexTaget;
        GenderCell();
        DOVirtual.DelayedCall(0.1f,()=> {
            InGameManager.Instance.Show(lstCellView[0].GetComponent<RectTransform>().position, lstCellView[indexTaget].GetComponent<RectTransform>().position);
        });
    }

    public void GenderCell() {
        int index = 0;
        foreach(CellData cell in levelData.CellDatas) {
            lstCellView[index].Show(cell);
            index++;
        }
    }



    [ContextMenu("GetCells")]
    public void GetCells() {
        Transform gameGrid = transform.Find("GameGrid");

        Transform cells = gameGrid.transform.Find("Cells");

        CellView[] lst = cells.GetComponentsInChildren<CellView>();
        lstCellView.Clear();
        lstCellView.AddRange(lst);
    }

    public void OnFineWay() {
        lstPath = new List<int>();
        bool canFineway = lstCellView[0].FinePoint(-1, indexTaget, lstPath);
        if(canFineway) {
            List<Vector3> lstPosiotn = new List<Vector3>();
            for(int i = lstPath.Count-1; i >-1; i --) {
                lstPosiotn.Add(lstCellView[lstPath[i]].transform.position);
            }
            InGameManager.Instance.DrawLine(lstPosiotn);
        }
    }

    

    private void OnDisable() {
        InGameManager.Instance.Hide();
    }
}
