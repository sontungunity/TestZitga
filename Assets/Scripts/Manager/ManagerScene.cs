using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ManagerScene : SingletonBlin<ManagerScene>
{
    [SerializeField] private ManagerHome manHome;
    [SerializeField] private ManagerGame manGame;
    protected override void Awake() {
        base.Awake();
       
    }

    private void Start() {
        manHome.Init();
        manGame.Init();

        //Defaul show home
        OnShowHome();
    }

    public void OnShowInGame(int level) {
        manHome.gameObject.SetActive(false);
        manGame.gameObject.SetActive(true);

        manGame.Show(level);

    }

    public void OnShowHome() {
        manHome.gameObject.SetActive(true);
        manGame.gameObject.SetActive(false);
        InGameManager.Instance.Hide();
        manHome.Show();
    }

}

[Serializable]
public abstract class SingletonBlin<T> : MonoBehaviour where T : SingletonBlin<T> {
    private static T instance;

    public static bool HasInstance => instance != null;


    public static T Instance {
        get {
            if(instance == null) {
                instance = FindObjectOfType<T>() ;
                if(instance == null) {
                    Debug.LogErrorFormat("[SINGLETON] Class {0} must be added to scene before run!", typeof(T));
                }
            }
            return instance;
        }
    }

    protected virtual void Awake() {
        if(instance == null) {
            instance = this as T;
        }
        else if(instance != this) {
            Debug.LogWarningFormat("[SINGLETON] Class {0} is initialized multiple times!", typeof(T));
            Destroy(this.gameObject);
        }
    }

    protected  void OnDestroy() {
        instance = null;
    }
}
