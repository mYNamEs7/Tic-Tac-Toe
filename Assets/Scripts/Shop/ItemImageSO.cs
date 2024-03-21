using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item Image", menuName = "Scriptable Objects/Item Image")]
public class ItemImageSO : ScriptableObject
{
    public string key;
    public Sprite mainImage;
    public Sprite iconImage;
}
