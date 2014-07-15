using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PMDToolkit.Logic.Display {
    public class DrawHelper {

        public static int GetArc(double maxHeight, double touchdownX, double currentX) {
            // = (-4 * m / (n ^ 2) ) * x ^ 2 + (4 * m / n) * x
            // m = height, n = total time, x = current time
            double height = -4 * maxHeight * Math.Pow(currentX / touchdownX, 2) + 4 * maxHeight * (currentX / touchdownX);
            return (int)height;
        }
    }
}
