using UnityEngine;

namespace Device
{
    public class Sensors
    {
        public static string GetDeviceName
        {
            get
            {
                return "No Windows Phone";
            }
        }

        public static void SetAlwaysIdleMode()
        {
            
        }

        static public float BatteryPercent
        {
            get { return 100; }
        }

        public static bool CompassIsAvailable
        {
            get
            {
                return false;
            }
        }

        public static float CompassMagneticHeading
        {
            get
            {
                return 0;
            }
        }

        public static float CompassTrueHeading
        {
            get
            {
                return 0f;
            }
        }

        public static bool CompassStart()
        {
            return false;
        }

        public static void CompassStop()
        {
            
        }

        public static bool GyroscopeIsAvailable
        {
            get
            {
                return false;
            }
        }

        public static bool GyroscopeStart(bool componentsOutput)
        {
            bool result = GyroscopeIsAvailable;
            return result;
        }

        static public void GyroscopeStop()
        {

        }

        static public Vector3 GyroscopeCurrentRotation
        {
            get { return Vector3.zero; }
        }

        static public float GyroscopeCurrentRotationX
        {
            get { return 0; }
        }

        static public float GyroscopeCurrentRotationY
        {
            get { return 0; }
        }

        static public float GyroscopeCurrentRotationZ
        {
            get { return 0; }
        }

        static public bool MotionIsAvailable
        {
            get
            {
                return false;
            }
        }
        static public bool MotionStart()
        {
            bool result = false;
            return result;
        }

        static public void MotionStop()
        {
            
        }

        static public Vector3 MotionGravity
        {
            get { return Vector3.down * 9.8f; }
        }

        static public Quaternion MotionAttitude
        {
            get { return Quaternion.identity; }
        }

        static public float MotionAttitudeYaw
        {
            get { return 0f; }
        }
        static public float MotionAttitudePitch
        {
            get { return 0f; }
        }
        static public float MotionAttitudeRoll
        {
            get { return 0f; }
        }

    }
}
