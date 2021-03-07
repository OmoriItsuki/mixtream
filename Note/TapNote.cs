using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class TapNote : Note
{
    public override TapStyle TapStyle { get; } = TapStyle.Tap;

    public TapNote(float startTime, Lane lane, NotesAttribute attribute) : 
        base(startTime, lane, attribute){}
}
