using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStart : MonoBehaviour {

    public Transform canvas;
    public int InitEnemyHealth;
    //public int InitMyHealth;
    public int InitEnemyAttack;
    public int InitMyAttack;
    public float emenyAttackSpeed = 1.5f;
    float[] myAttackSpeed;
    GameObject canvasPart;
    GameObject EnemyUIPart;
    GameObject FellowUIPart;
    int enemyHealth;
    int myHealth;
    int myMaxHealth;
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
    //List<Text> MyBattleInfo;
    BaseGameInfo baseGame;
    List<int> skillList;
    List<BaseGameInfo.fellow> fellowData = new List<BaseGameInfo.fellow>();
    List<BaseGameInfo.fellow.fightData> fellowFightData = new List<BaseGameInfo.fellow.fightData>();
    int fellowNum;

    // Use this for initialization
    void Start () {
        canvasPart = canvas.gameObject;
        EnemyUIPart = FindElementGo(canvasPart,"Game/EnemyUI");
        FellowUIPart = FindElementGo(canvasPart, "Game/FellowUI");
        baseGame = new BaseGameInfo();
        baseGame.saveInfo();

        fellowNum = 5;
        myAttackSpeed = new float[fellowNum];
        myHealth = 0;
        for (int i = 0; i < fellowNum; i++)
        {
            int id = i + 1;
            fellowData.Add(baseGame.fellowInfo[id]);
        }
        fellowFightData = baseGame.calculateAttribute(fellowData);
        for(int i = 0; i < fellowNum; i++)
        {
            myAttackSpeed[i] = fellowFightData[i].AttackSpeed;
        }
        mUpdateMySeconds = new float[fellowNum];
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
        skillList = new List<int>();
        MyName = new List<Text>();
        MyHealth = new List<Text>();
        MyTimeLeft = new List<Text>();
        List<Text> MyWeaponInfo = new List<Text>();
        //MyBattleInfo = new List<Text>();
        FellowSkillSlider = new List<Slider>();
        for (int i = 0; i < fellowNum; i++)
        {
            string name = "FellowUI" + (i + 1);
            BaseGameInfo.fellow fellow = fellowData[i];


            GameObject FellowUI = FindElementGo(FellowUIPart, name);
            FellowUI.SetActive(true);
            MyName.Add(FindElementGo(FellowUIPart, name+"/List/Name").GetComponent<Text>());
            MyHealth.Add(FindElementGo(FellowUIPart, name + "/List/Health").GetComponent<Text>());
            MyTimeLeft.Add(FindElementGo(FellowUIPart, name + "/List/TimeLeft").GetComponent<Text>());
            MyWeaponInfo.Add(FindElementGo(FellowUIPart, name + "/List/WeaponInfo").GetComponent<Text>());
            //MyBattleInfo.Add(FindElementGo(FellowUIPart, name + "/List/BattleInfo").GetComponent<Text>());
            FellowSkillSlider.Add(FindElementGo(FellowUIPart, "FellowSkill"+(i+1)).GetComponent<Slider>());
            int weaponId = fellowData[i].initWeapon;
            List<int> fellowSkillList = baseGame.weaponInfo[weaponId].initCard;
            int randomSkillIndex = UnityEngine.Random.Range(0, fellowSkillList.Count);
            skillList.Add(fellowSkillList[randomSkillIndex]);
            MyName[i].text = fellow.name;
            MyHealth[i].text = fellow.initHealth.ToString();
            MyTimeLeft[i].text = myAttackSpeed[i].ToString();
            MyWeaponInfo[i].text = baseGame.weaponInfo[weaponId].name;
            FellowSkillSlider[i].gameObject.GetComponentInChildren<Text>().text = baseGame.cardInfo[skillList[i]].name;

            myHealth += fellowFightData[i].Health;
        }
        myMaxHealth = myHealth;
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
        EnemyName.text = "呂布";
        EnemyHealth.text = enemyHealth.ToString();
        EnemyHealthText.text = enemyHealth + "/" + baseGame.EnemyLv * baseGame.EnemyLv * baseGame.baseEnemyBlood;
        EnemyHealthSlider.value = enemyHealth / (float)(baseGame.EnemyLv * baseGame.EnemyLv * baseGame.baseEnemyBlood);
        EnemyTimeLeft.text = emenyAttackSpeed.ToString();
        
        MyHealthText.text = myHealth + "/" + myMaxHealth;
        MyHealthSlider.value = myHealth / (float)myMaxHealth;
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
        }
        MyHealthText.text = myHealth + "/" + myMaxHealth;
        MyHealthSlider.value = myHealth / (float)myMaxHealth;
    }
    void EnemyDoAttack()
    {
        myHealth = myHealth - 10 - baseGame.EnemyLv * baseGame.EnemyLv;
    }

    void MeDoAttack(int i)
    {
        int doSkillId = skillList[i];
        int skillType = baseGame.cardInfo[doSkillId].damageType;
        int damage = 0;
        if(skillType == 1)
        {
            damage = baseGame.cardInfo[doSkillId].damageScale * fellowFightData[i].PhysicalAttack;
        }
        else if(skillType == 2)
        {
            damage = baseGame.cardInfo[doSkillId].damageScale * fellowFightData[i].MagicAttack;
        }
        else if(skillType == 5)
        {
            damage = baseGame.cardInfo[doSkillId].damageScale * Math.Max(fellowFightData[i].PhysicalAttack, fellowFightData[i].MagicAttack);
        }

        enemyHealth = enemyHealth - damage;
        int weaponId = fellowData[i].initWeapon;
        List<int> fellowSkillList = baseGame.weaponInfo[weaponId].initCard;
        int randomSkillIndex = UnityEngine.Random.Range(0, fellowSkillList.Count);
        skillList[i] = fellowSkillList[randomSkillIndex];
        FellowSkillSlider[i].gameObject.GetComponentInChildren<Text>().text = baseGame.cardInfo[skillList[i]].name;
        //MyBattleInfo[i].text = MyBattleInfo[i].text + "/n造成了10点伤害";
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
