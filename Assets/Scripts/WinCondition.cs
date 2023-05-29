using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class WinCondition : MonoBehaviour
{
    private int maxPlayers;
    private List<FixedString64Bytes> playerIds=new List<FixedString64Bytes>();
    [SerializeField] private Loader.Scene nextScene;
    private void Start() {
        maxPlayers=NetworkManager.Singleton.ConnectedClientsIds.Count;
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged += Instance_OnPlayerDataNetworkListChanged;
    }

    private void Instance_OnPlayerDataNetworkListChanged(object sender, System.EventArgs e) {
        var players = GameMultiplayer.Instance.GetPlayerList();
        int livingPlayers = 0;
        foreach (PlayerData player in players) {
            if (player.health > 0) {
                livingPlayers++;
            }
        }
        maxPlayers = livingPlayers;
        if (playerIds.Count == maxPlayers) {
            Loader.LoadNetwork(nextScene);
        }
    }

    public void OnTriggerEnter(Collider other) {
        if (other.gameObject.tag == "Player") {
            if (other.gameObject.TryGetComponent(out Player player)) {
                FixedString64Bytes id = player.GetPlayerId();
                if(CheckIfReachedEnd(id)){
                    playerIds.Add(id);
                    if (playerIds.Count == maxPlayers) {
                        Loader.LoadNetwork(nextScene);
                    }
                }
            }
        }
    }

    private bool CheckIfReachedEnd(FixedString64Bytes id) {
        foreach (FixedString64Bytes id2 in playerIds) {
            if (id2.Equals(id)) {
                return false;
            }
        }
        return true;
    }
    private void OnDestroy() {
        GameMultiplayer.Instance.OnPlayerDataNetworkListChanged -= Instance_OnPlayerDataNetworkListChanged;
    }
}
