﻿using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class TypeComposant
    {
        public virtual ICollection<Composant> Composants { get; set; }
        public int Id { get; set; }
        public string Nom { get; set; }
        public Qualite Qualite { get; set; }
    }
}