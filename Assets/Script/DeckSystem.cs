using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DeckBuild", menuName = "ScriptableObjects/Deck/DeckBuild", order = 1)]
public class DeckSystem : ScriptableObject
{
    public List<AEffect> cards;
}
