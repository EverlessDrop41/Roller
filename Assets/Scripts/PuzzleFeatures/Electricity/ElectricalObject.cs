using UnityEngine;
using System.Collections;

public abstract class ElectricalObject : MonoBehaviour, IElectricalItem
{
    public abstract bool IsOutputting();
}
