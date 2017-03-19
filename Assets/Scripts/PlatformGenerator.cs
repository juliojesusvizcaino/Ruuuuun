using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformGenerator : MonoBehaviour {
    public GameObject[] Obstacles;
    public float Offset;
    public float Margin;

    private float _lastPosition;
    private GameObject _obstaclesParent;

	// Use this for initialization
	void Start () {
	    _obstaclesParent = GameObject.Find("Obstacles");
	}
	
	// Update is called once per frame
	void Update () {
	    if (transform.position.z  + Margin + Offset > _lastPosition) {
	        int random = Random.Range(0, Obstacles.Length);
	        GameObject spawnObject;

	        spawnObject = Instantiate(Obstacles[random], (_lastPosition + Margin) * Vector3.forward + Obstacles[random].transform.position, Quaternion.identity);
	        spawnObject.transform.parent = _obstaclesParent.transform;
	        _lastPosition += Margin;
	        Destroy(spawnObject, 10.0f);
	    }
	}
}
