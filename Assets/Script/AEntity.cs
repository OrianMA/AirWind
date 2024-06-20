using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AEntity : MonoBehaviour
{
    public int lifePoint;
    public int energy;
    List<Status> allStatus = new();
    DeckSystem deck;
}
