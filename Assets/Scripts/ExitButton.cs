using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitButton : MonoBehaviour
{
    [SerializeField] private Button exitButton;
    private void Start() {
        exitButton.onClick.AddListener(() => {
            Application.Quit();
        });
    }
    
}
