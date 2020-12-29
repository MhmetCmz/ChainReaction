    using UnityEngine;

public class fanCtrl : MonoBehaviour
{
    public float windpower;
    void OnTriggerStay(Collider coll)
    {
        if (coll.CompareTag("Ball")||coll.CompareTag("torch") || coll.CompareTag("Bomb"))
        {
            coll.gameObject.GetComponent<Rigidbody>().AddForce(transform.right * windpower);
        }
    }
}
