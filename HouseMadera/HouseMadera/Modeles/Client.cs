﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HouseMadera.Modeles
{
    public class Client
    {
        public int Id { get; set; }

        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string Adresse3 { get; set; }
        public string CodePostal { get; set; }
        public string Ville { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Mobile { get; set; }

        public int StatutClient { get; set; }

        public List<Projet> Projets { get; set; }
    }
}
