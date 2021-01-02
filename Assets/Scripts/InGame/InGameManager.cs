using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
public class InGameManager : SingletonBlin<InGameManager>
{
    [SerializeField]private GameObject display;
    [SerializeField]private LineRenderer lineRenderer;
    [SerializeField]private Transform mainBug;
    [SerializeField]private Transform pointTaget;

    [SerializeField]private float speed = 10f; 
    
    private List<Vector3> checkpoint = new List<Vector3>();
    
    public void Show(Vector3 positionBug, Vector3 posionTarget) {
        display.SetActive(true);
        mainBug.position = positionBug;
        pointTaget.position = posionTarget;
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

        DOVirtual.DelayedCall(1f,()=> {
            mainBug.DOPath(checkpoint.ToArray(), speed, PathType.Linear, PathMode.TopDown2D)
            .SetLookAt(0.1f)
            .SetLoops(-1,LoopType.Restart);
        });
    }
}
