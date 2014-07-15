using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;
using PMDToolkit.Logic.Gameplay;
using PMDToolkit.Logic.Display;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using PMDToolkit.Graphics;

namespace PMDToolkit.Logic.Results {
    public class CreateSpell : IResult {

        //public ResultType Type { get { return ResultType.CreateSpell; } }

        public RenderTime Delay { get { return RenderTime.Zero; } }

        ISpellSprite spell;
        Display.Screen.EffectPriority priority;

        public CreateSpell(ISpellSprite spell, Display.Screen.EffectPriority priority) {
            this.spell = spell;
            this.priority = priority;
        }

        public void Execute() {
            Screen.Effects[priority].Add(spell);
        }

    }
}
