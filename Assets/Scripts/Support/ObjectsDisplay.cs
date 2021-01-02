using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsDisplay : MonoBehaviour
{
    [SerializeField] private List<GameObject> lstGameObject;
    private int indexSelect = 0;

    public void Init(int indexSelect = 0) {
        this.indexSelect = indexSelect;
    }

    public void Show() {
        for(int i = 0; i < lstGameObject.Count; i++) {
            lstGameObject[i].SetActive(i == indexSelect);
        }
    }

    public void SetIndexSelect(int indexSelect) {
        this.indexSelect = indexSelect;
        Show();
    }

    public void SetIndexSelect(params int[] indexSelectArray) {
        for(int i = 0; i < lstGameObject.Count; i++) {
            lstGameObject[i].SetActive(false);
            foreach(int indexSelect in indexSelectArray) {
                if(i == indexSelect) {
                    lstGameObject[i].SetActive(true);
                    break;
                }
            }
            
        }
    }
}
