using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameInfo : MonoBehaviour {

    private static BaseGameInfo m_Instance;
    public static BaseGameInfo Instance
    {
        get
        {
            if (m_Instance == null)
                m_Instance = new BaseGameInfo();
            return m_Instance;
        }
    }
    public int baseEnemyBlood = 100;
    public int EnemyBlood;
    private int enemyLv;
    public int EnemyLv
    {
        get { return PlayerPrefs.GetInt("level",1);}
        set { PlayerPrefs.SetInt("level", value); }
    }
    // Use this for initialization
    void Start () {
		
	}

    public void goToBeforeLevel()
    {
        EnemyLv = EnemyLv - 1;
    }

    public void goToNextLevel()
    {
        EnemyLv = EnemyLv + 1;
    }

	// Update is called once per frame
	void Update () {
		
	}
}
