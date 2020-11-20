using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float DEFAULT_SENSITIVITY = 0.5f;
    private const float MIN_RADIUS = 1f;

    [Header("Camera orbit")]
    [SerializeField] Transform target;
    public Transform Target
    {
        get => target;
        set { target = value ?? target; }
    }
    [SerializeField] Transform lookAt;
    public Transform LookAt
    {
        get => lookAt;
        set { lookAt = value ?? lookAt; }
    }

    float angleY, angleX;
    [SerializeField] float minAngleX = 10f, maxAngleX = 90f;
    public float MinAngleX
    { 
        get => minAngleX; 
        set {  minAngleX = minAngleX <= maxAngleX ? value : minAngleX ;}
    }
    public float MaxAngleX
    { 
        get => maxAngleX; 
        set {  maxAngleX = maxAngleX >= minAngleX ? value : minAngleX ;}
    }
    [SerializeField] float radius = 7.5f;
    public float Radius
    {
        get => radius;
        set => SetRadius(value);
    }
    void SetRadius(float value) => radius = Mathf.Max(MIN_RADIUS, value);

    [SerializeField] float turnSpeed = 15f;

    public float TurnSpeed
    {
        get => turnSpeed;
        set { turnSpeed = value >= 0 ? value : turnSpeed; }
    }
    [SerializeField] bool rotateTarget = true;
    Camera mainCamera;
    Transform cameraBody;
    Vector3 lookDirection;


    [Header("Camera zoom")]
    [SerializeField]  float normalFOV = 60f;
    [SerializeField] float zoomFOV = 40f;
    [SerializeField] float zoomTime = 0.15f;


    [Header("Mouse Sensitivity")]
    [Range(0,1)] [SerializeField] float mouseSensitivity = DEFAULT_SENSITIVITY;
    [SerializeField] bool usePrefsSensitivity = false;
    [SerializeField] string sensitivityPrefKey;

    #if UNITY_EDITOR
    void OnValidate()
    {
        if(usePrefsSensitivity){
            mouseSensitivity = PlayerPrefs.GetFloat(sensitivityPrefKey, DEFAULT_SENSITIVITY);
        }

        SetRadius(radius);
        
        GetMainCamera();
        UpdateLookDirection();
        SetFieldOfView(normalFOV);
        cameraBody.position = GetCameraPosition();
    }
    #endif

    void Awake()
    {
        GetMainCamera();
        UpdateLookDirection();
    }

    void GetMainCamera()
    {
        mainCamera = Camera.main;
        cameraBody = mainCamera.transform;
    }

    public void UpdateLookDirection()
    {
        lookDirection = (target.position - cameraBody.position).normalized; 
    }

    void SetFieldOfView(float fov)
    {
        mainCamera.fieldOfView = fov;
    }

    public void SetZoom(bool isZoom)
    {
        StartCoroutine(Zoom(isZoom ? zoomFOV : normalFOV));
    }
    
    IEnumerator Zoom(float fov)
    {
        float start = mainCamera.fieldOfView;
        float x = 0f;
        while (x < zoomTime)
        {
            x += Time.deltaTime;
            SetFieldOfView(Mathf.Lerp(start, fov, x / zoomTime));
            yield return null;
        }

        SetFieldOfView(fov);
    }

    void LateUpdate()
    {
        //Y angle using the Mouse Horizontal input
        float x = Input.GetAxis("Mouse X") * turnSpeed * mouseSensitivity;
        angleY += x;

        //X angle using the Mouse Vertical input
        float y = Input.GetAxis("Mouse Y") * turnSpeed * mouseSensitivity;
        angleX += y;
        angleX = Mathf.Clamp(angleX, minAngleX, maxAngleX);

        Vector3 newPosition = GetCameraPosition();
        cameraBody.position = newPosition;
        cameraBody.LookAt(lookAt);
        if (rotateTarget) RotateTarget();
    }

    Vector3 GetCameraPosition()
    {
        var quaternion = Quaternion.Euler(angleX, angleY, 0f);
        var rotOffset = quaternion * lookDirection;
        var newPosition = target.position - (rotOffset * radius);
        return newPosition;
    }

    void RotateTarget()
    {
        float desiredAngle = cameraBody.eulerAngles.y;
        float lerpAngle = Mathf.LerpAngle(target.eulerAngles.y, desiredAngle, 
        Time.deltaTime * turnSpeed);
        target.rotation = Quaternion.Euler(0f, lerpAngle, 0f);
    }
}