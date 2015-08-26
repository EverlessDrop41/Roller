using UnityEngine;
using System.Collections;
using System;

[RequireComponent(typeof(Collider))]
public class Button : ElectricalObject
{
    public Vector3 ButtonPressedChange = new Vector3(0, -0.5f);

    public float ResetDelay = 1f;

    Vector3 OriginPos;
    Vector3 DownPos;

    public override bool IsOutputting()
    {
        return IsActive;
    }

    public void Start()
    {
        OriginPos = transform.position;
        DownPos = OriginPos + ButtonPressedChange;
    }

    protected bool colliding = false;
    bool IsActive = false;

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
                IsActive = true;
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
            IsActive = false;
        }
        else
        {
            Invoke("ResetButton", ResetDelay);
        }
    }
}
