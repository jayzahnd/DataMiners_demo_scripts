using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HoverAndSelection : MonoBehaviour {

    [SerializeField] float fadeRate= 4.0f;

    public enum SelectionState { DESELECTED, HOVEREDON, SELECTED}
    public SelectionState currentSelTarget;

    public SpriteRenderer HSrndr;
    public Sprite hsSprite =null;
    public bool isInterractable= true;

    // Use this for initialization
    void Start () {
        currentSelTarget = SelectionState.DESELECTED;
        HSrndr = gameObject.GetComponent<SpriteRenderer>();
        HSrndr.enabled = false;
        
    }

    public void SetSpriteRef() { HSrndr.sprite = hsSprite; }

    public void SetHoverdOn()
    {
        if (isInterractable)
        {
            currentSelTarget = SelectionState.HOVEREDON;
            
            HSrndr.enabled = true;
            StartCoroutine(FadeSprite(HSrndr, currentSelTarget));
        }
    }
    public void SetSelected()
    {
        if (isInterractable)
        {
            currentSelTarget = SelectionState.SELECTED;
        }            
    }
    public void SetDeselected()
    {
        currentSelTarget = SelectionState.DESELECTED;
        HSrndr.enabled = false;
        StopAllCoroutines();
    }


    public IEnumerator FadeSprite(SpriteRenderer sprite, SelectionState sel)
    {    
        float alpha = 1f;

        while (currentSelTarget == sel)
        {
            while (sprite.color.a > 0)
            {
                alpha -= Time.deltaTime * fadeRate;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
                yield return null;
            }
            while (sprite.color.a < 1)
            {
                alpha += Time.deltaTime * fadeRate;
                sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, alpha);
                yield return null;
            }
            yield return null;
        }
        sprite.color = new Color(sprite.color.r, sprite.color.g, sprite.color.b, 1f);
    }

    // Update is called once per frame
    void Update () {
		
	}
}
