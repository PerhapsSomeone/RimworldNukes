using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.Sound;

namespace Nukes
{
    static class SoundManager
    {
        public static void playSound(SoundDef sound, Map map, Thing thing)
        {
            sound.PlayOneShot((SoundInfo)new TargetInfo(thing.Position, map, false));
        }
    }
}
