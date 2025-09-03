using UnityEngine;

public class MapElement : MonoBehaviour
{
    public enum MapMakerElementType
    {
        WALL=1,
        SLANTEDWALL ,
        MIDDLE_POSITION,
        REPLICATE,
        NONE = 0,
    }

    [SerializeField] private MapMakerElementType elementType;
    public MapMakerElementType ElementType => elementType;
}
