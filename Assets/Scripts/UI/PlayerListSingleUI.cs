using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerListSingleUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerNameText;
    [SerializeField] private List<Image> health;
    [SerializeField] private Sprite activeHealth;
    [SerializeField] private Sprite brokenHealth;
    private void Awake() {
       
    }
    public void SetPlayerName(string name) {
        playerNameText.text = name;
    }
    public void SetHealth(int lives) {
        for (int i = 0; i < 3; i++) {
            if (i <= lives - 1) {
                health[i].sprite = activeHealth;
            }
            else {
                health[i].sprite = brokenHealth;
            }
        }
    }
}
