using UnityEngine;
using System.Collections;

public class MotionSensor : MonoBehaviour, ISensorData
{
    public bool EnableGUI = true;
    public GUIText Console;

    public bool Yaw = true;
    public bool Pitch = false;
    public bool Roll = false;

    private float _angle;
    private bool _enabled;

    private float _prop;
    /*============================================================*/
    /*================= UNITY FUNCTIONAL =========================*/
    /*============================================================*/
    void Start()
    {
        Device.Sensors.MotionStart();
        _enabled = this.enabled;
    }

    void FixedUpdate()
    {
        _angle = Yaw ? Device.Sensors.MotionAttitudeYaw :
            Pitch ? Device.Sensors.MotionAttitudePitch : Roll ? Device.Sensors.MotionAttitudeRoll : 0;
    }

    void Update()
    {
        if (Console) Console.text = _angle.ToString();
    }
    void OnGUI()
    {
        if (!EnableGUI) return;
        GUI.Button(new Rect(10 * Main.PROPOPRION, Screen.height - 80 * _prop, 200 * _prop, 30 * _prop), "Motion: " + _angle);
    }

    void OnDestroy()
    {
        Device.Sensors.MotionStop();
    }
    /*============================================================*/
    /*================= SETTERS & GETTERS ========================*/
    /*============================================================*/
    public float MainValue
    {
        get { return _angle; }
    }
    public float NativeValue
    {
        get { return _angle; }
    }

    public float UnityValue
    {
        get { return 0; }
    }
    /*============================================================*/
    /*================= ISensor Implementation ===================*/
    /*============================================================*/
    public bool IsAvailable
    {
        get { return Device.Sensors.MotionIsAvailable; }
    }
    public bool IsNative
    {
        get { return true; }
    }
    public bool IsEnable
    {
        get { return _enabled; }
        set { _enabled = value; }
    }
    public string Name
    {
        get { return SensorTypes.MOTION; }
    }
    /*============================================================*/
    /*================= PRIVATE FUNCTION =========================*/
    /*============================================================*/
}
