using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Point : MonoBehaviour {
    public Vector2 position;
    SpriteRenderer spriteRenderer;
    Vector2 speed;
    Vector2 acceleration;
    public PointManager pm;
    public int index;
    public float mass = 1;
    bool locked = false;
    public List<Point> neighbors;
    public List<Point> diagneighbors;
    public Vector2[] bounds;
    float time_at_max;
    float maxspeed;
    public void ResetTime() {
        //speed= Vector2.zero;
        time_at_max = Time.time;
        maxspeed = speed.magnitude;
    }
    public void UpdateScreenPosition() {
        if(!spriteRenderer)spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        transform.position = position;
        if (locked) return;
        switch(pm.mode) {
            case 1: {
                spriteRenderer.color = Color.red;
                break;
            }
            case 2: {
                spriteRenderer.color = Color.Lerp(Color.blue, Color.red, Mathf.Min(pm.max_hue, Mathf.Abs(speed.x) +Mathf.Abs(speed.y)) / pm.max_hue);
                //spriteRenderer.color = Color.HSVToRGB(Mathf.Min(pm.max_hue, speed.x + speed.y) / pm.max_hue * 250 + 100, 100, 100);
                break;
            }
            case 3: {
                if (speed.magnitude > maxspeed*1.5f && speed.magnitude > pm.max_hue ) {
                    maxspeed = speed.magnitude;
                    pm.last_max = Time.time;
                    time_at_max = Time.time;
                }
                spriteRenderer.color = Color.Lerp(Color.black, Color.white,(0.001f - pm.latest_expl + time_at_max) / (0.001f - pm.latest_expl + pm.last_max));

                break;
            }
        }
    }
    public void UpdatePosition() {
        if (locked) return;
        position += speed * Time.deltaTime;
    }
    public void UpdateSpeed() {
        if (locked) return;
        speed += acceleration * Time.deltaTime;
        speed -= speed * (1-pm.Efficiency)*Time.deltaTime;
    }
    Vector2 GetForceFrom(Vector2 target , float desired_distance) {
        Vector2 resultant = target;
        resultant -= position;
        return pm.K_Stiffness * (resultant - desired_distance * resultant.normalized);
    }
    public void UpdateAcceleration() {
        if (locked) return;
        acceleration = Vector2.zero;
        foreach (Point p in neighbors) {
            acceleration += GetForceFrom(p.position,pm.desired_distance);
        }
        foreach (Point p in diagneighbors) {
            acceleration += GetForceFrom(p.position , pm.desired_distance * 1.414213f); // *sqrt(2)
        }
    }
    public void Lock() {
        locked = true;
        if(spriteRenderer==null) spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = false;
    }
    private void Start() {
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
    }
    public void AddSpeed(Vector2 newspeed) {
        speed += newspeed;
    }
}
