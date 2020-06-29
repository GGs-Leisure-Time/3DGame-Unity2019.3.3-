using UnityEngine;

public class GM : MonoBehaviour
{
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
    public int DeadNum;

    private void Start()
    {
        InvokeRepeating("CreatNpc", WaitTime, WaitTime);
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
    }

}

