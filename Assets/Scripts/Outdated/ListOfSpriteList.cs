using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpriteList
{

    public Sprite[] Pack;
}

public class ListOfSpriteList : MonoBehaviour
{

    [SerializeField] public SpriteList[] Sprites;
    [SerializeField] public SpriteList[] BackSprites;

    void Start()
    {
        
    }
}
