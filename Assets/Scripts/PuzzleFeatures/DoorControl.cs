using UnityEngine;
using System.Collections;

public class DoorControl : MonoBehaviour
{
    public Transform OpenedPosition;
    public Transform ClosedPosition;

    public ElectricalObject input;

    public float MoveSpeed = 1f;

    public bool OpenOnStart = true;

    public bool isOpen { get; private set; }

    bool isClosing, isOpening;

    bool isTransitioning
    {
        get {
            return isClosing || isOpening;
        }
    }

    void Start()
    {
        Vector3 openPos = (OpenOnStart ? OpenedPosition.position : ClosedPosition.position);
        transform.position = openPos;
    }

    void Update()
    {
        /*
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Close();
        }
        */

        if (input.IsOutputting())
        {
            Open();
        }
        else
        {
            Close();
        }

        if (isOpening)
        {
            if (!Utils.VectorRoughlyEqual(transform.position, OpenedPosition.position, 0.5f))
            {
                transform.position = Vector3.MoveTowards(transform.position, OpenedPosition.position, MoveSpeed * Time.deltaTime);
            }
            else
            {
                isOpening = false;
            }
        }
        else if (isClosing)
        {
            if (!Utils.VectorRoughlyEqual(transform.position, ClosedPosition.position, 0.5f))
            {
                transform.position = Vector3.MoveTowards(transform.position, ClosedPosition.position, MoveSpeed * Time.deltaTime);
            }
            else
            {
                isClosing = false;
            }
        }
    }

    public void ToggleOpenState()
    {
        if (!isTransitioning)
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open();
            }
        }
    }

    public void Open()
    {
        isOpening = true;
        isClosing = false;
    }

    public void Close()
    {
        isClosing = true;
        isOpening = false;
    }
}
