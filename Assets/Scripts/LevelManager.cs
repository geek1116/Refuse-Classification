using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    [Header("Fixed node: ")]
    public GameObject garbagePrefab;
    public GameObject background;
    public GameObject conveyor;
    public GameObject catPrefab;
    public GameObject mechanismGO;

    public Text titleText;

    public Text countdownText;

    public GameObject dialog;

    public Text dialogText;

    public GameObject arrorw;

    public static LevelManager instance;
    private GarbageManager garbageManager;
    private Cat cat;
    private Mechanism mechanism;

    // Level Config
    //private int level = 4;
    public float speed = 1.5f;//4
    private float speedCopy;
    private float intervalTime = 2.0f;//3
    private float intervalTimeCopy;
    private float timer = 0.0f;
    private Map map;
    public List<Vector3> arrPath;
    private Dictionary< int, List<int> > garbageCodes = new Dictionary< int, List<int> >();
    private List<int> counts = new List<int>();
    private int countSum;

    private LevelInit levelInit;
    private bool needGenerateGarbage;

    private int usedProp;

    private List<GarbageData> notes; // 记录垃圾分类失败后的语句
    private List<int> handbookCodes; // 记录已生产垃圾的code

    private int backgroundIndex;
    private List<Sprite> backgroundSprites;
    private SpriteRenderer conveyorSR;

    private bool isCountDown; // 每关开始时的倒计时
    private float startTime;

    private bool isGuidance;
    private bool isLevelStarting;

    void Awake() {
        if(instance == null)
        {
            instance = this;
        }
        levelInit = GetComponent<LevelInit>();
        garbageManager = new GarbageManager();
        cat = catPrefab.GetComponent<Cat>();
        mechanism = mechanismGO.GetComponent<Mechanism>();
        conveyorSR = conveyor.GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        timer = 0f;
        needGenerateGarbage = true;
        SetLevelConfig(GameData.level);
        usedProp = 0;
        notes = new List<GarbageData>();
        handbookCodes = new List<int>();
        backgroundIndex = 0;
        isCountDown = true;
        startTime = Time.time;
        countdownText.rectTransform.localScale = new Vector3(1,1,0);
        if(GameData.playerData.GetLevelCount() < GameData.level ) isGuidance = true;
        dialog.SetActive(false);
        arrorw.SetActive(false);
        isLevelStarting = true;
    }

    public void SetNeedGenerateGarbage(bool need)
    {
        needGenerateGarbage = need;
    }


    // Update is called once per frame
    void Update()
    {
        if(isCountDown)
        {
            if (isLevelStarting)
            {
                cat.OnLevelStart();
                isLevelStarting = false;
            }
            float tempTime = Time.time - startTime;
            if (tempTime <= 1f) countdownText.text = "3";
            else if (tempTime <= 2f) countdownText.text = "2";
            else if (tempTime <= 3f) countdownText.text = "1";
            else
            {
                countdownText.text = "0";
                isCountDown = false;
                countdownText.rectTransform.localScale = new Vector3(0, 0, 0);
            }
            return;
        }
        
        if(isGuidance)
        {
            if(GameData.level == 1)
            {
                GuidanceOne();
                return;
            }
        }

        if(!needGenerateGarbage) return;
        CalCountSum();
        if(countSum > 0) GenerateGarbage();
        
        if(garbageManager.IsEmpty() && levelInit.GetGamingStar() > 0 && countSum <= 0) // 通过关卡
        {
            int star = levelInit.GetGamingStar();
            int gold = star * 50 + GameData.level * 10;
            GameData.playerData.AddGold(gold); // 通过后获得金币奖励
            GameData.playerData.SetLevelStar(GameData.level, star); // 通过后设置星级
            GameData.playerData.AddHandbook(handbookCodes); // 通过后将图鉴添加到玩家数据
            GameData.playerData.WriteData(); // 保存到本地
            levelInit.HasSuccess();
        }
    }

    private void SetLevelConfig(int level)
    {
        map = GameData.config.GetMapConfig(level);
        arrPath = map.GetArrPath();
        foreach (int code in map.GetGarbageCodes())
        {
            int type = GameData.config.GetGarbageData(code).type;
            if(!garbageCodes.ContainsKey(type)) garbageCodes.Add(type, new List<int>());
            garbageCodes[type].Add(code);
        }
        counts = map.GetCount();
        CalCountSum();

        List<string> carType = map.GetMapTitle();
        string temp = "";
        foreach(string item in carType)
        {
            temp += item + " ";
        }
        titleText.text = temp;

        backgroundSprites = map.GetBackgroundImage();
        backgroundIndex = 0;
        //conveyorSR.sprite = map.GetConveyor();
        mechanism.Init(map.ExistPipe(), map.ExistPortal(), map.ExitBlowtorch());
    }

    public bool HadUsedProp()
    {
        if(usedProp > 0) return true;
        return false;
    }

    public void AddNotes(GarbageData garbageData)
    {
        if(!notes.Contains(garbageData)) notes.Add(garbageData);
    }

    public List<GarbageData> GetNotes()
    {
        return notes;
    }

    public void AddHandbookCodes(int code)
    {
        if (!handbookCodes.Contains(code)) handbookCodes.Add(code);
    }

    #region Garbage Generation

    public GameObject CreateGarbageAtPos(int code, Vector3 pos)
    {
        GarbageData garbageData = GameData.config.GetGarbageData(code);
        GameObject garbage = Instantiate(garbagePrefab, pos, Quaternion.identity);
        garbage.GetComponent<Garbage>().ResetGarbageData(garbageData);
        return garbage;
    }

    private void GenerateGarbage()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            int code = GetRandGarbageCode();
            GarbageData garbageData = GameData.config.GetGarbageData(code);
            GameObject garbage = Instantiate(garbagePrefab, arrPath[0], Quaternion.identity);
            garbage.GetComponent<Garbage>().ResetGarbageData(garbageData);

            garbageManager.AddGarbage(garbage);

            timer = intervalTime;
        }
    }

    private void CalCountSum()
    {
        countSum = 0;
        foreach (int countIndex in counts)
        {
            countSum += countIndex;
        }
    }

    private int GetRandGarbageCode()
    {
        int randNum = Random.Range(0, countSum);
        int sum = 0;
        int typeIndex = 0;

        for(int i = 0; i < counts.Count; i++)
        {
            sum += counts[i];
            if(randNum < sum)
            {
                typeIndex = i;
                break;
            }
        }

        int codeIndex = Random.Range(0, garbageCodes[typeIndex].Count);
        int code = garbageCodes[typeIndex][codeIndex];

        counts[typeIndex]--;
        if(counts[typeIndex] < 1)
        {
            garbageCodes.Remove(typeIndex);
        }

        return code;
    }
    #endregion

    #region Prop Method

    public void OnSlowDown(float _duration, float _speed)
    {
        usedProp++; // 记录是否使用过道具
        
        float cycleUsed = timer / intervalTime;
        intervalTimeCopy = intervalTime;
        intervalTime = speed / _speed * intervalTime;
        timer = intervalTime * cycleUsed;

        speedCopy = speed;
        speed = _speed;
        garbageManager.ChangeGarbagesSpeed(_speed);
        Invoke("ResetSpeed", _duration);
    }

    private void ResetSpeed()
    {
        float cycleUsed = timer / intervalTime;
        intervalTime = intervalTimeCopy;
        timer = intervalTime * cycleUsed;

        speed = speedCopy;
        garbageManager.ChangeGarbagesSpeed(speed);
    }
    
    public void OnRemind()
    {
        usedProp++; // 记录是否使用过道具
        garbageManager.RemindLastUnmatchGarbage(map.GetCarType());
    }

    public void OnEliminate()
    {
        usedProp++; // 记录是否使用过道具
        garbageManager.EliminateLastUnmatchGarbage(map.GetCarType());
    }

    #endregion

    #region Buff Method

    public void OnBuff(Garbage garbage)
    {
        switch ((GarbageData.Buff)garbage.buff)
        {
            case GarbageData.Buff.BadBoy:
                OnBadBoy(garbage.gameObject);
                break;
            case GarbageData.Buff.GreenTeaGirl:
                OnGreenTeaGirl(garbage.gameObject);
                break;
            case GarbageData.Buff.AcademicTrash:
                OnAcademicTrash(garbage.gameObject);
                break;
            case GarbageData.Buff.KeyboardMan:
                OnKeyboardMan(garbage.gameObject);
                break;
            default:
                break;
        }
    }

    private void OnBadBoy(GameObject garbage)
    {
        garbageManager.EliminateBothSizeGarbage(garbage);
    }

    private void OnGreenTeaGirl(GameObject garbage)
    {
        garbageManager.EliminateLastPerniciousGarbage();
    }

    private void OnAcademicTrash(GameObject garbage)
    {
        garbageManager.CopyBeforeGarbage(garbage);
    }

    private void OnKeyboardMan(GameObject garbage)
    {
        garbageManager.EliminateLastRecyclableGarbage();
        garbageManager.RemoveGarbage(garbage);
    }

    #endregion

    #region CatMethod

    public void OnCollectingGarbage(bool isPickedRight)
    {
        cat.CollectGarbage(isPickedRight);
    }

    public void OnSmashCat()
    {
        cat.OnSmashed();
    }

    #endregion

    #region mechanism method

    public void OnSprayShoot(GameObject garbage)
    {
        garbageManager.ChangeGarbageTypeRandomly(garbage);
    }

    public void OnPortalTrigger(GameObject garbageToTransfer, Vector2 outdoorPos, int targetPathIndex)
    {
        Vector2 targetPathNode = arrPath[targetPathIndex];

        garbageManager.PortalTransfer(garbageToTransfer, outdoorPos, targetPathIndex, targetPathNode);
    }

    public float GetGarbageDistance()
    {
        return intervalTime * speed;
    }

    #endregion

    #region Controll Scene Garbages
    public void ClearGarbages()
    {
        garbageManager.ClearGarbages();
    }

    public void RemoveGarbage(GameObject garbage)
    {
        garbageManager.RemoveGarbage(garbage);
    }

    public void ThrowGarbage(GameObject garbage)
    {
        garbageManager.ThrowGarbage(garbage);
    }
    #endregion

    #region Mixed Garbage

    public void OnSplitMixedGarbage(GameObject mixedGarbage, List<int> splitcodes)
    {
        garbageManager.SplitMixedGarbage(mixedGarbage, splitcodes);
    }

    #endregion

    #region Background

    public void OnCollectRightGarbage()
    {
        backgroundIndex++;
        if(backgroundIndex < backgroundSprites.Count)
        {
            background.GetComponent<SpriteRenderer>().sprite = backgroundSprites[backgroundIndex];
        }
    }

    #endregion

    void GuidanceOne()
    {
        float tempTime = Time.time - startTime;
        if(3f <= tempTime && tempTime <6f)
        {
            dialog.SetActive(true);
            dialogText.text = "垃圾实在太多了，\n我只能处理<color=#FF0000>以上</color>垃圾";
            arrorw.SetActive(true);
        }
        else if(6f <= tempTime && tempTime <= 9f)
        {
            dialogText.text = "你能把其他垃圾都放置到下方\n正确的垃圾桶里吗？就像这样";
        }
        else
        {
            dialog.SetActive(false);
            arrorw.SetActive(false);
            isGuidance = false;
        }
    }
}
