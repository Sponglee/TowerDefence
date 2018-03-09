using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGrndMusic : MonoBehaviour {
    [SerializeField]
    private GameObject backgroundMusic;
    public static AudioClip bossFight;
    // Use this for initialization
    void Start () {
        bossFight = Resources.Load<AudioClip>("bossFight");
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
