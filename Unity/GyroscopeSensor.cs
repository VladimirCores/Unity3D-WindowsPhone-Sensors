using UnityEngine;
using System.Collections;

public class GyroscopeSensor : MonoBehaviour, ISensorData
{

    public bool EnableGUI = true;
    public GUIText Console;

    public bool UseNative = true;
    public bool UseComponentsOutputs = true;

    public bool OutputX = true;
    public bool OutputY = false;
    public bool OutputZ = false;

    private Vector3 _rotation = new Vector3();
    private float _angle;
    private bool _enabled;

    private Gyroscope _gyro;
    private Vector3 _gyroCurrentRotationRate;
    private Vector3 _gyroCumulativeRotation = Vector3.zero;

    private float _prop;

    /*============================================================*/
    /*================= UNITY FUNCTIONAL =========================*/
    /*============================================================*/
    void Start()
    {
        if (UseNative) Device.Sensors.GyroscopeStart(UseComponentsOutputs);
        else Input.gyro.enabled = true;
        
        _enabled = this.enabled;
        _prop = Main.PROPOPRION;
    }

    void FixedUpdate()
    {
       if (UseNative)
        {
            if (UseComponentsOutputs)
            {
                _angle = OutputX ? Device.Sensors.GyroscopeCurrentRotationX : OutputY ? Device.Sensors.GyroscopeCurrentRotationY : OutputZ ? Device.Sensors.GyroscopeCurrentRotationZ : 0;
            }
            else
            {
                _rotation = Device.Sensors.GyroscopeCurrentRotation;
                _angle = GetOutputRotationValue();
            }
        }
        else
        {
            _rotation = Input.gyro.attitude.eulerAngles;
            _angle = GetOutputRotationValue();
        }
    }

    void OnGUI()
    {
        if (!EnableGUI) return;
        GUI.Button(new Rect(10 * _prop, Screen.height - 80 * _prop, 200 * _prop, 30 * _prop), "GYRO ANGLE: " + _angle);
        if (GUI.Button(new Rect(10 * _prop, Screen.height - 40 * _prop, 200 * _prop, 30 * _prop), "Use Native Gyroscope: " + UseNative))
        {
            UseNative = !UseNative;
        }

    }

    void Update()
    {
        if (Console) Console.text = _angle.ToString();
    }

    void OnDestroy()
    {
        Device.Sensors.GyroscopeStop();
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
        get
        {
            _rotation.Set(
                (float)Device.Sensors.GyroscopeCurrentRotationX,
                (float)Device.Sensors.GyroscopeCurrentRotationY,
                (float)Device.Sensors.GyroscopeCurrentRotationZ
            );
            _angle = GetOutputRotationValue();
            return _angle;
        }
    }

    public float UnityValue
    {
        get
        {
            _rotation = Input.gyro.attitude.eulerAngles;
            _angle = GetOutputRotationValue();
            return _angle;
        }
    }
    /*============================================================*/
    /*================= ISensor Implementation ===================*/
    /*============================================================*/
    public bool IsAvailable
    {
        get { return Device.Sensors.GyroscopeIsAvailable; }
    }
    public bool IsEnable
    {
        get { return _enabled; }
        set { _enabled = value; }
    }
    public bool IsNative
    {
        get { return UseNative; }
    }
    public string Name
    {
        get { return SensorTypes.GYROSCOPE; }
    }
    /*============================================================*/
    /*================= PRIVATE FUNCTION =========================*/
    /*============================================================*/
    private float GetOutputRotationValue()
    {
        float result = OutputX ? _rotation.x : OutputY ? _rotation.y : OutputZ ? _rotation.z : 0;
        return result;
    }
}
