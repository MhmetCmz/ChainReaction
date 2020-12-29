using UnityEngine;

public class CrossbowCtrl : MonoBehaviour,IInteractableObject
{
    public GameObject arrowGO,gerginRopeGO,serbestRopeGO; 
    public void LevelStart() { }
    void OnCollisionEnter(Collision col)
    {
        //if (!col.collider.CompareTag("Arrow"))
        //{ 
            arrowGO.GetComponent<Rigidbody>().isKinematic = false;
            gerginRopeGO.gameObject.SetActive(false);
            serbestRopeGO.gameObject.SetActive(true);
        //}
    }
    public void SetDefault()
    {
        serbestRopeGO.gameObject.SetActive(false);
        gerginRopeGO.gameObject.SetActive(true);
    }
}
