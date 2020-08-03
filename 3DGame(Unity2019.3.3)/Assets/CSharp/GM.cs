using System.Collections.Generic;
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

    #region 關卡
    [Header("目前的關卡")]
    public string LevelIDString;

    int LevelID;
    [Header("關卡個位數與十位數的位置")]
    public Image[] LevelImage;
    [Header("0~9數字圖")]
    public Sprite[] NumberSprite;

    #endregion

    // 遊戲暫停畫面
    public GameObject PauseObject;
    #region MP條
    [Header("信仰條")]
    public Image MagicBar;
    [Header("信仰條全滿時間點")]
    public float MagicTime;
    public float MagicTimeScript;
    #endregion

    #region 分數計算
    //計算玩家的分數
    int TotalScore;
    //儲存玩家的總分
    string SaveTotalScore = "SaveTotalScore";
    //將玩家的分數轉成字串
    string TotalScoreString;
    [Header("分數物件")]
    public GameObject ScoreObject;
    [Header("分數父物件")]
    public GameObject ScoreGridObject;
    public List<Image> ScoreImage;
    #endregion

    #region 遊戲結束
    //true代表勝利 false代表失敗
    public bool isWin;
    [Header("遊戲結束的UI物件")]
    public GameObject GameOverUI;
    [Header("遊戲勝利與失敗圖")]
    public Sprite WinSprite, LoseSprite;
    [Header("遊戲勝利與失敗Image")]
    public Image WinImage;
    [Header("關卡十位數與個位數")]
    public Image[] GameOverLevelImage;
    //獎勵分數
    string RewardScoreString;
    int GameOverScore;
    string GameOverScoreString;
    [Header("動態生成數字的物件-獎勵")]
    public GameObject RewardScoreObject;
    [Header("動態生成數字的父物件-獎勵")]
    public GameObject RewardScoreGridObject;
    [Header("動態生成數字的物件-遊戲結束總分")]
    public GameObject GameOverScoreObject;
    [Header("動態生成數字的父物件-遊戲結束總分")]
    public GameObject GameOverScoreGridObject;
    public List<Image> RewardScoreImage;
    public List<Image> GameOverScoreImage;

    #endregion

    #region 遊戲結束腳本
    public Button NextGameButton;

    #endregion



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

        FinalScore(0);
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
    public void FinalScore(int AddScore)
    {
        //怪物死亡加分
        TotalScore += AddScore;
        //總分數轉成字串
        TotalScoreString = TotalScore + "";
        //儲存總分數
        PlayerPrefs.SetInt(SaveTotalScore, TotalScore);
        //抓取自串總共有多少字數量 = 字串.Length
        //看list總數量再生成Image數量在空物件下
        for (int i = ScoreImage.Count; i < TotalScoreString.Length; i++)
        {
            //動態生成Image
            GameObject ScoreObjectPrefab = Instantiate(ScoreObject) as GameObject;
            //將生成出來的Image移動到空物件階層下
            ScoreObjectPrefab.transform.parent = ScoreGridObject.transform;
            //抓取動態生成物件的Image存放在List
            ScoreImage.Add(ScoreObjectPrefab.GetComponent<Image>());
        }
        //將每個Image帶入數字圖片
        for (int spritID = 0; spritID < TotalScoreString.Length; spritID++)
        {
            ScoreImage[spritID].sprite = NumberSprite[int.Parse(TotalScoreString.Substring(spritID, 1))];
        }
    }
    public void GameOver(int RewardScore)
    {
        GameOverUI.SetActive(true);
        Time.timeScale = 0;
        if (isWin)
        {
            WinImage.sprite = WinSprite;
            NextGameButton.interactable = true;
        }
        else
        {
            WinImage.sprite = LoseSprite;
            NextGameButton.interactable = false;
        }
        GameOverLevelImage[0].sprite = NumberSprite[int.Parse(LevelIDString.Substring(1, 1))];
        GameOverLevelImage[1].sprite = NumberSprite[int.Parse(LevelIDString.Substring(0, 1))];

        GameOverScore = TotalScore + RewardScore;
        GameOverScoreString = GameOverScore + "";
        RewardScoreString = RewardScore + "";
        for (int i = RewardScoreImage.Count; i < RewardScoreString.Length; i++)
        {
            GameObject RewardScorePrefab = Instantiate(ScoreObject) as GameObject;
            RewardScorePrefab.transform.parent = RewardScoreGridObject.transform;
            RewardScoreImage.Add(RewardScorePrefab.GetComponent<Image>());
        }
        for (int spritID = 0; spritID < RewardScoreString.Length; spritID++)
        {
            RewardScoreImage[spritID].sprite = NumberSprite[int.Parse(RewardScoreString.Substring(spritID, 1))];
        }
        for (int i_g = GameOverScoreImage.Count; i_g < GameOverScoreString.Length; i_g++)
        {
            GameObject GameOverScorePrefab = Instantiate(ScoreObject) as GameObject;
            GameOverScorePrefab.transform.parent = GameOverScoreGridObject.transform;
            GameOverScoreImage.Add(GameOverScorePrefab.GetComponent<Image>());
        }

        for (int spritID_g = 0; spritID_g < GameOverScoreString.Length; spritID_g++)
        {
            GameOverScoreImage[spritID_g].sprite = NumberSprite[int.Parse(GameOverScoreString.Substring(spritID_g, 1))];
        }
        //將資料寫入Excel表單
        ExcelWritter.ansList.Add(LevelIDString);
        ExcelWritter.ansList.Add(GameOverScoreString);
        //寫入Excel黨名與表單名稱
        ExcelWritter.WriteExcel("SaveData", "Data");
    }
    public void Regame()
    {
        SceneManager.LoadScene("Game");
    }
    public void Nextgame()
    {
        SceneManager.LoadScene("Game");
    }

}

