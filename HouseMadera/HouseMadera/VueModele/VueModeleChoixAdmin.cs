﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.Modeles;
using HouseMadera.Vues;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    class VueModeleChoixAdmin : ViewModelBase
    {
        public ICommand AdminProjet { get; private set; }
        public ICommand AdminClient { get; private set; }
        public ICommand Logout { get; private set; }


        [PreferredConstructor]
        public VueModeleChoixAdmin()
        {
            Logout = new RelayCommand(Deco);
            AdminProjet = new RelayCommand(AProjet);
            AdminClient = new RelayCommand(AClient);
        }

        private Commercial commercialConnecte;
        public Commercial CommercialConnecte
        {
            get { return commercialConnecte; }
            set { commercialConnecte = value; }
        }

        private async void Deco()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            if (window != null)
            {
                var result = await window.ShowMessageAsync("Avertissement", "Voulez-vous vraiment vous déconnecter ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                {
                    AffirmativeButtonText = "Oui",
                    NegativeButtonText = "Non",
                    AnimateHide = false,
                    AnimateShow = true
                });

                if (result == MessageDialogResult.Affirmative)
                {
                    VueLogin vl = new VueLogin();
                    vl.Show();
                    window.Close();
                }
            }
        }

        private void AClient()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            // TODO : lancer fenetre admin client, fermer cette vue 
        }

        private void AProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();
            VueChoixProjet vcp = new VueChoixProjet();
            ((VueModeleChoixProjet)vcp.DataContext).CommercialConnecte = CommercialConnecte;
            vcp.Show();
            window.Close();
        }
    }
}