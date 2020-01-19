using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayColor : MonoBehaviour
{
    public GameObject[] Colors;

    // Start is called before the first frame update
    void Start()
    {
        Colors[new System.Random().Next(0, Colors.Length)].SetActive(true);
    }

}
