using UnityEngine;

namespace Controllers {
	public class BGMController : MonoBehaviour {
	
		public AudioClip bgmTitle;
		public AudioClip bgmMatch;

		public void PlayTitleSong() {
			GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().clip = bgmTitle;
			GetComponent<AudioSource>().Play();
		}

		public void PlayMatchSong() {
			GetComponent<AudioSource>().Stop();
			GetComponent<AudioSource>().clip = bgmMatch;
			GetComponent<AudioSource>().Play();
		}
	
	}
}
