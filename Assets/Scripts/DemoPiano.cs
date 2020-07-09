/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class DemoPiano : MonoBehaviour {

	[SerializeField]
	public AudioClip clip;

	private int currentSource;
	private const int SOURCE_COUNT = 4;
	private AudioSource[] sources = new AudioSource[SOURCE_COUNT];

	// Use this for initialization
	void Start() {
		for(int i = 0; i < SOURCE_COUNT; i++) {
			AudioSource source = sources[i] = gameObject.AddComponent<AudioSource>();
		}
	}

	// Update is called once per frame
	void Update() {

	}

	void OnGUI() {

		// Draw keys.

		float x = 0, y = 0, w = 30, h = 100;

		int k = -16;
		int ki = 0;

		for (int i = 0; i < 19; i++) {

			Rect rect = new Rect(x, y, w, h);

			if (GUI.Button(rect, "")) {
				float pitch = Mathf.Pow(1.05946f, k);

				AudioSource source = sources[currentSource];

				currentSource++;
				if(currentSource >= SOURCE_COUNT) {
					currentSource = 0;
				}

				source.Stop();
				source.pitch = pitch;
				source.PlayOneShot(clip);
			}

			if (ki != 3 && ki != 6 && ki != 10 && ki != 13 && ki != 17) {
				k += 2;
			} else {
				k += 1;
			}

			ki++;

			x += w;
		}
	}
}*/