using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
public class ManagerGame : MonoBehaviour
{

    private LevelData levelData;
    private int level;
    [SerializeField] private List<CellView> lstCellView;

    [SerializeField] private ObjectsDisplay displayButton;
    [SerializeField] private ButtonBase btnFineWay;
    [SerializeField] private ButtonBase btnAuto;


    [SerializeField] private List<int> lstPath;

    //ResultView
    [Header("ResultView")]
    [SerializeField] private GameObject resultView;
    [SerializeField] private TextMeshProUGUI txtMeshPro;

    [SerializeField]private int indexCur = 0;
    [SerializeField]private int indexTaget;

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
        this.level = level;
        levelData = ManagerData.Instance.LevelDatas.GetlevelDataByIndex(level);
        indexCur = 0;
        indexTaget = Random.Range(0,130);
        displayButton.SetIndexSelect(0);
        resultView.SetActive(false);
        GenderCell();
        DOVirtual.DelayedCall(0.1f,()=> {
            InGameManager.Instance.Show(lstCellView[indexCur].GetComponent<RectTransform>().position, lstCellView[indexTaget].GetComponent<RectTransform>().position, OnShowResultView);
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
        displayButton.SetIndexSelect(2);
        bool canFineway = lstCellView[indexCur].FinePoint(-1, indexTaget, lstPath);
        if(canFineway) {
            List<Vector3> lstPosiotn = new List<Vector3>();
            for(int i = lstPath.Count-1; i >-1; i --) {
                lstPosiotn.Add(lstCellView[lstPath[i]].transform.position);
            }
            InGameManager.Instance.DrawLine(lstPosiotn);
            displayButton.SetIndexSelect(1);
        }
        else {
            InGameManager.Instance.Hide();
            resultView.gameObject.SetActive(true);
            txtMeshPro.SetText($" Khong co duong di toi day");
            DOVirtual.DelayedCall(1f, () => {
                ManagerScene.Instance.OnShowHome();
            });
        }
        
    }

    public void OnAutoMove() {
        InGameManager.Instance.OnAutoMove();
        displayButton.SetIndexSelect(2);
    }

    public void OnShowResultView() {
        InGameManager.Instance.Hide();
        resultView.gameObject.SetActive(true);
        int amoutStart = Random.Range(1,4);
        txtMeshPro.SetText($"You get {amoutStart} <sprite=\"stage_star1\" name=\"stage_star1\">");
        ManagerData.Instance.PlayerInfo.Uplevel(new LevelDataSave(this.level,amoutStart));
        DOVirtual.DelayedCall(1f,()=> {
            ManagerScene.Instance.OnShowHome(); 
        });
    }


#region MoveBug
    //Get updown bug
    private void Update() {
        Swipe();
    }

    Vector2 firstPressPos;
    Vector2 secondPressPos;
    Vector2 currentSwipe;

    bool isMoving = false;
    public void Swipe() {
        if(Input.GetMouseButtonDown(0)) {
            //save began touch 2d point
            firstPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        if(Input.GetMouseButton(0)) {
            if(isMoving) {
                return;
            }
            //save ended touch 2d point
            secondPressPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            //create vector from the two points
            currentSwipe = new Vector2(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

            //normalize the 2d vector
            currentSwipe.Normalize();

            //swipe upwards
            if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                MoveUp();
            }
            //swipe down
            if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f) {
                MoveBotton();
            }
            //swipe left
            if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                MoveLeft();
            }
            //swipe right
            if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f) {
                MoveRight();
            }
            firstPressPos = secondPressPos;
        }
    }

    public void MoveLeft() {
        if(!lstCellView[indexCur].CellData.Left){
            isMoving = true;
            indexCur = indexCur - 1;
            InGameManager.Instance.MoveBug(lstCellView[indexCur].GetComponent<RectTransform>().position, OnMoveComplate);
        }
    } 

    public void MoveUp() {
        if(!lstCellView[indexCur].CellData.Top) {
            isMoving = true;
            indexCur = indexCur - 10;
            InGameManager.Instance.MoveBug(lstCellView[indexCur].GetComponent<RectTransform>().position, OnMoveComplate);
        }
    }

    public void MoveRight() {
        if(!lstCellView[indexCur].CellData.Right) {
            isMoving = true;
            indexCur = indexCur +1;
            InGameManager.Instance.MoveBug(lstCellView[indexCur].GetComponent<RectTransform>().position, OnMoveComplate);
        }
    }

    public void MoveBotton() {
        if(!lstCellView[indexCur].CellData.Botton) {
            isMoving = true;
            indexCur = indexCur + 10;
            Debug.Log("Move +10");
            InGameManager.Instance.MoveBug(lstCellView[indexCur].GetComponent<RectTransform>().position, OnMoveComplate);
        }
    }

    public void OnMoveComplate() {
        isMoving = false;
    }
#endregion
}
