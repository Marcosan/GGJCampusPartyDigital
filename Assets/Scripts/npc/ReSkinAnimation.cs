﻿using UnityEngine;
using System;
using System.Linq;

public class ReSkinAnimation : MonoBehaviour
{
    public string spriteSheetName;
    void LateUpdate() {
        var subSprites = Resources.LoadAll<Sprite>("Personajes/" + spriteSheetName);
        //Debug.Log(subSprites.Length);

        foreach (var renderer in GetComponentsInChildren<SpriteRenderer>()) {
            if (renderer.sprite == null) {
                continue;
            }
            string spriteName = renderer.sprite.name;
            var newSprite = Array.Find(subSprites, item => item.name == spriteName);

            if (newSprite)
               // Debug.Log("Ha encontrado el objeto!");
                renderer.sprite = newSprite;
        }


    }
}
