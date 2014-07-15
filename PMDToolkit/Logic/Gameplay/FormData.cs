using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PMDToolkit.Maps;

namespace PMDToolkit.Logic.Gameplay {

    public struct FormData {

        public int Species;
        public int Form;
        public Enums.Coloration Shiny;
        public Enums.Gender Gender;


        public FormData(int species, int form, Enums.Gender gender, Enums.Coloration shiny)
        {
            Species = species;
            Form = form;
            Gender = gender;
            Shiny = shiny;
        }

    }
}
