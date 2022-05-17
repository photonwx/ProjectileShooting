using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class CannonInterface : MonoBehaviour 
{
    [SerializeField]
    Cursor targetCursor;

    [SerializeField]
    CannonController cannon;

    [SerializeField]
    Text timeOfFlightText;
    [SerializeField]
    Toggle[] launchToggles;

    [SerializeField]
    float defaultFireSpeed = 35;

    [SerializeField]
    float defaultFireAngle = 45;

    [SerializeField]
    float defaultFireTime = 1;

    private float initialFireAngle;
    private float initialFireSpeed;
    private float initialFireTime;
    private bool useLowAngle;

    private ELaunchMode LaunchMode;

    void Awake()
    {
        LaunchMode = ELaunchMode.Speed;
        useLowAngle = true;
        initialFireAngle = defaultFireAngle;
        initialFireSpeed = defaultFireSpeed;
        initialFireTime = defaultFireTime;
        foreach(var item in launchToggles)
        {
            if(item.name.StartsWith("Speed"))
            {
                item.isOn = true;
            }
            item.onValueChanged.AddListener(ifselect => { if(ifselect) OnToggleValueChanged(item); });
        }
    }

    // 同一管理Toggle，Toggle发生改变执行相应的事件
    private void OnToggleValueChanged(Toggle item)
    {
        switch(item.name)
        {
            case "Speed":
                LaunchMode = ELaunchMode.Speed;
                break;
            case "Angle":
                LaunchMode = ELaunchMode.Angle;
                break;
            case "Time":
                LaunchMode = ELaunchMode.Time;
                break;
        }
    }

    void Update()
    {
        switch(LaunchMode)
        {
            case ELaunchMode.Speed:
                cannon.SetTargetWithSpeed(targetCursor.transform.position, initialFireSpeed, useLowAngle);
                break;
            case ELaunchMode.Angle:
                cannon.SetTargetWithAngle(targetCursor.transform.position, initialFireAngle);
                break;
            case ELaunchMode.Time:
                cannon.SetTargetWithTime(targetCursor.transform.position, initialFireTime);
                break;
            default:
                break;
        }

        if (Input.GetButtonDown("Fire1") && !EventSystem.current.IsPointerOverGameObject())
        {
            cannon.Fire();
        }

        timeOfFlightText.text = Mathf.Clamp(cannon.lastShotTimeOfFlight - (Time.time - cannon.lastShotTime), 0, float.MaxValue).ToString("F3");
    }

    public void SetInitialFireAngle(string angle)
    {
        initialFireAngle = Convert.ToSingle(angle);
    }

    public void SetInitialFireSpeed(string speed)
    {
        initialFireSpeed = Convert.ToSingle(speed);
    }

    public void SetInitialFireTime(string time)
    {
        initialFireTime = Convert.ToSingle(time);
    }

    public void SetLowAngle(bool useLowAngle)
    {
        this.useLowAngle = useLowAngle;
    }
}

public enum ELaunchMode
{
    Speed,
    Angle,
    Time
}