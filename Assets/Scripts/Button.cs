using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Button : MonoBehaviour
{
    public Vector3 ButtonPressedChange = new Vector3(0, -0.5f);

    public float ResetDelay = 1f;

    Vector3 OriginPos;
    Vector3 DownPos;

    public void Start()
    {
        OriginPos = transform.position;
        DownPos = OriginPos + ButtonPressedChange;
    }

    public bool colliding = false;

    public void OnCollisionStay(Collision collision)
    {
        colliding = true;
        if (collision.contacts.Length > 0)
        {
            ContactPoint contact = collision.contacts[0];
            Debug.Log(contact.normal);
            if (contact.normal == new Vector3(0, -1, 0))
            {
                transform.position = DownPos;
            }
        }
    }

    public void OnCollisionExit(Collision collision)
    {
        colliding = false;
        Invoke("ResetButton", ResetDelay);
    }

    public void ResetButton()
    {
        if (!colliding)
        {
            transform.position = OriginPos;
        }
    }
}
