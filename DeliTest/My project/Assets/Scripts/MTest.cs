using UnityEngine;

public class MTest : MonoBehaviour
{
    public float moveSpeed = 1e4f;
    [HelpBox("Acceleration", HelpBoxType.Info)]
    public Vector3 acceleration;
    public AccelerationEvent[] accelerationEvents;

    [HelpBox("Compass", HelpBoxType.Info)]
    public bool compassEnabled;
    public float compassHeadingAccuracy;
    public float compassMagneticHeading;
    public Vector3 compassRawVector;
    public double compassTimestamp;
    public float compassTrueHeading;

    public bool compensateSensors;

    public DeviceOrientation deviceOrientation;


    [HelpBox("Gyroscope（陀螺仪）", HelpBoxType.Info)]
    public Quaternion gyroAttitude;
    public Vector3 gyroEuler;
    public bool gyroEnabled;
    public Vector3 gyroGravity;
    public Vector3 gyroRotationRate;
    public Vector3 gyroRotationRateUnbiased;
    public float gyroUpdateInterval;
    public Vector3 gyroUserAcceleration;

    public LocationService locationService;
    public LocationInfo locationLastInfo;
    public double locationTimeStamp;
    public float locationHorizontalAccuracy;
    public float locationVerticalAccuracy;
    public float locationAltitude;
    public float locationLatitude;
    public float locationLongitude;




    public bool multiTouchEnabled;
    public bool simulateMouseWithTouches;
    public bool stylusTouchSupported;
    public Touch[] touches;
    public bool touchEnabled;
    public bool touchPressureSupported;

    public Quaternion lastSetQuaternion = Quaternion.identity;
    Vector3 tarUpDirection = Vector3.up;
    Vector3 tarRightDirection = Vector3.right;
    Vector3 modelUpDirection = Vector3.back;
    Vector3 modelRightDirection = Vector3.down;
    public bool changeToLeftHand = true;
    private void Update()
    {
        Quaternion currentGyroRotation = Input.gyro.attitude;
        //currentGyroRotation = new Quaternion(-currentGyroRotation.y, currentGyroRotation.x, currentGyroRotation.z, currentGyroRotation.w);
        Quaternion deltaRotation = Quaternion.Inverse(lastSetQuaternion) * currentGyroRotation;
        Vector3 dv = deltaRotation.eulerAngles;
        dv = new(-dv.y, dv.x, dv.z);
        deltaRotation = Quaternion.Euler(dv);
        transform.localRotation = deltaRotation;

        acceleration = Input.acceleration;
        accelerationEvents = Input.accelerationEvents;
        int accId = 0;
        //if (accelerationEvents.Length == 0)
        //{
        //    Debug.Log("No acceleration events");
        //}
        //foreach (var accelerationEvent in accelerationEvents)
        //{
        //    Debug.Log(accId++ + " " + accelerationEvent.acceleration);
        //}
        
        Input.backButtonLeavesApp = true;

        compassEnabled = Input.compass.enabled = true;
        compassHeadingAccuracy = Input.compass.headingAccuracy;
        compassMagneticHeading = Input.compass.magneticHeading;
        compassRawVector = Input.compass.rawVector;
        compassTimestamp = Input.compass.timestamp;
        compassTrueHeading = Input.compass.trueHeading;

        compensateSensors = Input.compensateSensors;
        deviceOrientation = Input.deviceOrientation;

        gyroAttitude = Input.gyro.attitude;
        gyroEuler = gyroAttitude.eulerAngles;
        gyroEnabled = Input.gyro.enabled = true;
        gyroGravity = Input.gyro.gravity;
        gyroRotationRate = Input.gyro.rotationRate;
        gyroRotationRateUnbiased = Input.gyro.rotationRateUnbiased;
        gyroUpdateInterval = Input.gyro.updateInterval;
        gyroUserAcceleration = Input.gyro.userAcceleration;

        locationService = Input.location;
        if(locationService.status == LocationServiceStatus.Stopped)
            Input.location.Start();
        locationLastInfo = locationService.lastData;
        locationTimeStamp = locationLastInfo.timestamp;
        locationHorizontalAccuracy = locationLastInfo.horizontalAccuracy;
        locationVerticalAccuracy = locationLastInfo.verticalAccuracy;
        locationAltitude = locationLastInfo.altitude;
        locationLatitude = locationLastInfo.latitude;
        locationLongitude = locationLastInfo.longitude;
        locationAltitude = locationLastInfo.altitude;


        multiTouchEnabled = Input.multiTouchEnabled;
        simulateMouseWithTouches = Input.simulateMouseWithTouches;
        stylusTouchSupported = Input.stylusTouchSupported;
        touches = Input.touches;
        touchEnabled = Input.touchSupported;
        touchPressureSupported = Input.touchPressureSupported;
    }
    
    public void ResetRotation()
    {
        lastSetQuaternion = Input.gyro.attitude;
        transform.localRotation = Quaternion.identity;
    }
}
