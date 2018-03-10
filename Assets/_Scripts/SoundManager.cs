using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    public static AudioClip playerCoinSound;
    public static AudioClip playerCoinDrop;
    static AudioSource audioSrc;


	// Use this for initialization
	void Start () {
        playerCoinSound = Resources.Load<AudioClip>("coin");
        playerCoinDrop = Resources.Load<AudioClip>("coinDrop");
        audioSrc = GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            case "coin":
                audioSrc.PlayOneShot(playerCoinSound);
                break;
            case "drop":
                audioSrc.PlayOneShot(playerCoinDrop);
                break;
        }
    }
}
