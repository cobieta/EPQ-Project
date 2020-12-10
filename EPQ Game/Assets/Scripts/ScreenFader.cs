using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFader : MonoBehaviour {

	private Image fadePanel;
	private Animator anim;

	// Use this for initialization
	void Start () {
		fadePanel = GetComponent<Image>();
		anim = GetComponent<Animator>();
	}

	public void FadeIn () {
		// Fade in.
		anim.SetTrigger("FadeInTrigger");
		//fadePanel.color = Color.clear;
	}

	public void FadeOut () {
		// Fade out.
		anim.SetTrigger("FadeOutTrigger");
		//fadePanel.color = Color.black;
	}
}
