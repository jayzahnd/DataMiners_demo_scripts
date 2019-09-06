using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinimapClickToGoThere : MonoBehaviour, IPointerClickHandler
{

    [SerializeField] RenderTexture minimapIMG;
    public RectTransform minimapRendTexObj;
    [SerializeField]  GameObject minimapHolder;
    

    private static Camera minimapCamRef;
    [SerializeField]Camera minimapCamLocal;

    private static GameObject playerCamHolderRef;
    [SerializeField] GameObject playerCamHolderLocal;

    private static Vector3 playerCamOffsetRef;
    [SerializeField] Vector3 playerCamOffsetLocal;

    Vector3 newCameraPosition;
    private static Vector2 mapSize;


    public static void FetchCameraprefabs(Camera _minimapCam, GameObject _playerCamHolder, Vector3 _playerCamOffset, Vector2 _mapSize)
    {
        minimapCamRef = _minimapCam.GetComponent<Camera>();
        //Debug.Log(minimapCamRef.name + " is now set.");
        //playerCamRef = _playerCamHolder.GetComponentInChildren<Camera>(); obsolete. Keeping in case I need a direct cam reference later
        playerCamHolderRef = _playerCamHolder;
        playerCamOffsetRef = _playerCamOffset;

        mapSize = _mapSize;
        //Debug.Log(mapSize.x + " " + mapSize.y);
        //Debug.Log("playerCamOffsetRef: "+playerCamOffsetRef);

    }
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(minimapCamLocal == null)
        {
            minimapCamLocal = minimapCamRef;
        }
        if (playerCamHolderLocal == null)
        {
            playerCamHolderLocal = playerCamHolderRef;
        }
        if (playerCamOffsetLocal == null || playerCamOffsetLocal == Vector3.zero)
        {
            playerCamOffsetLocal = playerCamOffsetRef;
        }
    }

    //public void ClickToGoThere()
    //{
    //    RaycastHit hitFromAbove;
    //    Ray localRay = minimapCamRef.ScreenPointToRay(Input.mousePosition);

    //    if(Physics.Raycast(localRay,out hitFromAbove, Mathf.Infinity, 5<<9))
    //    {
    //        //Debug.Log(hitFromAbove.point);
    //        newCameraPosition = hitFromAbove.transform.position + playerCamOffset;

    //        playerCamRef.transform.position = newCameraPosition;
    //    }        
    //}

    //public void ClickToGoThere2()
    //{
    //    Rect miniMapRect = minimapRendTexObj.rect;
    //    Rect screenRect = new Rect(minimapRendTexObj.anchoredPosition.x, minimapRendTexObj.anchoredPosition.y, miniMapRect.width, miniMapRect.height);
    //    Debug.Log(miniMapRect);
    //    Debug.Log(screenRect);

    //    var mousePos = Input.mousePosition;
    //    Debug.Log("RAW mousePos x;y " + mousePos.x + " " + mousePos.y);
    //    mousePos.y -= screenRect.y;
    //    mousePos.x -= screenRect.x;
    //    Debug.Log("NEW mousePos x;y " + mousePos.x + " " + mousePos.y);

    //    Debug.Log(mapSize.x / screenRect.width);
    //    Debug.Log(mapSize.y / screenRect.height);

    //    Vector3 camPos = new Vector3(mousePos.x * (mapSize.x), 0, mousePos.y * (mapSize.y));

    //    Debug.Log("camPos " + camPos);

    //    playerCamHolderLocal.transform.position = camPos + playerCamOffsetLocal;

    //    Debug.Log("Final camPos " + playerCamHolderLocal.transform.position);

    //}

    //------------------------------------------------------------------------------------------------------------------------------------------------------
    //------------------------------------------------------------------------------------------------------------------------------------------------------
    // adapted from : https://forum.unity.com/threads/interactable-minimap-using-raw-image-render-texture-solved.525486/ || this NEEDS to implement "IPointerClickHandler" to work.

    public void OnPointerClick(PointerEventData eventData)
    {

        Vector2 localCursor = new Vector2(0, 0);
        
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(GetComponent<RawImage>().rectTransform, eventData.pressPosition, eventData.pressEventCamera, out localCursor))
        {

            Texture tex = GetComponent<RawImage>().texture;
            Rect r = GetComponent<RawImage>().rectTransform.rect;

            //Using the size of the texture and the local cursor, clamp the X,Y coords between 0 and width - height of texture
            float coordX = Mathf.Clamp(0, (((localCursor.x - r.x) * tex.width) / r.width), tex.width);
            float coordY = Mathf.Clamp(0, (((localCursor.y - r.y) * tex.height) / r.height), tex.height);

            //Convert coordX and coordY to % (0.0-1.0) with respect to texture width and height
            float recalcX = coordX / tex.width;
            float recalcY = coordY / tex.height;

            localCursor = new Vector2(recalcX, recalcY);

            CastMiniMapRayToWorld(localCursor);

        }
        else { Debug.Log("BOOP"); }

    }

    private void CastMiniMapRayToWorld(Vector2 localCursor)
    {
        Ray miniMapRay = minimapCamLocal.ScreenPointToRay(new Vector2(localCursor.x * minimapCamLocal.pixelWidth, localCursor.y * minimapCamLocal.pixelHeight));

        RaycastHit miniMapHit;

        if (Physics.Raycast(miniMapRay, out miniMapHit, Mathf.Infinity))
        {
            //Debug.Log("miniMapHit: " + miniMapHit.collider.gameObject + " HitPos= " + miniMapHit.point);

            Vector3 camPos = new Vector3(miniMapHit.point.x, 0, miniMapHit.point.z);
            playerCamHolderLocal.transform.position = camPos + playerCamOffsetLocal;
            //Debug.Log("Final camPos " + playerCamHolderLocal.transform.position);

        }

    }

}
