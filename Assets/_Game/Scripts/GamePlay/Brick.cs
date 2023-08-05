using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    private bool isCollect = false;

    public GameObject brick;
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && !isCollect)
        {
            //Debug.Log("a");
            isCollect = true;
            brick.SetActive(false);
            other.GetComponent<Player>().AddBrick();
        }
    }
}
