using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedVisual : MonoBehaviour
{
    [SerializeField] private SelectableObject baseObject;
    [SerializeField] private GameObject[] visualGameObjectArray;
    private void Start() {
        if (Player.LocalInstance != null) { 
            Player.LocalInstance.OnSelectedObjectChanged += Instance_OnSelectedObjectChanged;
        }
        else {
            Player.OnAnyPlayerSpawned += Player_OnAnyPlayerSpawned;
        }
    }

    private void Player_OnAnyPlayerSpawned(object sender, System.EventArgs e) {
        if (Player.LocalInstance != null) {
            Player.LocalInstance.OnSelectedObjectChanged -= Instance_OnSelectedObjectChanged;
            Player.LocalInstance.OnSelectedObjectChanged += Instance_OnSelectedObjectChanged;
        }
    }

    private void Instance_OnSelectedObjectChanged(object sender, Player.OnSelectedElementChangedEventArgs e) { 
        if (e.selectedElement == baseObject) {
            Show();
        }
        else {
            Hide();
        }
    }

    private void Show() {
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(true);
        }
    }
    private void Hide() {
        foreach (GameObject gameObject in visualGameObjectArray) {
            gameObject.SetActive(false);
        }
    }
}
