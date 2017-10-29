using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteBillboardRenderer : MonoBehaviour {

	protected void Start() {
		transform.SetPositionAndRotation(transform.position, Camera.main.transform.rotation);
	}

}
