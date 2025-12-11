using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameObjectExtension 
{
    public static bool CompareLayerMask(GameObject objectToCheck, LayerMask layerToCheck) => (layerToCheck.value & (1 << objectToCheck.layer)) != 0;
}
