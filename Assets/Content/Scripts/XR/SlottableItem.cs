using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlottableItem : MonoBehaviour
{
    [SerializeField]
    protected SlottableType type;
    public SlottableType Type => type;
}

public enum SlottableType
{
    SmallWeapon,
    LargeWeapon,

    HandgunMag = 100,
    RifleMag,
    HandcannonMag,
    AutoPistolMag,
}
