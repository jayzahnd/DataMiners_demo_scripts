using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScollingBackground : MonoBehaviour
{

    public float speed;

    MeshRenderer meshRenderer;

    // Use this for initialization
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        meshRenderer.material.mainTextureOffset +=
            new Vector2(speed * Time.deltaTime, 0);
    }
}
