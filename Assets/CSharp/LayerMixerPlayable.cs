using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Playables;
using UnityEngine.Animations;

public class LayerMixerPlayable : MonoBehaviour
{
    public AnimationClip idle;
    public AnimationClip walk;
    public AnimationClip attack;
    public AvatarMask mask;
    public float weight;
    public float mixLevel = 0.5f;
    public KeyCode keyCode;

    PlayableGraph playableGraph;
    AnimationLayerMixerPlayable layerMixerPlayable;

    AnimationMixerPlayable mixerPlayable;



    public void Start()
    {

        // 创建该图和混合器，然后将它们绑定到 Animator。

        playableGraph = PlayableGraph.Create();

        
        var idleClip = AnimationClipPlayable.Create(playableGraph, idle);

        var walkClip = AnimationClipPlayable.Create(playableGraph, walk);

        var attackClip = AnimationClipPlayable.Create(playableGraph, attack);


        mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);
        mixerPlayable.ConnectInput(0, idleClip, 0, 1.0f);
        mixerPlayable.ConnectInput(1, walkClip, 0, 0.5f);


        layerMixerPlayable = AnimationLayerMixerPlayable.Create(playableGraph, 2);


        layerMixerPlayable.ConnectInput(0, mixerPlayable, 0, 1.0f);
        layerMixerPlayable.ConnectInput(1, attackClip, 0, 0.5f);

        layerMixerPlayable.SetLayerMaskFromAvatarMask(1, mask);
        layerMixerPlayable.SetLayerAdditive(1, true);

        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponent<Animator>());
        playableOutput.SetSourcePlayable(layerMixerPlayable);

        //播放该图。
        //playableGraph.Play();

    }

    public void Update()
    {
        weight = Mathf.Clamp01(weight);
        mixerPlayable.SetInputWeight(0, 1.0f - weight);
        mixerPlayable.SetInputWeight(1, weight);


        mixLevel = Mathf.Clamp01(mixLevel);
        layerMixerPlayable.SetInputWeight(1, mixLevel);

        if (Input.GetKeyUp(keyCode))
        {
            if (playableGraph.IsPlaying())
            {
                playableGraph.Stop();
            }
            else
            {
                playableGraph.Play();
            }
        }

    }

    public void OnDestroy()
    {
        playableGraph.Destroy();
    }
}