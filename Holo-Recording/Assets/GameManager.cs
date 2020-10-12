using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public GameObject Cube;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //to play the animation
        if (Input.GetKeyDown(KeyCode.F))
        {
            Cube.GetComponent<Animator>().runtimeAnimatorController = Cube.GetComponent<AnimationRecorder>().aoc[Cube.GetComponent<AnimationRecorder>().playIndex];
            Cube.GetComponent<Animator>().SetTrigger("Play");
        }
    }
}
