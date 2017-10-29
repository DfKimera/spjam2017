using UnityEngine;

namespace Deprecated {
	public class DemoCubeController : MonoBehaviour {

		public Rigidbody body;

		public bool isFlippingH = false;
		public bool isFlippingV = false;
		public Vector3 origin;

		public int degreesRotatedH = 0;
		public int degreesRotatedV = 0;

		public int horizontalFlips = 0;
		public int verticalFlips = 0;


		public int flipSpeed = 5;

		// Use this for initialization
		void Start () {
			origin = transform.position;
			body = GetComponent<Rigidbody>();
		}
	
		// Update is called once per frame
		void Update () {

			checkInputs();
			
			handleCubeFlip();
	
		}

		bool isFlipping() {
			return isFlippingH || isFlippingV;
		}

		void checkInputs() {
			if(Input.GetKey(KeyCode.A) && !isFlipping()) {
				flipHorizontally();
			}

			if(Input.GetKey(KeyCode.S) && !isFlipping()) {
				flipVertically();
			}
		}

		void flipHorizontally() {
			if(isFlippingH) return;

			isFlippingH = true;
			degreesRotatedH = 0;
		}

		void flipVertically() {
			if(isFlippingV) return;

			isFlippingV = true;
			degreesRotatedV = 0;
		}

		void handleCubeFlip() {

			// Horizontal
			if(isFlippingH) {
				if(degreesRotatedH >= 90) {
					isFlippingH = false;

					degreesRotatedH = 0;
					origin = transform.position;

					horizontalFlips++;

					if(horizontalFlips >= 4) horizontalFlips = 0;

				} else {
					degreesRotatedH += flipSpeed;

					Vector3 face = Vector3.right;

					/*switch(horizontalFlips) {
				case 0: face = Vector3.right; break;
				case 1: face = Vector3.back; break;
				case 2: face = Vector3.left; break;
				case 3: face = Vector3.forward; break;
				}*/

					transform.RotateAround(origin + new Vector3(0, -0.5f, 0.5f), face, flipSpeed);
				}
			}
			
			// Vertical
			if(isFlippingV) {
				if(degreesRotatedV >= 90) {
					isFlippingV = false;
					degreesRotatedV = 0;
				} else {
					degreesRotatedV += flipSpeed;
					transform.RotateAround(Vector3.zero, Vector3.forward, flipSpeed);
				}
			}

		}
	}
}
