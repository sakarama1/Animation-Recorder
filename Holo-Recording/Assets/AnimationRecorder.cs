using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

public class AnimationRecorder : MonoBehaviour
{
    public AnimationClip[] clip;

    public AnimatorOverrideController[] aoc;

    private GameObjectRecorder m_Recorder;

    public bool isRecording;

    public int recordIndex;
    public int playIndex;

    void Start()
    {
        // Create recorder and record the script GameObject.
        m_Recorder = new GameObjectRecorder(gameObject);

        // Bind all the Transforms on the GameObject and all its children.
        m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            //Create Animation Clip to save
            AnimationClip animclip = new AnimationClip();
            AssetDatabase.CreateAsset(animclip, "Assets/MyAnim" + recordIndex + ".anim");
            AssetDatabase.SaveAssets();

            //Create Animator Override Controller to play different 
            AnimatorOverrideController animatorOverrideController = new AnimatorOverrideController(gameObject.GetComponent<Animator>().runtimeAnimatorController);
            AssetDatabase.CreateAsset(animatorOverrideController, "Assets/AnimatorOverrideController" + recordIndex + ".overrideController");
            AssetDatabase.SaveAssets();

            //save the dummy clips into the arrays 
            clip[recordIndex] = animclip;
            aoc[recordIndex] = animatorOverrideController;

            //start recording
            isRecording = true;
        }

        if (Input.GetKeyDown(KeyCode.S))
        {
            //stop recording
            isRecording = false;

            //save the recordings into a clip
            m_Recorder.SaveToClip(clip[recordIndex++]);

            //override the controller
            aoc[recordIndex - 1]["Recorded"] = clip[recordIndex - 1];

            //reset the recording and rebind the component
            m_Recorder.ResetRecording();
            m_Recorder.BindComponentsOfType<Transform>(gameObject, true);
        }

        //that is to switch between the clips
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ++playIndex;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            --playIndex;
        }
    }

    void LateUpdate()
    {

        //the variable to regulate the recording
        if (isRecording)
        {
            if (clip == null)
                return;

            // Take a snapshot and record all the bindings values for this frame.
            m_Recorder.TakeSnapshot(Time.deltaTime);
        }
    }

    //We don't need that part
    void OnDisable()
    {
        if (clip == null)
            return;

        if (m_Recorder.isRecording)
        {
            // Save the recorded session to the clip.
            m_Recorder.SaveToClip(clip[recordIndex]);
        }
    }
}
