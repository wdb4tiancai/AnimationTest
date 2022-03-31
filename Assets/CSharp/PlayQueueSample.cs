using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.Animations;

using UnityEngine.Playables;

public class PlayQueueSample : MonoBehaviour
{
    public AnimationClip[] clipsToPlay;

    PlayableGraph playableGraph;

    void Start()

    {

        playableGraph = PlayableGraph.Create();

        var playQueuePlayable = ScriptPlayable<PlayQueuePlayable>.Create(playableGraph);

        var playQueue = playQueuePlayable.GetBehaviour();

        playQueue.Initialize(clipsToPlay, playQueuePlayable, playableGraph);

        var playableOutput = AnimationPlayableOutput.Create(playableGraph, "Animation", GetComponentInChildren<Animator>());

        playableOutput.SetSourcePlayable(playQueuePlayable,0);

        playableGraph.Play();

    }

    void OnDisable()

    {

        // 销毁该图创建的所有可播放项和输出。

        playableGraph.Destroy();

    }
}
