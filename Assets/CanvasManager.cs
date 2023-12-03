using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public PointManager pointManager;
    public Slider stiffness;
    public TextMeshProUGUI stiffness_text;
    public Slider efficiency;
    public TextMeshProUGUI efficiency_text;
    public Slider distance;
    public TextMeshProUGUI distance_text;
    public Slider explosionpower;
    public TextMeshProUGUI explosionpower_text;
    public Slider dtime;
    public Slider mode;
    public TextMeshProUGUI dtime_text;
    public Slider len;
    public TextMeshProUGUI len_text;
    public void AfterStart()
    {
        stiffness.value = pointManager.K_Stiffness;
        efficiency.value = pointManager.Efficiency;
        distance.value = pointManager.desired_distance;
        explosionpower.value = pointManager.ExplosionPower;
        dtime.value = Time.timeScale;
    }
    void Update() {
        pointManager.K_Stiffness = stiffness.value;
        stiffness_text.text = "Coef elasticitate: " + stiffness.value;
        pointManager.Efficiency = efficiency.value;
        efficiency_text.text = "Eficienta: " + efficiency.value;
        pointManager.desired_distance = distance.value;
        distance_text.text = "Distanta: " + distance.value;
        pointManager.ExplosionPower = explosionpower.value;
        explosionpower_text.text = "Putere: " + explosionpower.value;
        dtime_text.text = "Timp: "+Time.timeScale;
        Time.timeScale = dtime.value;
        pointManager.mode = (int)mode.value;
        len_text.text = "Marime: " + len.value;
    }
}
