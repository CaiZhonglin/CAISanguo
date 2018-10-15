using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseGameInfo : MonoBehaviour {

    public Dictionary<int, card> cardInfo;
    public Dictionary<int, equip> weaponInfo;
    public Dictionary<int, fellow> fellowInfo;

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

    public class card
    {
        public string name;
        public int skillType;
        public int damageType;
        public int damageScale;
        public int cardValue;
        public int cardId;
    }
    public class equip
    {
        public string name;
        public int physicalAttack;
        public int magicAttack;
        public List<int> initCard;
        public int equipValue;
        public int CanEquipNum;
    }
    public class fellow
    {
        public string name;
        public int initPhysicalAttack;
        public int initMagicAttack;
        public float initAttackSpeed;
        public int initPhysicalDefense;
        public int initMagicDefense;
        public int initHealth;
        public int initWeapon;
        public int fellowQuality;

        public class fightData
        {
            public int PhysicalAttack;
            public int MagicAttack;
            public float AttackSpeed;
            public int PhysicalDefense;
            public int MagicDefense;
            public int Health;
            public int Weapon;
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

    public void saveInfo()
    {
        cardInfo = new Dictionary<int, card>();
        weaponInfo = new Dictionary<int, equip>();
        fellowInfo = new Dictionary<int, fellow>();
        TextAsset info = Resources.Load<TextAsset>("config/cardInfo");
        JSONObject json = new JSONObject(info.text);
        JSONObject cardJson = new JSONObject(info.text).GetField("Card");
        JSONObject weaponJson = new JSONObject(info.text).GetField("Weapon");
        JSONObject cardListJson = new JSONObject(info.text).GetField("CardList");
        List<JSONObject> cardList = cardListJson.list;
        for(int i = 0; i < cardList.Count; i++)
        {
            card subCard = new card();
            int cardId = (int)cardList[i].n;
            JSONObject cardData = cardJson.GetField(cardId + "");
            subCard.name = cardData.GetField("name").str;
            subCard.skillType = (int)cardData.GetField("skillType").n;
            subCard.damageType = (int)cardData.GetField("damageType").n;
            subCard.damageScale = (int)cardData.GetField("damageScale").n;
            subCard.cardValue = (int)cardData.GetField("cardValue").n;
            //Debug.LogError(subCard.name + " " + subCard.skillType + " " + subCard.damageType + " " + subCard.damageScale + " " + subCard.cardValue);
            cardInfo.Add(cardId, subCard);
        }
        for(int i = 0; i < weaponJson.Count; i++)
        {
            equip subWeapon = new equip();
            int index = i + 1;
            JSONObject weaponData = weaponJson.GetField(index + "");
            subWeapon.name = weaponData.GetField("name").str;
            subWeapon.physicalAttack = (int)weaponData.GetField("physicalAttack").n;
            subWeapon.magicAttack = (int)weaponData.GetField("magicAttack").n;
            List<JSONObject> card = weaponData.GetField("initCard").list;
            subWeapon.initCard = new List<int>();
            for (int j = 0; j < card.Count; j++)
            {
                subWeapon.initCard.Add((int)card[j].n);
            }
            subWeapon.equipValue = (int)weaponData.GetField("equipValue").n;
            subWeapon.CanEquipNum = (int)weaponData.GetField("CanEquipNum").n;
            //Debug.LogError(subWeapon.name+" "+ subWeapon.physicalAttack + " " + subWeapon.magicAttack + " " + subWeapon.initCard.Count + " " + subWeapon.equipValue + " " + subWeapon.CanEquipNum);
            weaponInfo.Add(index, subWeapon);
        }
        info = Resources.Load<TextAsset>("config/fellowInfo");
        JSONObject fellowJson = new JSONObject(info.text).GetField("Fellow");
        for (int i = 0; i < fellowJson.Count; i++)
        {
            fellow subFellow = new fellow();
            int index = i + 1;
            JSONObject fellowData = fellowJson.GetField(index + "");
            subFellow.name = fellowData.GetField("name").str;
            subFellow.initPhysicalAttack = (int)fellowData.GetField("initPhysicalAttack").n;
            subFellow.initMagicAttack = (int)fellowData.GetField("initMagicAttack").n;
            subFellow.initAttackSpeed = fellowData.GetField("initAttackSpeed").f;
            subFellow.initPhysicalDefense = (int)fellowData.GetField("initPhysicalDefense").n;
            subFellow.initMagicDefense = (int)fellowData.GetField("initMagicDefense").n;
            subFellow.initHealth = (int)fellowData.GetField("initHealth").n;
            subFellow.initWeapon = (int)fellowData.GetField("initWeapon").n;
            subFellow.fellowQuality = (int)fellowData.GetField("fellowQuality").n;
            //Debug.LogError(subFellow.name + " " + subFellow.initPhysicalAttack + " " + subFellow.initMagicAttack + " " + subFellow.initPhysicalDefense + " " + subFellow.initMagicDefense + " " + subFellow.initHealth + " " + subFellow.initWeapon + " " + subFellow.fellowQuality);
            fellowInfo.Add(index, subFellow);
        }
    }
    public List<fellow.fightData> calculateAttribute(List<BaseGameInfo.fellow> fellowData)
    {
        List<fellow.fightData> fightData = new List<fellow.fightData>();
        for (int i = 0; i < fellowData.Count; i++)
        {
            fellow.fightData subFellow = new fellow.fightData();
            subFellow.PhysicalAttack = fellowData[i].initPhysicalAttack + weaponInfo[fellowData[i].initWeapon].physicalAttack;
            subFellow.MagicAttack = fellowData[i].initMagicAttack + weaponInfo[fellowData[i].initWeapon].magicAttack;
            subFellow.PhysicalDefense = fellowData[i].initPhysicalDefense;
            subFellow.MagicDefense = fellowData[i].initMagicDefense;
            subFellow.AttackSpeed = fellowData[i].initAttackSpeed;
            subFellow.Health = fellowData[i].initHealth;
            fightData.Add(subFellow);
        }
        return fightData;
    }
    // Update is called once per frame
    void Update () {
		
	}
}
