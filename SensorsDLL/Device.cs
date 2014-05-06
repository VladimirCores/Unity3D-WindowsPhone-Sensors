using Windows.Phone.Devices.Power;
using Microsoft.Devices.Sensors;
using Microsoft.Phone.Shell;
using System;
using UnityEngine;
using Compass       = Microsoft.Devices.Sensors.Compass;
using Gyroscope     = Microsoft.Devices.Sensors.Gyroscope;
using Motion        = Microsoft.Devices.Sensors.Motion;
using Quaternion    = Microsoft.Xna.Framework.Quaternion;
using Vector3       = Microsoft.Xna.Framework.Vector3;
using Microsoft.Xna.Framework;

namespace Device
{
    public class Sensors
    {

        static private Compass          compass;
        static private float            compassMagneticHeading;
        static private float            compassTrueHeading;

        static private Gyroscope        gyro;
        static private DateTimeOffset   gyroLastUpdateTime      = DateTimeOffset.MinValue;
        static private Vector3          gyroCurrentRotationRate = Vector3.Zero;
        static private Vector3          gyroCumulativeRotation = Vector3.Zero;

        static private Motion           motion;
        static private AttitudeReading  motionAttitudeReading;
        static private Vector3          motionGravity;

        static public string GetDeviceName
        {
            get
            {
                return Microsoft.Phone.Info.DeviceStatus.DeviceName;
            }
        }

        static public void SetAlwaysIdleMode()
        {
            PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            PhoneApplicationService.Current.ApplicationIdleDetectionMode = IdleDetectionMode.Disabled;
        }

        static public float BatteryPercent
        {
            get { return Battery.GetDefault().RemainingChargePercent; }
        }

        static public bool CompassIsAvailable
        {
            get { return Compass.IsSupported; }
        }

        static public float CompassMagneticHeading
        {
            get { return compassMagneticHeading; }
        }

        static public float CompassTrueHeading
        {
            get  { return compassTrueHeading; }
        }

        static public bool CompassStart()
        {
            bool result = CompassIsAvailable;
            if (result)
            {
                if (compass == null) { 
                    compass = new Compass();
                    compass.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                }
                compass.CurrentValueChanged += handler_Compass;
                compass.Start();
            }
            return result;
        }

        static public void CompassStop()
        {
            compass.Stop();
            compass.CurrentValueChanged -= handler_Compass;
            compass.Dispose();
        }

        
        static public bool GyroscopeIsAvailable
        {
            get { return Gyroscope.IsSupported; }
        }
        static public bool GyroscopeStart()
        {
            bool result = GyroscopeIsAvailable;
            if (result)
            {
                gyroCumulativeRotation = Vector3.Zero;

                gyro = new Gyroscope();
                gyro.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                gyro.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<GyroscopeReading>>(handler_Gyroscope);
                try
                {
                    gyro.Start();
                }
                catch (InvalidOperationException)
                {
                    result = false;
                }  
            }
            return result;
        }
        static public void GyroscopeStop()
        {
            gyro.Stop();
            gyro.CurrentValueChanged -= handler_Gyroscope;
            gyro.Dispose();
        }
        static public UnityEngine.Vector3 GyroscopeCurrentRotation
        {
            get { return gyroCumulativeRotation.ToUnityVector3() * Mathf.Rad2Deg; }
        }
        static public float GyroscopeCurrentRotationX
        {
            get { return gyroCumulativeRotation.X * Mathf.Rad2Deg; }
        }
        static public float GyroscopeCurrentRotationY
        {
            get { return gyroCumulativeRotation.Y * Mathf.Rad2Deg; }
        }
        static public float GyroscopeCurrentRotationZ
        {
            get { return gyroCumulativeRotation.Z * Mathf.Rad2Deg; }
        }


        static public bool MotionIsAvailable
        {
            get
            {
                return Motion.IsSupported;
            }
        }
        static public bool MotionStart()
        {
            bool result = MotionIsAvailable;
            if (result)
            {
                motion = new Motion();
                motion.TimeBetweenUpdates = TimeSpan.FromMilliseconds(20);
                motion.CurrentValueChanged += handler_Motion;
                motion.Start();
            }
            return result;
        }

        static public void MotionStop()
        {
            motion.Stop();
            motion.CurrentValueChanged -= handler_Motion;
            motion.Dispose();
        }

        static public UnityEngine.Vector3 MotionGravity
        {
            get { return motionGravity.ToUnityVector3() * Mathf.Rad2Deg; }
        }

        static public UnityEngine.Quaternion MotionAttitude
        {
            get { return motionAttitudeReading.Quaternion.ToUnityQuaternion(); }
        }

        static public float MotionAttitudeYaw
        {
            get { return motionAttitudeReading.Yaw; }
        }
        static public float MotionAttitudePitch
        {
            get { return motionAttitudeReading.Pitch; }
        }
        static public float MotionAttitudeRoll
        {
            get { return motionAttitudeReading.Roll; }
        }

        static private void handler_Compass(object sender, SensorReadingEventArgs<CompassReading> e)
        {
            compassTrueHeading = (float)e.SensorReading.TrueHeading;
            compassMagneticHeading = (float)e.SensorReading.MagneticHeading;
        }

        static private void handler_Gyroscope(object sender, SensorReadingEventArgs<GyroscopeReading> e) 
        {
            if (gyroLastUpdateTime.Equals(DateTimeOffset.MinValue)){
                gyroLastUpdateTime = e.SensorReading.Timestamp;
            }
            else
            {
                gyroCurrentRotationRate = e.SensorReading.RotationRate;
                TimeSpan timeSinceLastUpdate = e.SensorReading.Timestamp - gyroLastUpdateTime;
                gyroCumulativeRotation += gyroCurrentRotationRate * (float)(timeSinceLastUpdate.TotalSeconds);
                gyroLastUpdateTime = e.SensorReading.Timestamp;
            }
        }

        static private void handler_Motion(object sender, SensorReadingEventArgs<MotionReading> e)
        {
            motionAttitudeReading = e.SensorReading.Attitude;
            motionGravity = e.SensorReading.Gravity;
        }
    }

    public static class CovertionUtils
    {
        public static UnityEngine.Vector3 ToUnityVector3(this Vector3 value)
        {
            return new UnityEngine.Vector3(value.X, value.Y, value.Z);
        }

        public static UnityEngine.Quaternion ToUnityQuaternion(this Quaternion value)
        {
            return new UnityEngine.Quaternion(value.X, value.Y, value.Z, value.W);
        }

    }
}
