﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoteHighwayWwiseSync : MonoBehaviour
{
    public AK.Wwise.Event noteHighwayWwiseEvent;

    [HideInInspector] public float secondsPerBeat;

    //Unity events are pretty flexible - you've already used them if you've used UI buttons

    //they are also easy to extend if you ever need to pass arguments -- https://docs.unity3d.com/ScriptReference/Events.UnityEvent_1.html

    [Header("These correspond to callback Flags that we're getting from Wwise")]
    public UnityEvent OnR;
    public UnityEvent OnG;
    public UnityEvent OnB;

    public UnityEvent OnLevelEnded;

    public UnityEvent OnEveryGrid;
    public UnityEvent OnEveryBeat;
    public UnityEvent OnEveryBar;

    [Space(10)]
    [Header("These correspond to Midi notes that we're getting from Wwise")]

    public UnityEvent OnSusRStart;
    public UnityEvent OnSusREnd;
    public UnityEvent OnTransientR;

    [Space(10)]

    public UnityEvent OnSusGStart;
    public UnityEvent OnSusGEnd;
    public UnityEvent OnTransientG;

    [Space(10)]

    public UnityEvent OnSusBStart;
    public UnityEvent OnSusBEnd;
    public UnityEvent OnTransientB;


    

    //you might need to do some debugging to figure out which notes you're getting from your midi file, as not every DAW is consistent
    //keep in mind middle C (C4) is 60

    //use F4 (65 if done in Reaper), G and A for held notes, F#, G#, and A# for transient notes
    //these correspond to my R G and B cues respectively (I encourage you to rename these...)
    public const byte sustainR = 65;
    public const byte transientR = 66;
    public const byte sustainG = 67;
    public const byte transientG = 68;
    public const byte sustainB = 69;
    public const byte transientB = 70;


    //id of the wwise event - using this to get the playback position
    uint playingID;

    void Start()
    {

        //most of the time in wwise you just post events and attach them to game objects, 
        
        playingID = noteHighwayWwiseEvent.Post(gameObject,

            //but this is a bit different
            //we want to use a callback - allowing us to set up communication between one system (wwise) and another (unity)

            //wwise gives you the option of sending messages on musically significant times - beats, bars, etc

            (uint)(AkCallbackType.AK_MusicSyncAll
            //we use a bitwise operator - the single | - because there are are a couple things we want communicated.  

            |

            //we also want to get accurate playback position (my tests show it's usually within 5 ms, sometimes as high as 30 ms), which requires a callback as well.
            AkCallbackType.AK_EnableGetMusicPlayPosition | AkCallbackType.AK_MIDIEvent), 
            
            //this is the function we define in code, which will fire whenever we get wwise music events
            MusicCallbackFunction); 

        
    }

    //the music callback gets fed some information from the wwise engine
    void MusicCallbackFunction(object in_cookie, AkCallbackType in_type, AkCallbackInfo in_info)
    {



        //we only want music-specific information, so we cast this info accordingly
        AkMusicSyncCallbackInfo _musicInfo;
        AkMIDIEventCallbackInfo _midiInfo;  
        //check if it's music callback (beat, marker, bar, grid etc)
        if (in_info is AkMusicSyncCallbackInfo)
        {
            //Debug.Log("music callback");
            _musicInfo = (AkMusicSyncCallbackInfo)in_info;

            //we're going to use this switchboard to fire off different events depending on wwise sends
            switch (_musicInfo.musicSyncType)
            {
                case AkCallbackType.AK_MusicSyncUserCue:

                    CustomCues(_musicInfo.userCueName, _musicInfo);

                    break;
                case AkCallbackType.AK_MusicSyncBeat:


                    OnEveryBeat.Invoke();
                    break;
                case AkCallbackType.AK_MusicSyncBar:
                    //I want to make sure that the secondsPerBeat is defined on our first measure.
                    secondsPerBeat = _musicInfo.segmentInfo_fBeatDuration;
                    //Debug.Log("Seconds Per Beat: " + secondsPerBeat);

                    OnEveryBar.Invoke();
                    break;

                case AkCallbackType.AK_MusicSyncGrid:
                    //the grid is defined in Wwise - usually on your playlist.  It can be as small as a 32nd note

                    OnEveryGrid.Invoke();
                    break;
                default:
                    break;


            }
        }

        
        //Using MIDI Beatmaps!
        if (in_info is AkMIDIEventCallbackInfo)
        {

            _midiInfo = (AkMIDIEventCallbackInfo)in_info;
            
            //pay attention to this debug log to get the byte number of your midi notes
            //Debug.Log("MIDI note is: " + _midiInfo.byOnOffNote);

            //note on cue
            if(_midiInfo.byType == AkMIDIEventTypes.NOTE_ON)
            {
                switch(_midiInfo.byOnOffNote)
                {
                    //first check the sustain notes
                    case sustainR:
                        OnSusRStart.Invoke();
                        break;
                    case sustainG:
                        OnSusGStart.Invoke();
                        break;
                    case sustainB:
                        OnSusBStart.Invoke();
                        break;

                    //check the transient notes
                    case transientR:
                        OnTransientR.Invoke();
                        break;
                    case transientG:
                        OnTransientG.Invoke();
                        break;
                    case transientB:
                        OnTransientB.Invoke();
                        break;
                    default:
                        break;

                }
            }
            //called when midi note is released
            else if (_midiInfo.byType == AkMIDIEventTypes.NOTE_OFF)
            {
                switch (_midiInfo.byOnOffNote)
                {
                    case sustainR:
                        OnSusREnd.Invoke();
                        break;
                    case sustainG:
                        OnSusGEnd.Invoke();
                        break;
                    case sustainB:
                        OnSusBEnd.Invoke();
                        break;
                    default:
                        break;

                }
            }
        }   
    }




    public void CustomCues(string cueName, AkMusicSyncCallbackInfo _musicInfo)
    {
        switch (cueName)
        {

            case "R":
                OnR.Invoke();
                break;
            case "G":
                OnG.Invoke();
                break;
            case "B":
                OnB.Invoke();
                break;
            case "LevelEnded":
                OnLevelEnded.Invoke();
                break;
            default:
                break;

        }
    }

    //we use this to evaluate player button presses and see if they are within the scoring windows
    public int GetMusicTimeInMS()
    {

        AkSegmentInfo segmentInfo = new AkSegmentInfo();

        AkSoundEngine.GetPlayingSegmentInfo(playingID, segmentInfo, true);

        return segmentInfo.iCurrentPosition;
    }

    //We're going to call this when we spawn a gem, in order to determine when it's crossing time should be
    //Crossing time is based on the current playback position, our beat duration, and our beat offset
    public int SetCrossingTimeInMS(int beatOffset)
    {
        AkSegmentInfo segmentInfo = new AkSegmentInfo();

        AkSoundEngine.GetPlayingSegmentInfo(playingID, segmentInfo, true);

        int offsetTime = Mathf.RoundToInt(1000 * secondsPerBeat * beatOffset);

        //Debug.Log("setting time: " + segmentInfo.iCurrentPosition + offsetTime);

        return segmentInfo.iCurrentPosition + offsetTime;
    }


}
