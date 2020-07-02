using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    #region 生成怪物
    [Header("怪物")]
    public GameObject NPC;
    [Header("怪物生成點")]
    public GameObject CreatPos;
    [Header("NPC產出頻率(秒)")]
    public float WaitTime;
    [Header("每關最多怪物數量")]
    public int MaxNum;
    //目前在場景內已經產出多少數量
    int Num;
    [Header("Boss")]
    public GameObject King;
    public float DeadNum;
    //怪物條
    public Image MonsterBar;
    #endregion
    [Header("目前的關卡")]
    public string LevelIDString;

    int LevelID;
    [Header("關卡個位數與十位數的位置")]
    public Image[] LevelImage;
    [Header("0~9數字圖")]
    public Sprite[] NumberSprite;

    // 遊戲暫停畫面
    public GameObject PauseObject;

    [Header("信仰條")]
    public Image MagicBar;
    [Header("信仰條全滿時間點")]
    public float MagicTime;
    public float MagicTimeScript;

    private void Start()
    {
        //恢復整體遊戲時間
        Time.timeScale = 1;
        InvokeRepeating("CreatNpc", WaitTime, WaitTime);

        #region 第一種 - 計算LEVEL數值與圖示
        /*
        //將字串轉化成數值
        LevelID = int.Parse(LevelIDString);
        //十位數
        int a = LevelID / 10;
        //個位數
        int b = LevelID % 10;
        //個位數圖片
        LevelImage[0].sprite = NumberSprite[b];
        //十位數圖片
        LevelImage[1].sprite = NumberSprite[a];
        */
        #endregion

        #region 第二種 - 計算LEVEL數值與圖示
        /*
        //將字串裡的數值切割直接存在字串陣列
        string[] TotalLevelIDString = LevelIDString.Split('_');
        //將字串陣列裡面的文字轉換成數值帶入圖片
        LevelImage[0].sprite = NumberSprite[int.Parse(TotalLevelIDString[1])];
        LevelImage[1].sprite = NumberSprite[int.Parse(TotalLevelIDString[0])];
        */
        #endregion

        #region 第三種 - 計算LEVEL數值與圖示
        LevelImage[0].sprite = NumberSprite[int.Parse(LevelIDString.Substring(1, 1))];
        LevelImage[1].sprite = NumberSprite[int.Parse(LevelIDString.Substring(0, 1))];
        #endregion
    }

    private void Update()
    {
        //信仰條隨時間增加
        MagicTimeScript += Time.deltaTime;
        //信仰條時間介於0與MagicTime之間
        MagicTimeScript = Mathf.Clamp(MagicTimeScript, 0, MagicTime);
        //將信仰條的時間反應在MagicBar上
        MagicBar.fillAmount = MagicTimeScript / MagicTime;
    }

    void CreatNpc()
    {
        if (Num < MaxNum)
        {
            //抓取collider三維座標最大值
            Vector3 MaxValue = CreatPos.GetComponent<Collider>().bounds.max;
            //抓取collider三維座標最小值
            Vector3 MinValue = CreatPos.GetComponent<Collider>().bounds.min;
            //隨機抓取怪物生成點
            Vector3 RandomPos = new Vector3(Random.Range(MinValue.x, MaxValue.x), MinValue.y, MinValue.z);
            //動態生成怪物
            Instantiate(NPC, RandomPos, CreatPos.transform.rotation);
            //生成後數量+1
            Num++;
        }

        if (DeadNum == MaxNum && GameObject.FindGameObjectsWithTag("King").Length <= 0)
        {
            Instantiate(King, CreatPos.transform.position, CreatPos.transform.rotation);
        }
    }
    public void DeadCount()
    {
        DeadNum++;
        MonsterBar.fillAmount = (MaxNum - DeadNum) / MaxNum;
    }
    public void PauseGame()
    {
        //整體遊戲時間暫停
        Time.timeScale = 0;
        PauseObject.SetActive(true);
    }
    public void ReturnGame()
    {
        //恢復遊戲時間
        Time.timeScale = 1;
        PauseObject.SetActive(false);
    }
    public void BackMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }


}

