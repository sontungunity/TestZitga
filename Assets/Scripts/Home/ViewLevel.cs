using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ViewLevel : MonoBehaviour
{
    [SerializeField] private ObjectsDisplay linesDisplays;
    [SerializeField] private ObjectsDisplay infoDisplays;

    [SerializeField] private ButtonBase btnPlayGame;

    [Header("LevelInfo")]
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private GameObject spiderWeb;

    [SerializeField] private ObjectsDisplay lstStar;
    private LevelDataSave levelSave;
    private PlayerInfo playerInfo;

    private int level;

    private void Awake() {
        playerInfo = ManagerData.Instance.PlayerInfo;
        btnPlayGame.AddListener(OnSelected);
    }

    public void Init(int level) {
        this.level = level;
        this.levelSave = ManagerData.Instance.GetLevelSave(level);
        Show();
    }

    public void SetLevel(int level) {
        this.level = level;
        this.levelSave = ManagerData.Instance.GetLevelSave(level);
        Show();
    }

    public void Show() {
        Show_Line();

        Show_Info();

        Show_Star();
    }

    public void Show_Line() {
        if((level + 1) % 8 == 0) {
            linesDisplays.SetIndexSelect(0, 1);
        }else if((level + 1) % 4 == 0) {
            linesDisplays.SetIndexSelect(0);
        }else {
            if(level%4==0 && level % 8 !=0) {
                linesDisplays.SetIndexSelect(2);
            }
            else {
                linesDisplays.SetIndexSelect(1);
            }
        }
    }

    public void Show_Info() {
        if(level == 0) {
            infoDisplays.SetIndexSelect(1);
        }
        else {
            infoDisplays.SetIndexSelect(0);
            txtLevel.SetText((level+1).ToString());
            spiderWeb.SetActive(playerInfo.LevelUnlock < level);
        }
    }

    public void Show_Star() {
        if(playerInfo.LevelUnlock < level || levelSave == null) {
            lstStar.SetIndexSelect(3);
        }
        else {
            List<int> lst = new List<int>();
            for(int i = 0;i < levelSave.AmountStar; i++) {
                lst.Add(i);
            }
            lstStar.SetIndexSelect(lst.ToArray());
        }
    }
    public void OnSelected() {
        ManagerScene.Instance.OnShowInGame(level);
    }
}
