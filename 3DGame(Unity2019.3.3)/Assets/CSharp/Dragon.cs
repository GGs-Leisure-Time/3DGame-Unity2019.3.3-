using UnityEngine;

public class Dragon : MonoBehaviour
{
    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().tag == "NPC")
        {
            hit.GetComponent<NPC>().HitNpc(100f, 5f);
        }
        if (hit.GetComponent<Collider>().tag == "King")
        {
            hit.GetComponent<NPC>().HitNpc(100f, 3f);
        }
        if (hit.GetComponent<Collider>().name == "floor")
        {
            Destroy(transform.parent.gameObject);

            GameObject.FindGameObjectWithTag("Player").GetComponent<Player>().CanCreateMagic = false;
            GameObject.Find("GM").GetComponent<GM>().MagicTimeScript = 0;
        }
    }
}
