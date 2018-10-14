using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMgr : MonoBehaviour {

    public Button StartGame;
    // Use this for initialization

    void Start () {
        StartGame.onClick.AddListener(() => {
            Debug.Log("点击开始游戏");
            SceneManager.LoadScene("game");
        });
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
