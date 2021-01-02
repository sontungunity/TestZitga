using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class InGameManager : SingletonBlin<InGameManager>
{
    [SerializeField]private GameObject display;
    [SerializeField]private LineRenderer lineRenderer;
    [SerializeField]private Transform mainBug;
    [SerializeField]private Transform pointTaget;

    private List<Vector3> checkpoint = new List<Vector3>();
    private Action OnComplate;
    public void Show(Vector3 positionBug, Vector3 posionTarget, Action OnComplate) {
        display.SetActive(true);
        mainBug.position = positionBug;
        pointTaget.position = posionTarget;
        this.OnComplate = OnComplate;
        this.lineRenderer.positionCount = 0;
    }

    public void Hide() {
        display.SetActive(false);
    }


    public void DrawLine(List<Vector3> checkpoint) {
        this.lineRenderer.startWidth = 0.1f;
        this.lineRenderer.endWidth = 0.1f;
        this.lineRenderer.positionCount = checkpoint.Count;

        this.checkpoint = new List<Vector3>();
        this.checkpoint.AddRange(checkpoint);

        this.lineRenderer.SetPositions(checkpoint.ToArray());
    }

    public void OnAutoMove() {
        DOVirtual.DelayedCall(0.5f, () => {
            mainBug.DOPath(checkpoint.ToArray(), checkpoint.Count*0.5f, PathType.Linear, PathMode.TopDown2D)
            .SetLookAt(0.1f).OnComplete(()=> { OnComplate?.Invoke(); });
        });
    }

}
