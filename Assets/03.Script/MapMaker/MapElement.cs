using UnityEngine;

public class MapElement : MonoBehaviour
{
    public enum MapMakerElementType
    {
        WALL = 1,        
        SLANTEDWALL,
        START_POSITION,
        MIDDLEP_OSITION,
        END_POSITION,
        REPLICATE,
        NONE = 0,
    }

    [SerializeField] private MapMakerElementType elementType;
    public MapMakerElementType ElementType => elementType;
}
