using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ReSkinItems : MonoBehaviour
{
    public string spriteSheetName;

    void LateUpdate() {
        var subSprites = Resources.LoadAll<Sprite>("Objetos/" + spriteSheetName);
       // Debug.Log(subSprites.Length);

        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>()) {
            string spriteName = renderer.sprite.name;
            var newSprite = Array.Find(subSprites, item => item.name == spriteName);

            if (newSprite)
                //Debug.Log("Ha encontrado el objeto!");
                renderer.sprite = newSprite;
        }


    }
}
