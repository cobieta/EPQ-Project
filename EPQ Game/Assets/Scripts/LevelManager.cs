using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour {

	public float autoLoadLevelAfter;
	public int currentLoadedSceneIndex;

	private PlayerMovementRecorder movesRecorder;
	private ScreenFader fader;
	private string levelName;

	void Awake () {
		currentLoadedSceneIndex = SceneManager.GetActiveScene().buildIndex;
		movesRecorder = GameObject.FindObjectOfType<PlayerMovementRecorder>();
		fader = GameObject.FindObjectOfType<ScreenFader>();
		if (currentLoadedSceneIndex > 1 && currentLoadedSceneIndex < 5) {
			movesRecorder.recordingMoves = true;
			movesRecorder.ChangePosition();
		}
	}
	
	void Start () {
		if (autoLoadLevelAfter <= 0) {
			//Debug.Log("Autoload disabled, use a positive number of seconds.");
		} else {
			Invoke("LoadNextLevel", autoLoadLevelAfter);
		}
	}

	public void LoadLevel(string name){
		Debug.Log ("New Level load: " + name);
		SceneManager.LoadScene(name);
	}

	public void ReLoadLevel () {
		SceneManager.LoadScene(currentLoadedSceneIndex);
	}

	public void QuitRequest(){
		Debug.Log ("Quit requested");
		Application.Quit ();
	}
	
	public void LoadNextLevel () {
		movesRecorder.numberOfDeaths = 0;
		movesRecorder.reAllocateArrays();
		SceneManager.LoadScene(currentLoadedSceneIndex+1);
	}

	public void LoadNextAfterFade () {
		fader.FadeOut();
		Invoke("LoadNextLevel", 1f);
	}

	public void LoadAfterFade (string name) {
		fader.FadeOut();
		levelName = name;
		Invoke("LoadLevelFadeName", 1f);
	}

	void LoadLevelFadeName () {
		Debug.Log ("New Level load: " + levelName);
		SceneManager.LoadScene(levelName);
	}

}
