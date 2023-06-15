using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CP_OnomatopeChoice : MonoBehaviour
{
    public List<TextureData> textureDatas;
    public SpriteRenderer spriteFond;
    public SpriteRenderer spriteOnomatope;
    
    public void BAM ()
    {
        TextureData texturechoisie = textureDatas[Random.Range(0, textureDatas.Count)];
        spriteFond.sprite = texturechoisie.textureFond;
        spriteOnomatope.sprite = texturechoisie.textureOnomatope;
    }

    [Serializable]
    public struct TextureData
    {
        public Sprite textureOnomatope;
        public Sprite textureFond;
    }
}
