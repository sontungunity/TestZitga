using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class ScrollViewRecycle : MonoBehaviour
{
    [SerializeField] private RectTransform content;
    [SerializeField] private GridLayoutGroup gridlayout;
    [SerializeField] private ScrollRect scrollRect;
    [SerializeField] private RectTransform viewport;
    [SerializeField] List<RectTransform> lstCell;
    [SerializeField] private int numberCol = 4;

    //private PlayerInfo playerinfo;
    private float SceerPoint; // 1 o tren man hinh = bao nhieu 


    //private LoadResource loadResource;
    private Vector2 sizeCell = default;

    private Bounds boundsBorder;
    private float hightCellDelta;
    private int row;

    //Show data;
    private int indexCellBot, indexCellTop;
    private int indexDataBot, indexDataTop;

    //Get listen
    private Vector2 _prevAnchoredPos;


    //Status
    private bool _recycling;

    //private Tween scrollauto;
    float normalizedPosition;

    public void Awake() {
    }

    public void Init() {
        //loadResource = ResourceManager.Instance.LoadResource;
        float sizeCellx = viewport.rect.width / numberCol;
        gridlayout.cellSize = new Vector2(sizeCellx, sizeCellx*4/3);
        sizeCell = gridlayout.cellSize;
        //Scale Size
        ScaleContent();

        //Add listenner
        scrollRect.onValueChanged.RemoveAllListeners();
        scrollRect.onValueChanged.AddListener(OnValueChangedListener);

        //Bound
        boundsBorder = new Bounds();
        SetBoundsBorder();
    }


    public void ScaleContent() {
        row = (int)(Mathf.Floor(ManagerData.Instance.GetAmountLevel() / (float)numberCol));
        if(ManagerData.Instance.GetAmountLevel() % numberCol != 0) {
            row++;
        }
        row++;
        content.sizeDelta = new Vector2(viewport.rect.width, sizeCell.y * row);
    }

    private void SetBoundsBorder() {
        Vector3[] _corners = new Vector3[4];
        viewport.GetWorldCorners(_corners);
        float highViewWold = _corners[2].y - _corners[0].y;
        float hightView = viewport.rect.height;
        SceerPoint = hightView / highViewWold;
        hightCellDelta = (lstCell.Count / numberCol) * sizeCell.y;
        float threshHold = (hightCellDelta - hightView) * 0.5f;

        boundsBorder.min = new Vector3(_corners[0].x * SceerPoint, _corners[0].y * SceerPoint - threshHold);
        boundsBorder.max = new Vector3(_corners[2].x * SceerPoint, _corners[2].y * SceerPoint + threshHold);
    }

    public void Show() {
        indexDataBot = 0;
        indexDataTop = indexDataBot;

        indexCellBot = 0;
        indexCellTop = 0;

        foreach(var cell in lstCell) {
            ViewLevel viewLevel = lstCell[indexCellTop].GetComponent<ViewLevel>();
            viewLevel.Init(indexDataTop);
            viewLevel.Show();
            indexCellTop++;
            indexDataTop = LogicIndexData(indexDataTop, true);
        }
        indexCellTop--;
        indexDataTop = LogicIndexData(indexDataTop,false);
        ;
        //MoveToLevel();
        //if(scrollauto != null) {
        //    scrollauto.Kill();
        //}

        //scrollauto = DOVirtual.DelayedCall(0.2f, () => { scrollauto = scrollRect.DOVerticalNormalizedPos(normalizedPosition + 0.0001f, 0.01f); });
    }

    public int LogicIndexData(int index,bool plus = true) {
        int row = 0;
        int indexRow = 0;
        row = index / numberCol;
        indexRow = index % numberCol;
        bool Row2 = row%2==0;
        if(plus) {
            if(Row2) {
                if(indexRow == numberCol - 1) {
                    return index + 4;
                }
                else {
                    return index + 1;
                }
            }
            else {
                if(indexRow == 0) {
                    return index + 4;
                }
                else {
                    return index - 1;
                }
            }
        }
        else {
            if(Row2) {
                if(indexRow == 0) {
                    return index - 4;
                }
                else {
                    return index - 1;
                }
            }
            else {
                if(indexRow == numberCol-1) {
                    return index - 4;
                }
                else {
                    return index + 1;
                }
            }
        }
    }

    //public void MoveToLevel() {
    //    int levelUnlock = ResourceManager.Instance.PlayerInfo.LevelUnLock;
    //    int levelMax = loadResource.LevelMax;

    //    normalizedPosition = ((levelUnlock - 5) / (float)levelMax);
    //    if(levelUnlock < 20) {
    //        normalizedPosition = 0;
    //    }
    //    if(levelUnlock > levelMax - 30) {
    //        normalizedPosition = 1;
    //    }
    //    scrollRect.normalizedPosition = new Vector2(0, normalizedPosition);
    //}

    public void OnValueChangedListener(Vector2 normalizedPos) {
        Vector2 dir = content.anchoredPosition - _prevAnchoredPos;
        HandlingAction(dir);
        _prevAnchoredPos = content.anchoredPosition;
    }

    public void HandlingAction(Vector2 direction) {
        if(_recycling || lstCell == null || lstCell.Count == 0)
            return;
        if(direction.y > 0 && indexDataBot > -5) {
            RecycleTopToBottom();
        }
        else if(direction.y < 0 && indexDataTop < 1000) {
            RecycleBottomToTop();
            //StartCoroutine(RecycleBottomToTopII());
        }

    }



    private void RecycleTopToBottom() {
        _recycling = true;
        int disWhile = 0;
        while(lstCell[indexCellTop].MinY() * SceerPoint > boundsBorder.max.y && indexDataBot > 0 && disWhile < 3000) {
            lstCell[indexCellTop].anchoredPosition -= new Vector2(0, hightCellDelta);

            indexCellBot = indexCellTop;
            indexCellTop = (indexCellTop - 1 + lstCell.Count) % lstCell.Count;

            //Set data
            indexDataBot = LogicIndexData(indexDataBot, false);
            indexDataTop = LogicIndexData(indexDataTop, false);
            lstCell[indexCellBot].GetComponent<ViewLevel>().SetLevel(indexDataBot);
            disWhile++;
        }
        _recycling = false;
    }

    private void RecycleBottomToTop() {
        _recycling = true;
        int disWhile = 0;
        //Debug.Log($"Pos cell Max y{lstCell[indexCellBot].MaxY()} and  Pos bound min Y{boundsBorder.min.y}");
        while(lstCell[indexCellBot].MaxY() * SceerPoint < boundsBorder.min.y && indexDataBot < 1000 && disWhile < 3000) {
            lstCell[indexCellBot].anchoredPosition += new Vector2(0, hightCellDelta);

            indexCellTop = indexCellBot;
            indexCellBot = (indexCellBot + 1) % lstCell.Count;

            //Set data
            indexDataBot = LogicIndexData(indexDataBot, true);
            indexDataTop = LogicIndexData(indexDataTop, true);
            lstCell[indexCellTop].GetComponent<ViewLevel>().SetLevel(indexDataTop);
            disWhile++;
        }
        _recycling = false;
    }
}
