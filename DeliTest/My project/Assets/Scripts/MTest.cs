using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.tvOS;

public class MTest : MonoBehaviour
{
    public float moveSpeed = 1e4f;
    [HelpBox("Acceleration",HelpBoxType.Info)]
    public Vector3 acceleration;
    public AccelerationEvent[] accelerationEvents;

    [HelpBox("Compass",HelpBoxType.Info)]
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

    private void Update()
    {

        transform.localRotation = Input.gyro.attitude;
        Vector3 bigAcc = new(
           Mathf.Abs(Input.gyro.userAcceleration.y) >= 0.1f ? Input.gyro.userAcceleration.y:0,
           Mathf.Abs(Input.gyro.userAcceleration.x) >= 0.1f ? -Input.gyro.userAcceleration.x:0,
           Mathf.Abs(Input.gyro.userAcceleration.z) >= 0.1f ? Input.gyro.userAcceleration.z:0
           );
        GetComponent<Rigidbody>().velocity +=
            new Vector3(Input.gyro.userAcceleration.y,
                -Input.gyro.userAcceleration.x,
                Input.gyro.userAcceleration.z)
            * moveSpeed * Time.deltaTime;
        Input.gyro.updateInterval = 1f;
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


        Debug.Log("acc " + acceleration);
        Debug.Log("g acc " + gyroUserAcceleration);
    }
}
