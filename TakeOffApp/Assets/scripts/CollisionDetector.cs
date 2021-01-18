using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    public Parallaxer.Direction direction;
    // Start is called before the first frame update
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter(Collider collision) {
        if (collision.gameObject.tag == "obstacle" || collision.gameObject.name == "cloud") {
            if (!GameObject.Find("ObstacleSpawner").GetComponent<Parallaxer>().OnScreen(gameObject.transform)) {
                gameObject.transform.position = GameObject.Find("ObstacleSpawner").GetComponent<Parallaxer>().changePos(direction);
            }
        }
    }

}
