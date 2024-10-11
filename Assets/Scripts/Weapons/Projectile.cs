using System.Collections;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] float speed = 10f;

    private void Update()
    {
        transform.Translate(Vector3.down * Time.deltaTime * speed);

        if((transform.position.x > ScreenBorderLimit.Right() || transform.position.x < ScreenBorderLimit.Left()) ||
           (transform.position.y > ScreenBorderLimit.Top() || transform.position.y < ScreenBorderLimit.Bottom()))
        {
            gameObject.SetActive(false);
        }
    }
}