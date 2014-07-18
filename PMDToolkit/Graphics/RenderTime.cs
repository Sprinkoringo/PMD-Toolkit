/*The MIT License (MIT)

Copyright (c) 2014 PMU Staff

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in
all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
THE SOFTWARE.
*/


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
