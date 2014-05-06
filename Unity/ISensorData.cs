using UnityEngine;
using System.Collections;

public interface ISensorData
{
    float MainValue
    {
        get;
    }
    bool IsAvailable
    {
        get;
    }
    bool IsNative
    {
        get;
    }
    bool IsEnable
    {
        get;
        set;
    }
    string Name
    {
        get;
    }
    float NativeValue
    {
        get;
    }
    float UnityValue
    {
        get;
    }

}
