using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class InGameUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI plankCount;
    [SerializeField] private TextMeshProUGUI nailCount;
    [SerializeField] private Transform playerContainer;
    [SerializeField] private Transform playerTemplate;
    public static InGameUI Instance { get; private set; }
    private void Awake() {
        Instance = this;
        Hide();
    }
    private void Start() {
        plankCount.text = PlayerConsumables.Instance.GetPlankCount()+"x";
        nailCount.text = PlayerConsumables.Instance.GetNailCount()+"x";
        playerTemplate.gameObject.SetActive(false);
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += InGameUI_OnPlayerDataNetworkListChanged;
        UpdatePlayerUIList();
    }

    private void InGameUI_OnPlayerDataNetworkListChanged(object sender, EventArgs e) {
        UpdatePlayerUIList();
    }

    public void UpdatePlayerUIList() {
        foreach (Transform child in playerContainer) {
            if (child == playerTemplate) continue;
            Destroy(child.gameObject);
        }

        var players = GameMultiplayer.Instance.GetPlayerList();

        foreach (PlayerData player in players) {
            Transform playerUITransform = Instantiate(playerTemplate, playerContainer);
            playerUITransform.gameObject.SetActive(true);
            playerUITransform.GetComponent<PlayerListSingleUI>().SetPlayerName(player.playerName.ToString());
            playerUITransform.GetComponent<PlayerListSingleUI>().SetHealth(player.health);
        }
    }

    public void SetPlankCount(float count) {
        plankCount.text = count.ToString() + "x";
    }

    public void SetNailCount(float count) {
        nailCount.text = count.ToString() + "x";
    }

    public void Show() {
        gameObject.SetActive(true);
    }
    private void Hide() {
        gameObject.SetActive(false);
    }

    private void OnDestroy() {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= InGameUI_OnPlayerDataNetworkListChanged;
    }

}
