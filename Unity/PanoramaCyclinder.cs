using UnityEngine;
using System.Collections;

public class PanoramaCyclinder : MonoBehaviour
{
    private static float PANOSIZE = 7004.0f;
    private static float DELTA_ANGLE_MULT = 0.0f;

    public GUIText Console;
    public bool UseTouch = true;
    public int MoveSpeedMultiplier = 5;
    [Range(0, 20)] public int AngleViscosity = 5;

    private ISensorData _sensor;

    private Vector3 _mouseScreenPoint;
    private Vector3 _mouseDeltaOffset;
    private Vector3 _mouseStartPoint;
    private bool _mouseDown;

    private float _rotationSpeed;

    private bool UseGyroscope = false;
    private bool UseCompass = false;
    private bool UseMotion = false;

    void Start()
    {
        Device.Sensors.SetAlwaysIdleMode();
        DELTA_ANGLE_MULT = 360.0f * (float)MoveSpeedMultiplier / PANOSIZE;
    }

    void Update()
    {
        if (UseTouch)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
            {
                Vector2 touchDeltaPosition = Input.GetTouch(0).deltaPosition;
                transform.Rotate(0, CalculateDeltaAngleByOffset(touchDeltaPosition.x), 0);
            }

            if (Input.GetMouseButtonDown(0))
            {
                _mouseDown = true;
                _mouseStartPoint = Input.mousePosition;
            }

            if (_mouseDown)
            {
                _mouseScreenPoint = Input.mousePosition;
                _mouseDeltaOffset = _mouseScreenPoint - _mouseStartPoint;
                _mouseStartPoint = _mouseScreenPoint;

                transform.Rotate(0, CalculateDeltaAngleByOffset(_mouseDeltaOffset.x), 0);
            }
            if (Input.GetMouseButtonUp(0)) _mouseDown = false;
        }
        else
        {
            if (_sensor != null && _sensor.IsEnable)
            {
                Quaternion target = Quaternion.Euler(0, _sensor.MainValue, 0);
                transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * AngleViscosity);
            }
            else _sensor = FindWorkingSensor();
        }

        TraceAngle();
    }

    private float CalculateDeltaAngleByOffset(float offset)
    {
        float result = DELTA_ANGLE_MULT * offset;
        return result;
    }

    private void TraceAngle()
    {
        if (Console != null) Console.text = transform.localEulerAngles.y.ToString();
    }

    private ISensorData FindWorkingSensor()
    {
        ISensorData result = null;
        result = this.GetComponent<GyroscopeSensor>();
        if (result == null || result.IsEnable == false) result = this.GetComponent<CompassSensor>();
        else if (result == null || result.IsEnable == false) result = this.GetComponent<MotionSensor>();
        return result;
    }
}
