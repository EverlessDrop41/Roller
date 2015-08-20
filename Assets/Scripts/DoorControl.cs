using UnityEngine;
using System.Collections;

public class DoorControl : MonoBehaviour
{
    public Transform OpenedPosition;
    public Transform ClosedPosition;

    public float MoveTime = 1f;

    public bool OpenOnStart = true;

    public bool isOpen { get; private set; }

    bool isClosing, isOpening;

    void Start()
    {
        Vector3 openPos = (OpenOnStart ? OpenedPosition.position : ClosedPosition.position);
        transform.position = openPos;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            Open();
        }

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            Close();
        }

        if (isOpening)
        {
            if (!Utils.VectorRoughlyEqual(transform.position, OpenedPosition.position, 0.5f))
            {
                transform.position = Vector3.Lerp(transform.position, OpenedPosition.position, MoveTime * Time.deltaTime);
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
                transform.position = Vector3.Lerp(transform.position, ClosedPosition.position, MoveTime * Time.deltaTime);
            }
            else
            {
                isClosing = false;
            }
        }
    }

    public void ToggleOpenState()
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
