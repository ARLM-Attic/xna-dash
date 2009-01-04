
using System;
using Microsoft.Xna.Framework.Audio;

namespace XNADash.SoundFx
{
    public class SoundFxManager
    {
        readonly AudioEngine engine;
        readonly SoundBank soundBank;
        WaveBank waveBank;
        private static SoundFxManager instance;

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

        private SoundFxManager()
        {
            // Initialize audio objects.
            engine = new AudioEngine("Content\\Audio\\XNADash_sound.xgs");
            soundBank = new SoundBank(engine, "Content\\Audio\\XNADash.xsb");
            waveBank = new WaveBank(engine, "Content\\Audio\\XNADash.xwb");
        }

        /// <summary>
        /// Gets the instance of the sound effect manager.
        /// This should be used because the class is implented as a singleton
        /// </summary>
        public static SoundFxManager Instance
        {
            get
            {
                if (instance == null)
                    instance = new SoundFxManager();

                return instance;
            }
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
