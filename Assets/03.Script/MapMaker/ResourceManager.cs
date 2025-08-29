using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    public static ResourceManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = FindAnyObjectByType<ResourceManager>();
            }

            return instance;
        }
    }

    private static ResourceManager instance;    

    private enum MapMakerResource
    {
        LeftSlantedWall,
        RightSlantedWall,
        Replicate
    }

    [System.Serializable]
    private class MapMakerContainer
    {
        public MapMakerResource resourceName;
        public GameObject resource;
    }

    [SerializeField] private List<MapMakerContainer> mapMakerResource_List;
    [SerializeField] private List<MapMakerContainer> playerResource_List;

    private void Awake()
    {


    }


}
