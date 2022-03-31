using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

public class HeroAnimation : MonoBehaviour
{
    public AnimationClip idle;
    public AnimationClip walk;
    public AvatarMask upMask;
    public AvatarMask downMask;
    public float weight;
    public KeyCode keyCode1;
    public KeyCode keyCode2;
    public KeyCode keyCode3;

    PlayableGraph playableGraph;
    AnimationLayerMixerPlayable layerMixerPlayable;

    AnimationMixerPlayable mixerPlayable;

    public AnimationClip[] fireClipsToPlay;
    public AnimationClip[] tossGrenadeClipsToPlay;
    public AnimationClip[] jumpClipsToPlay;

    ScriptPlayable<PlayQueuePlayable>  fireQueuePlayable;
    ScriptPlayable<PlayQueuePlayable> tossGrenadeQueuePlayable;
    ScriptPlayable<PlayQueuePlayable> jumpQueuePlayable;

    public void Start()
    {

        // 创建该图和混合器，然后将它们绑定到 Animator。

        playableGraph = PlayableGraph.Create();

        //下半身
        var idleClip = AnimationClipPlayable.Create(playableGraph, idle);

        var walkClip = AnimationClipPlayable.Create(playableGraph, walk);
        

        mixerPlayable = AnimationMixerPlayable.Create(playableGraph, 2);
        mixerPlayable.ConnectInput(0, idleClip, 0, 1.0f);
        mixerPlayable.ConnectInput(1, walkClip, 0, 0.5f);

        //上半身
        fireQueuePlayable = ScriptPlayable<PlayQueuePlayable>.Create(playableGraph);

        var playQueue = fireQueuePlayable.GetBehaviour();

        playQueue.Initialize(fireClipsToPlay, fireQueuePlayable, playableGraph);

        fireQueuePlayable.Pause();


        tossGrenadeQueuePlayable = ScriptPlayable<PlayQueuePlayable>.Create(playableGraph);

        playQueue = tossGrenadeQueuePlayable.GetBehaviour();

        playQueue.Initialize(tossGrenadeClipsToPlay, tossGrenadeQueuePlayable, playableGraph);

        tossGrenadeQueuePlayable.Pause();

        jumpQueuePlayable = ScriptPlayable<PlayQueuePlayable>.Create(playableGraph);

        playQueue = jumpQueuePlayable.GetBehaviour();

        playQueue.Initialize(jumpClipsToPlay, jumpQueuePlayable, playableGraph);

        jumpQueuePlayable.Pause();
        
        //层级

        layerMixerPlayable = AnimationLayerMixerPlayable.Create(playableGraph, 2);


        layerMixerPlayable.ConnectInput(0, mixerPlayable, 0, 1f);



        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponentInChildren<Animator>());
        playableOutput.SetSourcePlayable(layerMixerPlayable);

        //播放该图。
        playableGraph.Play();

    }

    public void Update()
    {
        weight = Mathf.Clamp01(weight);
        mixerPlayable.SetInputWeight(0, 1.0f - weight);
        mixerPlayable.SetInputWeight(1, weight);
        

        if (Input.GetKeyUp(keyCode1))
        {
            if (fireQueuePlayable.GetPlayState() == PlayState.Playing)
            {
                layerMixerPlayable.DisconnectInput(1);
                fireQueuePlayable.Pause();
            }
            else
            {
                layerMixerPlayable.ConnectInput(1, fireQueuePlayable, 0, 1f);
                layerMixerPlayable.SetLayerMaskFromAvatarMask(1, upMask);
                layerMixerPlayable.SetLayerAdditive(1, true);
                fireQueuePlayable.Play();
            }
        }
        else if (Input.GetKeyUp(keyCode2))
        {
            if (tossGrenadeQueuePlayable.GetPlayState() == PlayState.Playing)
            {
                layerMixerPlayable.DisconnectInput(1);
                tossGrenadeQueuePlayable.Pause();
            }
            else
            {
                layerMixerPlayable.ConnectInput(1, tossGrenadeQueuePlayable, 0, 1f);

                layerMixerPlayable.SetLayerMaskFromAvatarMask(1, upMask);
                layerMixerPlayable.SetLayerAdditive(1, true);
                tossGrenadeQueuePlayable.Play();
            }
        }
        else if (Input.GetKeyUp(keyCode3))
        {
            if (jumpQueuePlayable.GetPlayState() == PlayState.Playing)
            {
                layerMixerPlayable.DisconnectInput(1);
                jumpQueuePlayable.Pause();
            }
            else
            {
                layerMixerPlayable.ConnectInput(1, jumpQueuePlayable, 0, 1f);
                layerMixerPlayable.SetLayerMaskFromAvatarMask(1, downMask);
                layerMixerPlayable.SetLayerAdditive(1, false);
                jumpQueuePlayable.Play();
            }
        }

    }

    public void OnDestroy()
    {
        playableGraph.Destroy();
    }
}
