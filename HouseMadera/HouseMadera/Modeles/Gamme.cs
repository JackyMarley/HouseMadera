﻿using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class Gamme
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public Finition Finition { get; set; }
        public Isolant Isolant { get; set; }
    }
}
