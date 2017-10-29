using UnityEngine;

namespace Controllers {
	public class BGMController : MonoBehaviour {

		public AudioSource track;
	
		public AudioClip bgmTitle;
		public AudioClip bgmMatch;

		protected void Start () {
			track = GetComponent<AudioSource>();
		}

		public void PlayTitleSong() {
			track.Stop();
			track.clip = bgmTitle;
			track.Play();
		}

		public void PlayMatchSong() {
			track.Stop();
			track.clip = bgmMatch;
			track.Play();
		}
	
	}
}
