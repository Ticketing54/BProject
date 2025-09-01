using System.Collections.Generic;
using UnityEngine;

public class MapMakerContainer : MonoBehaviour
{
    public List<ElementData> elementList;

    [System.Serializable]
    public class ElementData
    {
        public MapElement.MapMakerElementType type;
        public GameObject elementObject;
    }
    public GameObject GetElementObject(MapElement.MapMakerElementType _type)
    {
        int index = elementList.FindIndex(o => o.type == _type);

        if (index < 0)
            return null;

        return elementList[index].elementObject;
    }
}
