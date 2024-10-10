using System.Collections;
using System.Collections.Generic;
using UnityEngine.Animations;
using UnityEngine;

[CreateAssetMenu(fileName = "New Character Data", menuName = "Character Data/Create New Character Data")]
public class CharacterData : ScriptableObject
{
    public string characterName;

    public Sprite defaultSprite;
    public Sprite characterPanelArt;

    public RuntimeAnimatorController charAnim;
}
