using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour {

    private MeshRenderer[] boundaryLines;
    private Material sharedMat;

	// Use this for initialization
	void Start () {
        boundaryLines = GetComponentsInChildren<MeshRenderer>();
        sharedMat = boundaryLines[0].sharedMaterial;
	}
	
	public void SetColor(Color c) {
        sharedMat.color = c;
    }

    public void SetSize(float pos) {
        boundaryLines[0].transform.localPosition = new Vector3(-pos, 0f);
        boundaryLines[1].transform.localPosition = new Vector3(pos, 0f);
        boundaryLines[2].transform.localPosition = new Vector3(0f, -pos);
        boundaryLines[3].transform.localPosition = new Vector3(0f, pos);
    }
}
