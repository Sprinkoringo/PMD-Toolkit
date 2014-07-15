using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Graphics
{
    public struct RenderTime
    {
        public int Ticks;

        public int ToMillisecs()
        {
            return Ticks / TextureManager.FPS_CAP;
        }

        public RenderTime(int ticks)
        {
            Ticks = ticks;
        }

        public static RenderTime FromMillisecs(int millisecs)
        {
            return new RenderTime(millisecs * TextureManager.FPS_CAP);
        }

        public static RenderTime Zero
        {
            get { return new RenderTime(); }
        }


        public static bool operator >(RenderTime param1, RenderTime param2)
        {
            return (param1.Ticks > param2.Ticks);
        }

        public static bool operator >=(RenderTime param1, RenderTime param2)
        {
            return (param1.Ticks > param2.Ticks || param1.Ticks == param2.Ticks);
        }

        public static bool operator <(RenderTime param1, RenderTime param2)
        {
            return (param1.Ticks < param2.Ticks);
        }

        public static bool operator <=(RenderTime param1, RenderTime param2)
        {
            return (param1.Ticks < param2.Ticks || param1.Ticks == param2.Ticks);
        }

        public static bool operator ==(RenderTime param1, RenderTime param2)
        {
            return (param1.Ticks == param2.Ticks);
        }

        public static bool operator !=(RenderTime param1, RenderTime param2)
        {
            return !(param1 == param2);
        }

        public static RenderTime operator +(RenderTime param1, RenderTime param2)
        {
            return new RenderTime(param1.Ticks + param2.Ticks);
        }

        public static RenderTime operator -(RenderTime param1, RenderTime param2)
        {
            return new RenderTime(param1.Ticks - param2.Ticks);
        }

        public static RenderTime operator *(RenderTime param1, RenderTime param2)
        {
            return new RenderTime(param1.Ticks * param2.Ticks);
        }

        public static RenderTime operator /(RenderTime param1, RenderTime param2)
        {
            return new RenderTime(param1.Ticks / param2.Ticks);
        }

        public static RenderTime operator %(RenderTime param1, RenderTime param2)
        {
            return new RenderTime(param1.Ticks % param2.Ticks);
        }



        public static RenderTime operator +(RenderTime param1, int param2)
        {
            return new RenderTime(param1.Ticks + param2);
        }

        public static RenderTime operator -(RenderTime param1, int param2)
        {
            return new RenderTime(param1.Ticks - param2);
        }

        public static RenderTime operator *(RenderTime param1, int param2)
        {
            return new RenderTime(param1.Ticks * param2);
        }

        public static RenderTime operator /(RenderTime param1, int param2)
        {
            return new RenderTime(param1.Ticks / param2);
        }

    }
}
