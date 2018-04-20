using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour {

    [Header("Game")]
    public GravityForce prefab;
    [Range(1, 10)] public int spawnAmo;

    [Header("Rules")]
    public float timeLimit;
    public float size;
    public Boundary boundary;
    public GameMenu gameMenu;

    private bool paused = false;
    private bool ended = false;
    private bool failing = false;

    public static Game Instance;

    private void Awake() {
        Instance = this;
    }

    // Use this for initialization
    void Start () {
        Camera cam = Camera.main;
        cam.orthographicSize = size;

        boundary.SetSize(size);
        boundary.SetColor(Color.green);

        gameMenu.UpdateTime(timeLimit);

        //Random.InitState(0);
        for(int i = 0; i < spawnAmo; i++) {
            Vector3 pos = Vector3.zero;
            pos.x = Random.Range(-size, size);
            pos.y = Random.Range(-size, size);

            var obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.initForce = Random.insideUnitCircle;
        }

        PauseGame();
    }
	
	// Update is called once per frame
	void Update () {
        if (ended) {
            return;
        }
        if (paused) {
            PauseUpdate();
        } else {
            PlayUpdate();
        }

        CheckBoundary();
    }

    // Spawn Balls, Stop Time
    private void PauseUpdate() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            PlayGame();
            return;
        }

        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) { 
            // Check if User clicked game area vs. UI
            
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;

            var obj = Instantiate(prefab, pos, Quaternion.identity);
            obj.OnPauseGame();

            print("Spawning Object at " + pos);
        }
    }

    // Move
    private void PlayUpdate() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            PauseGame();
            return;
        }

        timeLimit -= Time.deltaTime;
        if(timeLimit < 0f) { 
            timeLimit = 0f;
           
            gameMenu.EndGame((failing ? "You Lost!" : "You Won!"));
            PauseGame();
            ended = true;
        }

        gameMenu.UpdateTime(timeLimit);
    }

    private void CheckBoundary() {
        GravityForce[] forces = GameObject.FindObjectsOfType<GravityForce>();
        Color c = Color.green;
        failing = false;
        foreach (var force in forces) {
            Vector3 pos = force.transform.position;
            float radius = force.radius;


            //Debug.DrawLine(new Vector2(pos.x - radius, pos.y), new Vector2(pos.x, pos.y + radius), Color.gray);
            //Debug.DrawLine(new Vector2(pos.x, pos.y + radius), new Vector2(pos.x+radius, pos.y), Color.gray);
            //Debug.DrawLine(new Vector2(pos.x + radius, pos.y), new Vector2(pos.x, pos.y - radius), Color.gray);
            //Debug.DrawLine(new Vector2(pos.x, pos.y - radius), new Vector2(pos.x - radius, pos.y), Color.gray);

            if (pos.x - radius < -size || pos.x + radius > size || pos.y - radius < -size || pos.y + radius > size) {
                print("Meteors out of Bounds");
                //Debug.Break();

                c = Color.red;
                failing = true;
                break;
            }
        }
        boundary.SetColor(c);
    }

    public void PauseGame() {
        paused = true;

        GravityForce[] objects = FindObjectsOfType<GravityForce>();
        foreach (var go in objects) {
            go.OnPauseGame();
        }

        //print("Paused");
        gameMenu.ChangeGameBtn("Play");
    }

    public void PlayGame() {
        paused = false;

        GravityForce[] objects = FindObjectsOfType<GravityForce>();
        foreach (var go in objects) {
            go.OnResumeGame();
        }

        //print("Playing");
        gameMenu.ChangeGameBtn("Pause");
    }

    public void EndGame() {
#if UNITY_EDITOR
        EditorApplication.isPlaying = false;
#endif
        Application.Quit();
        
    }

    public void PlayAgain() {
        SceneManager.LoadScene("Game");
    }
}
