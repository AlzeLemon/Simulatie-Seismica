using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PointManager : MonoBehaviour
{
    public int Length = 10;
    public float K_Stiffness = 1;//Newton per unit
    public float Efficiency = 1;
    public float desired_distance = 1;

    public int mode;
    public float max_hue= 2;
    List<Point> Points;
    public GameObject PointGameObject;
    public float ExplosionPower = 1;
    public CanvasManager cm;
    public float latest_expl;
    public float last_max;
    public GameObject Pointholder;
    public void ResetSim(Slider sl) {
        Length = (int)sl.value;
        Points.Clear();
        Destroy(Pointholder);
        Start();
    }
    public Vector2 IntToGrid(int pos , int length) {
        return new Vector2(pos%length , pos/length);
    }
    void CreatePoints() {
        Points = new List<Point>();
        for (int i = 0; i < Length * Length; i++) {
            Vector2 gridpos = IntToGrid(i, Length);
            Points.Add(Instantiate(PointGameObject, gridpos, transform.rotation,Pointholder.transform).GetComponent<Point>());
            Points[i].position = gridpos;
            Points[i].pm = this;
            Points[i].index = i;
            Points[i].neighbors = new List<Point>();
            Points[i].ResetTime();
        }
    }
    void LockPoints() {
        for (int i = 0; i < Length; i++) {
            Points[i].Lock();
        }
        for (int i = 0; i < Length; i++) {
            Points[i*Length].Lock();
        }
        for (int i = 1; i <= Length; i++) {
            Points[i*Length-1].Lock();
        }
    }
    void CreateNeighbors() {

        for (int i = Length; i < Length * Length; i++) {
            Points[i].neighbors.Add(Points[i - Length]); // Down
        }

        for (int i = 0; i < Length * (Length-1); i++) {
            Points[i].neighbors.Add(Points[i + Length]); // Up
        }

        for (int i = 0; i < Length; i++) {
            for (int j = 0; j < Length-1; j++) {
                Points[i * Length + j].neighbors.Add(Points[i * Length + j + 1]); // Right
            }
        }

        for (int i = 0; i < Length; i++) {
            for (int j = 1; j < Length; j++) {
                Points[i * Length + j].neighbors.Add(Points[i * Length + j - 1]); // Left
            }
        }

        for (int i = 0; i < Length-1; i++) {
            for (int j = 0; j < Length-1; j++) {
                Points[i * Length + j].diagneighbors.Add(Points[i * Length + j +Length +1]); // UpRight
            }
        }

        for (int i = 0; i < Length-1; i++) {
            for (int j = 1; j < Length; j++) {
                Points[i * Length + j].diagneighbors.Add(Points[i * Length + j +Length -1]); // UpLeft
            }
        }

        for (int i = 1; i < Length; i++) {
            for (int j = 1; j < Length; j++) {
                Points[i * Length + j].diagneighbors.Add(Points[i * Length + j -Length -1]); // DownLeft
            }
        }

        for (int i = 1 ; i < Length; i++) {
            for (int j = 0; j < Length -1; j++) {
                Points[i * Length + j].diagneighbors.Add(Points[i * Length + j -Length +1]); // DownRight
            }
        }


    }
    public void Explode(Vector2 pos) {
        latest_expl = Time.time;
        last_max = latest_expl;
        int p=0;
        float dist = Vector2.Distance(Points[p].position, pos);
        for(int i = 0;i<Length*Length;i++) {
            Points[i].ResetTime();
            if (Vector2.Distance(Points[i].position, pos) < dist) {
                dist = Vector2.Distance(Points[i].position, pos);
                p = i;
            }
        }
        foreach (Point ne in Points[p].neighbors) {
            int ind = ne.index;
            Points[ind].AddSpeed((Points[ind].position - Points[p].position).normalized * ExplosionPower);
        }
        foreach (Point ne in Points[p].diagneighbors) {
            int ind = ne.index;
            Points[ind].AddSpeed((Points[ind].position - Points[p].position).normalized * ExplosionPower / 1.414213f);
        }
    }
    public void Start() {
        Pointholder = Instantiate(new GameObject());
        latest_expl = Time.time;
        last_max = latest_expl;

        CreatePoints();
        //LockPoints();
        CreateNeighbors();
        cm.AfterStart();
    }

    void Update()
    {
        foreach(Point p in Points) {
            p.UpdateAcceleration();
        }
        foreach(Point p in Points) {
            p.UpdateSpeed();
        }
        foreach(Point p in Points) {
            p.UpdatePosition();
        }
        foreach(Point p in Points) {
            p.UpdateScreenPosition();
        }
    }
}
