using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    bool gameBegun = false;
    public static GameManager singleton;
    [SerializeField] GameObject playerPrefab;
    [SerializeField] Transform spawnPoint;
    private GameObject playerObject;
    private void Update() {
        if (gameBegun) {

        }
    }
    private void Start() {
        singleton = this;
    }
    public void PrepareGame() {
        UIManager.singleton.PlayAnimation("loginDismiss");
        SpawnPlayer();
    }

    public void SpawnPlayer() {
        playerObject = Instantiate(playerPrefab, spawnPoint.position, spawnPoint.rotation);
    }

    public void DismissGame() {
        Destroy(playerObject);
        
    }

    public void StartGame() {
        PlayerManager.singleton.SetControllable(true);
        CameraManager.singleton.currentCheckPoint = 1;
    }

    public void SpawnCoin() {

    }

    public void SpawnEnemy() {

    }

    
}
