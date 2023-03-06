using UnityEngine;

public class Trampoline : MonoBehaviour
{
    [SerializeField] private float dropForce;

    public void Drop(GameObject gameObj)
    {
        gameObj.GetComponent<Rigidbody2D>().AddForce(Vector2.up * dropForce, ForceMode2D.Impulse);
    }
}
