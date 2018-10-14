using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour {

    public Transform canvas;
    public int InitEnemyHealth;
    public int InitMyHealth;
    GameObject canvasPart;
    GameObject EnemyUIPart;
    GameObject FellowUIPart;
    int enemyHealth;
    int myHealth;
    float emenyAttackSpeed = 1.5f;
    float myAttackSpeed = 1f;
    private float mUpdateEnemySeconds = 0f;
    private float mUpdateMySeconds = 0f;
    Text EnemyName;
    Text EnemyHealth;
    Text EnemyHealthText;
    Slider EnemyHealthSlider;
    Text EnemyTimeLeft;
    Text MyName;
    Text MyHealth;
    Text MyHealthText;
    Slider MyHealthSlider;
    Text MyTimeLeft;


    // Use this for initialization
    void Start () {
        canvasPart = canvas.gameObject;
        EnemyUIPart = FindElementGo(canvasPart,"Game/EnemyUI");
        FellowUIPart = FindElementGo(canvasPart, "Game/FellowUI");
        InitUIPart();
    }

    // Update is called once per frame
    void Update () {
        if(myHealth > 0 && enemyHealth > 0)
        {
            mUpdateEnemySeconds += Time.deltaTime;
            mUpdateMySeconds += Time.deltaTime;
            if (mUpdateEnemySeconds >= emenyAttackSpeed)
            {
                EnemyDoAttack();
                mUpdateEnemySeconds = 0;
            }
            if (mUpdateMySeconds >= myAttackSpeed)
            {
                MeDoAttack();
                mUpdateMySeconds = 0;
            }
            FillUIPart();
        }
        else
        {
            EndGame();
        }
	}
    void InitUIPart()
    {
        GameObject Cover = FindElementGo(canvasPart, "Game/Cover");
        EnemyName = FindElementGo(EnemyUIPart, "GrpFrame/List/Name").GetComponent<Text>();
        EnemyHealth = FindElementGo(EnemyUIPart, "GrpFrame/List/Health").GetComponent<Text>();
        EnemyHealthText = FindElementGo(EnemyUIPart, "EnemyHealth/HealthNum").GetComponent<Text>();
        EnemyHealthSlider = FindElementGo(EnemyUIPart, "EnemyHealth").GetComponent<Slider>();
        EnemyTimeLeft = FindElementGo(EnemyUIPart, "GrpFrame/List/TimeLeft").GetComponent<Text>();
        MyName = FindElementGo(FellowUIPart, "GrpFrame/List/Name").GetComponent<Text>();
        MyHealth = FindElementGo(FellowUIPart, "GrpFrame/List/Health").GetComponent<Text>();
        MyHealthText = FindElementGo(FellowUIPart, "MyHealth/HealthNum").GetComponent<Text>();
        MyHealthSlider = FindElementGo(FellowUIPart, "MyHealth").GetComponent<Slider>();
        MyTimeLeft = FindElementGo(FellowUIPart, "GrpFrame/List/TimeLeft").GetComponent<Text>();
        enemyHealth = InitEnemyHealth;
        myHealth = InitMyHealth;
        EnemyName.text = "呂布";
        EnemyHealth.text = enemyHealth.ToString();
        EnemyHealthText.text = enemyHealth + "/" + InitEnemyHealth;
        EnemyHealthSlider.value = enemyHealth / (float)InitEnemyHealth;
        EnemyTimeLeft.text = emenyAttackSpeed.ToString();
        MyName.text = "刘备";
        MyHealth.text = myHealth.ToString();
        MyHealthText.text = myHealth + "/" + InitMyHealth;
        MyHealthSlider.value = myHealth / (float)InitMyHealth;
        MyTimeLeft.text = myAttackSpeed.ToString();
        Cover.GetComponent<Button>().onClick.AddListener(() =>
        {
            SceneManager.LoadScene("init");
        });
    }
    void FillUIPart()
    {
        EnemyHealth.text = enemyHealth.ToString();
        EnemyHealthText.text = enemyHealth + "/" + InitEnemyHealth;
        EnemyHealthSlider.value = enemyHealth / (float)InitEnemyHealth;
        EnemyTimeLeft.text = emenyAttackSpeed.ToString();
        MyHealth.text = myHealth.ToString();
        MyHealthText.text = myHealth + "/" + InitMyHealth;
        MyHealthSlider.value = myHealth / (float)InitMyHealth;
        MyTimeLeft.text = myAttackSpeed.ToString();
    }
    void EnemyDoAttack()
    {
        myHealth = myHealth - 10;
    }

    void MeDoAttack()
    {
        enemyHealth = enemyHealth - 10;
    }
    void EndGame()
    {
        GameObject Cover = FindElementGo(canvasPart, "Game/Cover");
        if (myHealth <= 0)
        {
            GameObject EnemyWin = FindElementGo(canvasPart, "Game/EnemyWin");
            EnemyWin.SetActive(true);
        }
        else
        {
            GameObject MeWin = FindElementGo(canvasPart, "Game/MeWin");
            MeWin.SetActive(true);
        }
        Cover.SetActive(true);
    }
    public GameObject FindElementGo(GameObject go, string name)
    {
        if (go == null) return null;
        GameObject subGo = null;
        Transform trans = go.transform.Find(name);
        if (trans != null) subGo = trans.gameObject;
        return subGo;
    }
}
