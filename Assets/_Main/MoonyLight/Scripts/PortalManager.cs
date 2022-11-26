using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalManager :MonoBehaviour
{

    private static PortalManager instance = null;

    public Portal[] portals= new Portal[3];

    [Button]
    public void startCreate()
    {
        foreach (Portal portal in portals)
        {
            portal.startCreate();
        }
    }

    void Awake()
    {
        if (null == instance)
        {
            instance = this;


        }
        else
        {

            Destroy(this.gameObject);
        }
    }

    public static PortalManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }



}
