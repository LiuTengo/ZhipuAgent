using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject name;
    public float moveSpeed = 10.0f;
    public bool movable;

    private void Start()
    {
        EnableMovement();
    }

    private void Update()
    {
        if (!movable) return;
        
        Vector3 h = Vector3.zero;
        if (Input.GetKey(KeyCode.A))
        {
            h = Vector3.left;
            transform.localScale = new Vector3(-1,1,1);
            name.transform.localScale = new Vector3(-1,1,1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            h = Vector3.right;
            transform.localScale = new Vector3(1,1,1);
            name.transform.localScale = new Vector3(1,1,1);
        }

        var delta = h * (Time.deltaTime * moveSpeed);
        transform.position += delta;
    }

    public void DisableMovement()
    {
        movable = false;
    }

    public void EnableMovement()
    {
        movable = true;
    }
}
