
using System;
using Microsoft.Xna.Framework.Audio;

namespace XNADash.SoundFx
{
    public class SoundFxManager
    {
        AudioEngine engine;
        SoundBank soundBank;
        WaveBank waveBank;

        public enum CueEnums
        {
            applause,
            bump,
            diamond,
            exit,
            menu,
            move,
            start
        }

        public SoundFxManager()
        {
            // Initialize audio objects.
            engine = new AudioEngine("Content\\Audio\\XNADash_sound.xgs");
            soundBank = new SoundBank(engine, "Content\\Audio\\XNADash.xsb");
            waveBank = new WaveBank(engine, "Content\\Audio\\XNADash.xwb");
        }

        public void PlaySound(CueEnums cue)
        {
            switch (cue)
            {
                case CueEnums.applause:
                    soundBank.PlayCue("applause-2");
                    break;
                case CueEnums.bump:
                    soundBank.PlayCue("bump");
                    break;
                case CueEnums.diamond:
                    soundBank.PlayCue("diamond");
                    break;
                case CueEnums.exit:
                    soundBank.PlayCue("exit");
                    break;
                case CueEnums.menu:
                    soundBank.PlayCue("menu");
                    break;
                case CueEnums.move:
                    soundBank.PlayCue("move");
                    break;
                case CueEnums.start:
                    soundBank.PlayCue("start");
                    break;
                default :
                    throw new ArgumentException("Enumeration did not match cue switch");
            }
        }
    }
}
