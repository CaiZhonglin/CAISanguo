using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour {

    public Transform canvas;
    public int InitEnemyHealth;
    public int InitMyHealth;
    public int InitEnemyAttack;
    public int InitMyAttack;
    public float emenyAttackSpeed = 1.5f;
    public float[] myAttackSpeed;
    GameObject canvasPart;
    GameObject EnemyUIPart;
    GameObject FellowUIPart;
    int enemyHealth;
    int myHealth;
    private float mUpdateEnemySeconds = 0f;
    private float[] mUpdateMySeconds;
    Text EnemyName;
    Text EnemyHealth;
    Text EnemyHealthText;
    Slider EnemyHealthSlider;
    Slider MyHealthSlider;
    Slider EnemySkillSlider;
    List<Slider> FellowSkillSlider;
    Text EnemyTimeLeft;
    List<Text> MyName;
    List<Text> MyHealth;
    Text MyHealthText;
    List<Text> MyTimeLeft;
    List<Text> MyBattleInfo;
    BaseGameInfo baseGame;
    
    int fellowNum;

    // Use this for initialization
    void Start () {
        canvasPart = canvas.gameObject;
        EnemyUIPart = FindElementGo(canvasPart,"Game/EnemyUI");
        FellowUIPart = FindElementGo(canvasPart, "Game/FellowUI");
        fellowNum = myAttackSpeed.Length;
        mUpdateMySeconds = new float[fellowNum];
        baseGame = new BaseGameInfo();
        InitUIPart();
    }

    // Update is called once per frame
    void Update () {
        if(myHealth > 0 && enemyHealth > 0)
        {
            mUpdateEnemySeconds += Time.deltaTime;
            if (mUpdateEnemySeconds >= emenyAttackSpeed)
            {
                EnemyDoAttack();
                mUpdateEnemySeconds = 0;
            }
            for(int i = 0; i < fellowNum; i++)
            {
                mUpdateMySeconds[i] += Time.deltaTime;
                if (mUpdateMySeconds[i] >= myAttackSpeed[i])
                {
                    MeDoAttack(i);
                    mUpdateMySeconds[i] = 0;
                }
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
        EnemySkillSlider = FindElementGo(EnemyUIPart, "EnemySkill").GetComponent<Slider>();
        MyName = new List<Text>();
        MyHealth = new List<Text>();
        MyTimeLeft = new List<Text>();
        MyBattleInfo = new List<Text>();
        FellowSkillSlider = new List<Slider>();
        for (int i = 0; i < fellowNum; i++)
        {
            string name = "FellowUI" + (i + 1);
            GameObject FellowUI = FindElementGo(FellowUIPart, name);
            FellowUI.SetActive(true);
            MyName.Add(FindElementGo(FellowUIPart, name+"/List/Name").GetComponent<Text>());
            MyHealth.Add(FindElementGo(FellowUIPart, name + "/List/Health").GetComponent<Text>());
            MyTimeLeft.Add(FindElementGo(FellowUIPart, name + "/List/TimeLeft").GetComponent<Text>());
            MyBattleInfo.Add(FindElementGo(FellowUIPart, name + "/List/BattleInfo").GetComponent<Text>());
            FellowSkillSlider.Add(FindElementGo(FellowUIPart, "FellowSkill"+(i+1)).GetComponent<Slider>());
            MyName[i].text = "刘备"+i;
            MyHealth[i].text = myHealth.ToString();
            MyTimeLeft[i].text = myAttackSpeed[0].ToString();
        }
        if (fellowNum < 5)
        {
            for(int i = fellowNum; i < 5; i++)
            {
                GameObject FellowUI = FindElementGo(FellowUIPart, "FellowUI" + (i + 1));
                GameObject SkillUI = FindElementGo(FellowUIPart, "FellowSkill" + (i + 1));
                FellowUI.SetActive(false);
                SkillUI.SetActive(false);
            }
        }
        MyHealthText = FindElementGo(FellowUIPart, "MyHealth/HealthNum").GetComponent<Text>();
        MyHealthSlider = FindElementGo(FellowUIPart, "MyHealth").GetComponent<Slider>();
        
        enemyHealth = baseGame.EnemyLv * baseGame.EnemyLv * baseGame.baseEnemyBlood;
        myHealth = InitMyHealth;
        EnemyName.text = "呂布";
        EnemyHealth.text = enemyHealth.ToString();
        EnemyHealthText.text = enemyHealth + "/" + baseGame.EnemyLv * baseGame.EnemyLv * baseGame.baseEnemyBlood;
        EnemyHealthSlider.value = enemyHealth / (float)(baseGame.EnemyLv * baseGame.EnemyLv * baseGame.baseEnemyBlood);
        EnemyTimeLeft.text = emenyAttackSpeed.ToString();
        
        MyHealthText.text = myHealth + "/" + InitMyHealth;
        MyHealthSlider.value = myHealth / (float)InitMyHealth;
        Cover.GetComponent<Button>().onClick.AddListener(() =>
        {
            if(myHealth <= 0)
            {
                baseGame.goToBeforeLevel();
            }
            else
            {
                baseGame.goToNextLevel();
            }
            SceneManager.LoadScene("Game");
        });
    }
    void FillUIPart()
    {
        EnemyHealth.text = enemyHealth.ToString();
        EnemyHealthText.text = enemyHealth + "/" + baseGame.EnemyLv * baseGame.EnemyLv * baseGame.baseEnemyBlood;
        EnemyHealthSlider.value = enemyHealth / (float)(baseGame.EnemyLv * baseGame.EnemyLv * baseGame.baseEnemyBlood);
        EnemyTimeLeft.text = emenyAttackSpeed.ToString();
        EnemySkillSlider.value = (emenyAttackSpeed - mUpdateEnemySeconds) / emenyAttackSpeed;
        EnemySkillSlider.gameObject.GetComponentInChildren<Text>().text = "巨龙吐息";
        for (int i = 0; i < fellowNum; i++)
        {
            MyHealth[i].text = myHealth.ToString();
            MyTimeLeft[i].text = mUpdateMySeconds[i].ToString();
            FellowSkillSlider[i].value = (myAttackSpeed[i] - mUpdateMySeconds[i]) / myAttackSpeed[i];
            FellowSkillSlider[i].gameObject.GetComponentInChildren<Text>().text = "普攻";
        }
        MyHealthText.text = myHealth + "/" + InitMyHealth;
        MyHealthSlider.value = myHealth / (float)InitMyHealth;
    }
    void EnemyDoAttack()
    {
        myHealth = myHealth - 10;
    }

    void MeDoAttack(int i)
    {
        enemyHealth = enemyHealth - 10;
        MyBattleInfo[i].text = MyBattleInfo[i].text + "/n造成了10点伤害";
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
