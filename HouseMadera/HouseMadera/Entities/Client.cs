﻿using System.Collections.Generic;

namespace HomeMadera.Entities
{
    public class Client
    {
        public string Adresse1 { get; set; }
        public string Adresse2 { get; set; }
        public string Adresse3 { get; set; }
        public string CodePostal { get; set; }
        public string Email { get; set; }
        public int Id { get; set; }

        public string Mobile { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public virtual ICollection<Projet> Projets { get; set; }
        public StatutClient StatutClient { get; set; }
        public string Telephone { get; set; }
    }
}