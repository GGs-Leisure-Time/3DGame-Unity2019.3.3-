using UnityEngine;

public class Arrow : MonoBehaviour
{
    public float speed;
    public float DeleteTime;

    private void Start()
    {
        Destroy(gameObject, DeleteTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().tag == "NPC")
        {
            hit.GetComponent<NPC>().HitNpc(30f, 5f);
            Destroy(gameObject);
        }

        if (hit.GetComponent<Collider>().tag == "King")
        {
            hit.GetComponent<NPC>().HitNpc(30f, 3f);
            Destroy(gameObject);
        }
    }

}
