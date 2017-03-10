﻿using HouseMadera.DAL;
using System;
using System.Collections.Generic;

namespace HouseMadera.Modeles
{
    public class Projet:ISynchronizable
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Reference { get; set; }
        public DateTime UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public Commercial Commercial { get; set; }
        public Client Client { get; set; }
        public DateTime? MiseAJour { get; set; }
        public DateTime? Suppression { get; set; }
        public DateTime? Creation { get; set; }
      

        public bool IsUpToDate<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.MiseAJour == null)
                return true;
            else
                return MiseAJour == modele.MiseAJour;
        }

        public bool IsDeleted<TMODELE>(TMODELE modele) where TMODELE : ISynchronizable
        {
            if (modele.Suppression != null && !Suppression.HasValue)
            {
                Suppression = modele.Suppression;
                return true;
            }

            return false;
        }

        public void Copie(Projet projet)
        {
            Nom = projet.Nom;
            Reference = projet.Reference;
            Commercial = projet.Commercial;
            Client = projet.Client;
            MiseAJour = projet.MiseAJour;
            Suppression = projet.Suppression;
            Creation = projet.Creation;
        }
    }
}