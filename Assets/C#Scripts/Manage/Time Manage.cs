using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManage : MonoBehaviour, ISavable
{
    public static TimeManage timeManage;

    public Light sunLight;
    public float time;
    public float speed;
    public float day;
    public bool isNight;
    public Color dayLight;
    public Color nightLight;

    private void OnEnable()
    {
        timeManage = this;
        ISavable savable = this;
        sunLight.color = dayLight;
        
        RenderSettings.ambientMode = UnityEngine.Rendering.AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.35f, 0.35f, 0.4f, 1f);
    }

    private void OnDisable()
    {
        ISavable savable = this;
        savable.UnRegisterSaveData();
    }

    private void Update()
    {
        LightLogic();
    }
    
    private void LightLogic()
    {
        sunLight.transform.rotation = Quaternion.Euler(time, -30, sunLight.transform.rotation.z);
        if (time <=180 && !isNight)
        {
            if (time >= 35)
                time += Time.deltaTime * speed;
            else
                time += Time.deltaTime * speed * 2;
            if (time >= 180)
            {
                sunLight.color = nightLight;
                time = 0;
                isNight = true;
            }
        }

        if (time <= 180 && isNight)
        {
            if (time >= 35)
                time += Time.deltaTime * speed;
            else
                time += Time.deltaTime * speed * 2;
            if (time >= 180)
            {
                sunLight.color = dayLight;
                time = 0;
                isNight = false;
                day++;
            }
        }
    }

    public void Save(Data data)
    {
        data.TimeAngle = time;
        data.IsNight = isNight;
        data.Day = day;
    }

    public void Load(Data data)
    {
        time = data.TimeAngle;
        isNight = data.IsNight;
        day = data.Day;
        sunLight.color = isNight ? nightLight : dayLight;
    }
}
