using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class InGameManager : SingletonBlin<InGameManager> {
    [SerializeField]private GameObject display;
    [SerializeField]private LineRenderer lineRenderer;
    [SerializeField]private Transform mainBug;
    [SerializeField]private Transform pointTaget;

    private List<Vector3> checkpoint = new List<Vector3>();
    private Action OnComplate;
    protected Vector3 lastPosition;
    protected Tween tweenMove;
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
        tweenMove = DOVirtual.DelayedCall(0.5f, () => {
            mainBug.DOPath(checkpoint.ToArray(), checkpoint.Count * 0.25f, PathType.Linear, PathMode.Sidescroller2D)
            //.SetLookAt(0.1f)
            .OnUpdate(LookAt)
            .OnComplete(() => { OnComplate?.Invoke(); });
        });
    }

    public void LookAt() {
        Vector3 currentDirection = mainBug.transform.position - lastPosition;
        mainBug.transform.up = currentDirection.normalized;
        lastPosition = mainBug.transform.position;
    }

    public void MoveBug(Vector3 positionBug,Action OnCompalte) {
        if(tweenMove != null && tweenMove.IsPlaying()) {
            return;
        }
        tweenMove = mainBug.transform.DOMove(positionBug,0.25f).OnComplete(()=> {
            if(Vector2.Distance(mainBug.transform.position,pointTaget.position)<=0.001f) {
                this.OnComplate?.Invoke();
                return;
            }
            OnCompalte?.Invoke(); 
        });
    }
}
