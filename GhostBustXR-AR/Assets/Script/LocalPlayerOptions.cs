using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class LocalPlayerOptions : MonoBehaviour
{
    public GameObject[] ToEnableIfLocal;

    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<PhotonView>().IsMine)
        {
            foreach (var item in ToEnableIfLocal)
            {
                item.SetActive(true);
            }
        }
    }

}
