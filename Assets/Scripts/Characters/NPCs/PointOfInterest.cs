﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum POIType
{
    Item,
    Trade,
    Door
}

public class PointOfInterest : MonoBehaviour {

    public POIType navPointOfInterest;
    public bool occupied;
}
