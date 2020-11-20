using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;


public class TVControl : MonoBehaviour
{

    public VideoPlayer TV;

    private void Awake() {
        TV = GameObject.FindWithTag("TV").GetComponent<VideoPlayer>();
    }
    public void startTV() {
        TV.Play();
    }
}
