using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTest : MonoBehaviour
{
    [Button]
    private void PlayScreen()
    {
        GetComponent<Animation>().Play();
    }
}
