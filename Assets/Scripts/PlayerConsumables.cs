using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerConsumables : NetworkBehaviour {
    public static PlayerConsumables Instance { get; private set; }
    [SerializeField] private int plankCount=0;
    [SerializeField] private int nailCount=1;
    [SerializeField] private int playerHealth = 3;
    [SerializeField] private int maxPlayerHealth = 3;
    [SerializeField] private int availableSkillShots = 1;
    [SerializeField] private int maxSkillShots = 1;

    private void Awake() {
        Instance = this;
    }
    public void Start() {
        SceneManager.sceneLoaded += this.OnLoadCallback;
        
    }

    private void OnLoadCallback(Scene arg0, LoadSceneMode arg1) {
        InGameUI.Instance.SetPlankCount(plankCount);
        InGameUI.Instance.SetNailCount(nailCount);
    }

    public void AddPlank() {
        plankCount++;
        InGameUI.Instance.SetPlankCount(plankCount);
    }
    public bool RemovePlank() {
        if (plankCount > 0) {
            plankCount--;
            InGameUI.Instance.SetPlankCount(plankCount);
            return true;
        }
        return false;
    }
    public int GetPlankCount() {
        return plankCount;
    }
    public void AddNail() {
        nailCount++;
        InGameUI.Instance.SetNailCount(nailCount);
    }

    public bool RemoveNail() {
        if (nailCount > 0) {
            nailCount--;
            InGameUI.Instance.SetNailCount(nailCount);
            return true;
        }
        return false;
    }
    public int GetNailCount() {
        return nailCount;
    }


    public bool AddHealth() {
        if (playerHealth < maxPlayerHealth) {
            GameMultiplayer.Instance.ChangePlayerHealth(1);
            playerHealth++;
            return true;
        }
        return false;
    }
    public bool RemoveHealth() {
        if (playerHealth > 1 ) {
            GameMultiplayer.Instance.ChangePlayerHealth(-1);
            playerHealth--;
            return true;
        }
        playerHealth = 0;
        return false;
    }
    public int GetPlayerHealtht() {
        return playerHealth;
    }

    public bool AddSkillShot() {
        if (availableSkillShots < maxSkillShots) {
            availableSkillShots++;
            return true;
        }
        return false;
    }
    public bool RemoveSkillShot() {
        if (availableSkillShots > 0) {
            availableSkillShots--;
            return true;
        }
        return false;
    }
    public int GetSkillShot() {
        return availableSkillShots;
    }



}

