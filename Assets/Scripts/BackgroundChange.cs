using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundChange : MonoBehaviour {

    public Material skyboxMat;
    public Texture[] textures;

    private static BackgroundChange Instance = null;


    private void Awake() {
        if(Instance == null) {
            Instance = this;
        } else {
            Destroy(this.gameObject);
        }
        RenderSettings.skybox = skyboxMat;
        OnLevelWasLoaded(0);
    }

    private void OnLevelWasLoaded(int level) {
        var state = Random.state;
        Random.InitState(System.DateTime.Now.Millisecond);

        skyboxMat.SetTexture("_FrontTex", textures[Random.Range(0, textures.Length)]);

        Random.state = state;
    }
}