using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    #region 人物轉向
    //將RAYCASTHIT的射線打到的所有模型存放在此陣列中
    RaycastHit[] hits;
    //從陣列中找尋要的物件
    RaycastHit hit;
    //滑鼠點擊的座標點
    Vector3 targetPos;
    //玩家要看的方向位置
    Vector3 Lookpos;
    #endregion

    #region 法術物件
    [Header("法術物件")]
    public GameObject Arrow;
    [Header("法術物件生成點")]
    public GameObject ArrowPos;
    #endregion

    #region 玩家血量
    [Header("玩家血量")]
    public float TotalHP;
    float ScriptHP;
    public Image HPBar;
    #endregion

    #region 信仰大招
    [Header("信仰BAR條")]
    public Image MagicBar;
    //判斷是否已經點擊大招按鈕
    public bool CanCreateMagic;
    [Header("大招物件")]
    public GameObject MagicObject;
    //儲存動態生成出來大絕招物件
    GameObject MagicObjectPrefab;
    #endregion


    private void Start()
    {
        ScriptHP = TotalHP;
    }

    private void Update()
    {
        //持續按下滑鼠左鍵，玩家會持續面相滑鼠點擊的位置
        if (Input.GetMouseButton(0))
        {
            //滑鼠點擊的地方轉換成遊戲內的三維座標後，此座標點與main camera兩點連為一線，成為射線
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //射線從main camera發射，長度為100單位，將射線打到的所有3D物件存放在hits陣列中
            hits = Physics.RaycastAll(Camera.main.transform.position, ray.direction, 100);
            //用for迴圈檢測打到的物件中是否有地板物件
            for (int i = 0; i < hits.Length; i++)
            {
                hit = hits[i];
                if (hit.collider.name == "floor")
                {
                    //將射線打到的位置點代數targetpos，只要記錄xz平面的座標值
                    targetPos = new Vector3(hit.point.x, transform.position.y, hit.point.z);
                    if (!CanCreateMagic)
                    {
                        //使用數學內插法，讓玩家從A點慢慢轉向至B點，如果沒有使用內插法，玩家會立刻執行轉向
                        Lookpos = Vector3.Lerp(Lookpos, targetPos, Time.deltaTime * 10);
                        //讓玩家注視內插法的座標點
                        transform.LookAt(Lookpos);
                        //玩家進行攻擊
                        GetComponent<Animator>().SetBool("att", true);
                    }
                    //如果按下大招按鈕
                    else
                    {
                        //目前場景上沒有大招物件
                        if (MagicObjectPrefab == null)
                            //動態生成大招物件
                            MagicObjectPrefab = Instantiate(MagicObject) as GameObject;
                        //如果Dragon的重力沒有被開啟才可以移動
                        if(!MagicObjectPrefab.GetComponentInChildren<Rigidbody>().useGravity)
                        //大招物件位置 - 滑鼠位置
                        MagicObjectPrefab.transform.position = targetPos;
                    }
                }             
            }
        }

        else
        {
            GetComponent<Animator>().SetBool("att", false);
        }
        if (Input.GetMouseButtonUp(0) && CanCreateMagic)
        {
            MagicObjectPrefab.GetComponentInChildren<Rigidbody>().useGravity = true;
        }
    }

    public void CreatArrow()
    {
        Instantiate(Arrow, ArrowPos.transform.position, ArrowPos.transform.rotation);
    }

    public void HurtPlayerHP(float Hurt)
    {
        ScriptHP -= Hurt;
        HPBar.fillAmount = ScriptHP / TotalHP;
        //如果玩家死亡，遊戲失敗
        if (HPBar.fillAmount == 0)
        {
            GameObject.Find("GM").GetComponent<GM>().isWin = false;
            GameObject.Find("GM").GetComponent<GM>().GameOver(0);
        }
    }

    public void PointerDownMagic()
    {
        if (MagicBar.fillAmount == 1)
        {
            CanCreateMagic = true;
        }
    }
}
