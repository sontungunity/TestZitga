using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerHome : MonoBehaviour
{
    [SerializeField]
    private ScrollViewRecycle scrollView;

    private void Awake() {
        
    }

    public void Init() {
        scrollView.Init();
    }

    public void Show() {
        scrollView.Show();
    }
}
