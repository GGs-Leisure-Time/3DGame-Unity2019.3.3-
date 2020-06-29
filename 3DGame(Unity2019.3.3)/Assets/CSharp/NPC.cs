using UnityEngine;

public class NPC : MonoBehaviour
{
    public float speed;
    private float ScriptSpeed;

    public float TotalHP;
    private float ScriptHP;

    private void Start()
    {
        ScriptSpeed = speed;
        ScriptHP = TotalHP;
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * Time.deltaTime * ScriptSpeed);
    }
    
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().name == "mazu_wall")
        {
            ScriptSpeed = 0;
            GetComponent<Animator>().SetBool("ATK", true);
        }
    }

    private void OnTriggerExit(Collider hit)
    {
        if (hit.GetComponent<Collider>().name == "mazu_wall")
        {
            ScriptSpeed = speed;
            GetComponent<Animator>().SetBool("ATK", false);
        }
    }

    public void HitNpc(float Hurt, float dis) 
    {
        ScriptHP -= Hurt;
        ScriptHP = Mathf.Clamp(ScriptHP, 0, TotalHP);
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z +dis);
        if (ScriptHP <= 0)
        {
            GetComponent<Animator>().SetTrigger("DEAD");
            ScriptSpeed = 0;
            GetComponent<Collider>().enabled = false;
            GameObject.Find("GM").GetComponent<GM>().DeadCount();
        }
    }
}
