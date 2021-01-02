using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class ManagerGame : MonoBehaviour
{

    private LevelData levelData;
    [SerializeField] private List<CellView> lstCellView;

    [SerializeField] private ObjectsDisplay displayButton;
    [SerializeField] private ButtonBase btnFineWay;
    [SerializeField] private ButtonBase btnAuto;

    [SerializeField] private int indexTaget;
    [SerializeField] private List<int> lstPath;

    //ResultView
    [Header("ResultView")]
    [SerializeField] private GameObject resultView;
    [SerializeField] private TextMeshProUGUI txtMeshPro;
    public void Awake() {
        btnFineWay.AddListener(OnFineWay);
        btnAuto.AddListener(OnAutoMove);
    }

    public void Init() {
        for(int i = 0; i < lstCellView.Count; i++) {
            lstCellView[i].Init(i, lstCellView);
        }
    }

    public void Show(int level) {
        levelData = ManagerData.Instance.LevelDatas.GetlevelDataByIndex(level);
        indexTaget = Random.Range(0,130);
        displayButton.SetIndexSelect(0);
        resultView.SetActive(false);
        GenderCell();
        DOVirtual.DelayedCall(0.1f,()=> {
            InGameManager.Instance.Show(lstCellView[0].GetComponent<RectTransform>().position, lstCellView[indexTaget].GetComponent<RectTransform>().position, OnShowResultView);
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
        displayButton.SetIndexSelect(1);
    }

    public void OnAutoMove() {
        InGameManager.Instance.OnAutoMove();
        displayButton.SetIndexSelect(2);
    }

    public void OnShowResultView() {
        resultView.gameObject.SetActive(true);
        int amoutStart = Random.Range(1,4);
        txtMeshPro.SetText($"You get {amoutStart} <sprite=\"stage_star1\" name=\"stage_star1\">");
        DOVirtual.DelayedCall(1f,()=> { ManagerScene.Instance.OnShowHome(); });
    }

    private void OnDisable() {
        InGameManager.Instance.Hide();
    }
}
