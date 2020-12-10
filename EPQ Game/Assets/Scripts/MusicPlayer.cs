using UnityEngine;
using System.Collections;

public class MusicPlayer : MonoBehaviour {
	static MusicPlayer instance = null;
	
	public AudioClip splashClip;
	public AudioClip startMusic;
	public AudioClip gameMusic;
	public AudioClip endMusic;
	
	private AudioSource music;
	
	void Start () {
		AudioSource.PlayClipAtPoint(splashClip, transform.position);
		if (instance != null && instance != this) {
			Destroy (gameObject);
			print ("Duplicate music player self-destructing!");
		} else {
			instance = this;
			GameObject.DontDestroyOnLoad(gameObject);
			music = GetComponent<AudioSource>();
			music.clip = startMusic;
			music.loop = true;
			music.Play();
		}
	}
	
	void OnLevelWasLoaded (int level) {
		Debug.Log("MusicPlayer: loaded level" + level);
		music.Stop();
		if (level == 0) {
			music.clip = startMusic;
		} else if (level == 1) {
			music.clip = gameMusic;
		} else {
			music.clip = endMusic;
		}
		music.loop = true;
		music.Play();
	}
}
