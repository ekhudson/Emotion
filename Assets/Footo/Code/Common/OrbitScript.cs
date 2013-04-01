using UnityEngine;
using System.Collections;

public class OrbitScript : MonoBehaviour
{
    [System.Serializable]
    public class SatteliteObjects
    {
        public GameObject SatteliteObject;
        public float OrbitSpeed = 1f;
        public bool ClockwiseOrbit = true;
        public bool LookAtOrbitPoint = true;
    }

    public SatteliteObjects[] Sattelites;
    public Transform OrbitPoint;

    void Update ()
    {
        int direction = 1;
        foreach(SatteliteObjects sat in Sattelites)
        {
            direction = sat.ClockwiseOrbit == true ? 1 : -1;
            sat.SatteliteObject.transform.LookAt(OrbitPoint.transform.position);
            sat.SatteliteObject.transform.position += (direction * (sat.SatteliteObject.transform.right * sat.OrbitSpeed)) * Time.deltaTime;
        }
	}
}
