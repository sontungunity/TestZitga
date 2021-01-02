using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ManagerHome : MonoBehaviour
{
    [SerializeField]
    private ScrollViewRecycle scrollView;
    [SerializeField] private TextMeshProUGUI amoutStar;

    private void Awake() {
        
    }

    public void Init() {
        scrollView.Init();
    }

    public void Show() {
        amoutStar.SetText(ManagerData.Instance.PlayerInfo.GetAmoutStar().ToString());
        scrollView.Show();
    }
}
