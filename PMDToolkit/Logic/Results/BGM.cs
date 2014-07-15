using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DragonOgg.MediaPlayer;

using OpenTK.Audio;
using OpenTK.Audio.OpenAL;
using PMDToolkit.Data;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class BGM : IResult {

        //public ResultType Type { get { return ResultType.BGM; } }
        public RenderTime Delay { get { return RenderTime.Zero; } }

        string newBGM;
        bool fade;

        public BGM(string newBGM, bool fade) {
            this.newBGM = Paths.MusicPath + newBGM;
            this.fade = fade;
        }

        public void Execute() {

            if (Display.Screen.Song != newBGM)
            {
                if (Display.Screen.NextSong != null || Display.Screen.Song == "")
                {
                    //do nothing, and watch it tick down
                } else if (!String.IsNullOrEmpty(AudioManager.BGM.CurrentFile) || !fade) {
                    //otherwise, set up the tick-down
                    Display.Screen.MusicFadeTime = Display.Screen.MUSIC_FADE_TOTAL;
                }
                Display.Screen.NextSong = newBGM;
            }
        }
    }
}
