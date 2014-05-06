using UnityEngine;
using System.Collections;

public class CompassSensor : MonoBehaviour, ISensorData
{

    public bool EnableGUI = true;
    public GUIText Console;
    public bool UseNative = true;

    private float _angle;
    private bool _enabled;

    private float _prop;
    /*============================================================*/
    /*================= UNITY FUNCTIONAL =========================*/
    /*============================================================*/
    void Start()
    {
        if (UseNative) Device.Sensors.CompassStart();
        else Input.compass.enabled = true;

        _enabled = this.enabled;
        _prop = Main.PROPOPRION;
    }

    void FixedUpdate()
    {
        _angle = UseNative ? (float)Device.Sensors.CompassTrueHeading : Input.compass.trueHeading;
    }

    void Update()
    {
        if (Console) Console.text = _angle.ToString();
    }

    void OnGUI()
    {
        if (!EnableGUI) return;
        if (GUI.Button(new Rect(10 * Main.PROPOPRION, Screen.height - 80 * _prop, 200 * _prop, 30 * _prop), "Use Native Compass: " + UseNative))
        {
            UseNative = !UseNative;
        }
    }

    void OnDestroy()
    {
        Device.Sensors.CompassStop();
    }

    /*============================================================*/
    /*================= SETTERS & GETTERS ========================*/
    /*============================================================*/
    public float MainValue
    {
        get
        {
            return -_angle;
        }
        private set
        {
            _angle = value;
        }
    }
    public float NativeValue
    {
        get
        {
            return (float)Device.Sensors.CompassTrueHeading;
        }
    }

    public float UnityValue
    {
        get
        {
            return Input.compass.enabled ? Input.compass.trueHeading : 0.0f;
        }
    }

    /*============================================================*/
    /*================= ISensor Implementation ===================*/
    /*============================================================*/

    public bool IsAvailable
    {
        get { return Device.Sensors.CompassIsAvailable; }
    }
    public bool IsEnable
    {
        get { return _enabled; }
        set { _enabled = value; }
    }
    public string Name
    {
        get { return SensorTypes.COMPASS; }
    }
    public bool IsNative
    {
        get { return UseNative; }
    }
    /*============================================================*/
    /*================= PRIVATE FUNCTION =========================*/
    /*============================================================*/
}