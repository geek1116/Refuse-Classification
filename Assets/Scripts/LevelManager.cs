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

    public Text titleText;

    public static LevelManager instance;
    private GarbageManager garbageManager;
    private Cat cat;

    // Level Config
    private int level = 1;
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

    private List<string> notes; // 记录垃圾分类失败后的语句

    void Awake() {
        if(instance == null)
        {
            instance = this;
        }
    }

    void OnEnable()
    {
        timer = 0f;
        levelInit = GetComponent<LevelInit>();
        cat = catPrefab.GetComponent<Cat>();
        needGenerateGarbage = true;
        SetLevelConfig(level);
        garbageManager = new GarbageManager();
        usedProp = 0;
        notes = new List<string>();
    }

    public void SetNeedGenerateGarbage(bool need)
    {
        needGenerateGarbage = need;
    }

    // Update is called once per frame
    void Update()
    {
        if(!needGenerateGarbage) return;
        CalCountSum();
        if(countSum > 0) GenerateGarbage();
        
        if(garbageManager.IsEmpty() && levelInit.GetGamingStar() > 0 && countSum <= 0) // 通过关卡
        {
            int star = levelInit.GetGamingStar();
            int gold = star * 50 + level * 10;
            GameData.playerData.AddGold(gold); // 通过后获得金币奖励
            GameData.playerData.SetLevelStar(level, star); // 通过后设置星级
            GameData.playerData.AddHandbook(map.GetGarbageCodes()); // 通过后将图鉴添加到玩家数据
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
    }

    public bool HadUsedProp()
    {
        if(usedProp > 0) return true;
        return false;
    }

    public void AddNotes(GarbageData garbageData)
    {
        string note = garbageData.ToString();
        if(!notes.Contains(note)) notes.Add(note);
    }

    public List<string> GetNotes()
    {
        return notes;
    }

    #region Garbage Generation
    private void GenerateGarbage()
    {
        timer -= Time.deltaTime;
        if(timer < 0.0f)
        {
            int code = GetRandGarbageCode();
            GarbageData garbageData = GameData.config.GetGarbageData(code);
            GameObject garbage = Instantiate(garbagePrefab, arrPath[0], Quaternion.identity);
            garbage.GetComponent<Garbage>().Reset(garbageData);

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
}
