using UnityEngine;

namespace Rendering {
	public class SpriteBillboardRenderer : MonoBehaviour {

		protected void Start() {
			transform.SetPositionAndRotation(transform.position, Camera.main.transform.rotation);
		}

	}
}
