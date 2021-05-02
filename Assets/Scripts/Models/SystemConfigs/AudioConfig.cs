using System.Collections.Generic;
using Core;
using Models.Audio;
using UnityEngine;

namespace Models.SystemConfigs
{
    public enum AudioLabel
    {
        InvadersMove, 
        GameOver, 
        HitInvaders, 
        HitPlayer,
        Shoot,
        SpecialMove,
        SpecialDie,
        LevelWin,
        NewGame,
        Click
    }
    
    [CreateAssetMenu(menuName = "Game/System Config/Audio System Config")]
    public class AudioConfig : SystemConfig
    {
        [Range(0f,1f)] public float  masterVolume;
        public int                   bufferSize;
        public List<AudioData>       audios;
    }
}