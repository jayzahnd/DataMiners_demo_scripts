using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class CameraControl : MonoBehaviour {


	GameObject objectOver;

	public bool scrolling;

	GameObject cameraHolder;

	public float scrollSpeed;
    public float scrollSpeedArrowKeys;

    public bool leftMoveCam;
    public bool rightMoveCam;
    public bool upMoveCam;
    public bool downMoveCam;

    private void Awake() {
        // binding arrow keys to scrolling

        

    }


    // Use this for initialization
    void Start () {
		
	}
	
    public void FetchCameraprefab(GameObject _mainCam) {

        cameraHolder = _mainCam;
        
    }

	// Update is called once per frame
	void Update () {
        leftMoveCam = Input.GetKey(KeyCode.LeftArrow);
        rightMoveCam = Input.GetKey(KeyCode.RightArrow);
        upMoveCam = Input.GetKey(KeyCode.UpArrow);
        downMoveCam = Input.GetKey(KeyCode.DownArrow);
    }

	void FixedUpdate () {

		if (scrolling == true) {

			if (objectOver.name == "TopLeft") {  //Top to Topleft

				cameraHolder.transform.Translate (0, 0, scrollSpeed * Time.deltaTime);

			} else if (objectOver.name == "BottomRight") { // Bottom to Bottomright

				cameraHolder.transform.Translate (0, 0, (scrollSpeed  * -1 ) * Time.deltaTime);

			} else if (objectOver.name == "BottomLeft") { // Left to Bottomleft

				cameraHolder.transform.Translate ((scrollSpeed  * -1 ) * Time.deltaTime, 0, 0);

			} else if (objectOver.name == "TopRight") { // Right to Topright

				cameraHolder.transform.Translate (scrollSpeed * Time.deltaTime, 0, 0);

			} else if (objectOver.name == "Left") { // Topleft to Left

				cameraHolder.transform.Translate ((scrollSpeed  * -1 ) * Time.deltaTime, 0, scrollSpeed * Time.deltaTime);

			} else if (objectOver.name == "Top") { // Topright to  Top

				cameraHolder.transform.Translate (scrollSpeed * Time.deltaTime, 0, scrollSpeed * Time.deltaTime);

			} else if (objectOver.name == "Bottom") { // Bottomleft to Bottom

				cameraHolder.transform.Translate ((scrollSpeed  * -1 ) * Time.deltaTime, 0, (scrollSpeed  * -1 ) * Time.deltaTime);

			} else if (objectOver.name == "Right") { // Bottomright to Right

				cameraHolder.transform.Translate ( scrollSpeed * Time.deltaTime, 0, (scrollSpeed  * -1 ) * Time.deltaTime );

			}
		}

        if (upMoveCam && leftMoveCam) {  //Top to Topleft

            cameraHolder.transform.Translate(0, 0, scrollSpeedArrowKeys * Time.deltaTime);

        }
        else if (downMoveCam && rightMoveCam) { // Bottom to Bottomright

            cameraHolder.transform.Translate(0, 0, (scrollSpeedArrowKeys * -1) * Time.deltaTime);

        }
        else if (downMoveCam && leftMoveCam) { // Left to Bottomleft

            cameraHolder.transform.Translate((scrollSpeedArrowKeys * -1) * Time.deltaTime, 0, 0);

        }
        else if (upMoveCam && rightMoveCam) { // Right to Topright

            cameraHolder.transform.Translate(scrollSpeedArrowKeys * Time.deltaTime, 0, 0);

        }
        else if (leftMoveCam) { // Topleft to Left

            cameraHolder.transform.Translate((scrollSpeedArrowKeys * -1) * Time.deltaTime, 0, scrollSpeedArrowKeys * Time.deltaTime);

        }
        else if (upMoveCam) { // Topright to  Top

            cameraHolder.transform.Translate(scrollSpeedArrowKeys * Time.deltaTime, 0, scrollSpeedArrowKeys * Time.deltaTime);

        }
        else if (downMoveCam) { // Bottomleft to Bottom

            cameraHolder.transform.Translate((scrollSpeedArrowKeys * -1) * Time.deltaTime, 0, (scrollSpeedArrowKeys * -1) * Time.deltaTime);

        }
        else if (rightMoveCam) { // Bottomright to Right

            cameraHolder.transform.Translate(scrollSpeedArrowKeys * Time.deltaTime, 0, (scrollSpeedArrowKeys * -1) * Time.deltaTime);

        }

    }

	public void ScrollTriggered(BaseEventData data) {

		PointerEventData pointerData = data as PointerEventData;

		objectOver = pointerData.pointerEnter;

		//Debug.Log (objectOver.name);

		scrolling = true;

	}

	public void ScrollEnded() {

		scrolling = false;

	}

}
