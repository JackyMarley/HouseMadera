﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Ioc;
using HouseMadera.DAL;
using HouseMadera.Modeles;
using HouseMadera.Utilities;
using HouseMadera.Vues;
using iTextSharp.text;
using iTextSharp.text.pdf;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Input;

namespace HouseMadera.VueModele
{
    public class VueModeleDetailsProjet : ViewModelBase
    {

        [PreferredConstructor]
        public VueModeleDetailsProjet()
        {
            WindowLoaded = new RelayCommand(WindowLoadedEvent);
            Deconnexion = new RelayCommand(Deco);
            Retour = new RelayCommand(RetourAdminProjet);
            SupprimerProduit = new RelayCommand(DeleteProduit);
            EditerProduit = new RelayCommand(EditionProduit);
            GenererDevis = new RelayCommand(GenDevis);
            GenererPlan = new RelayCommand(GenPlan);
            SelectedProduitCmd = new RelayCommand(ChangeDetailsProduits);
            NouveauProduit = new RelayCommand(CreerUnProduit);
        }

        public ICommand WindowLoaded { get; private set; }
        public ICommand SupprimerProduit { get; private set; }
        public ICommand Deconnexion { get; private set; }
        public ICommand Retour { get; private set; }
        public ICommand EditerProduit { get; private set; }
        public ICommand GenererDevis { get; private set; }
        public ICommand GenererPlan { get; private set; }
        public ICommand SelectedProduitCmd { get; private set; }
        public ICommand NouveauProduit { get; private set; }

        #region PROPRIETES

        private MetroWindow thisWindow = Application.Current.Windows.OfType<MetroWindow>().FirstOrDefault();

        private MetroWindow vuePrecedente;
        public MetroWindow VuePrecedente
        {
            get { return vuePrecedente; }
            set
            {
                vuePrecedente = value;
            }
        }

        private Produit selectedProduit;
        public Produit SelectedProduit
        {
            get { return selectedProduit; }
            set
            {
                if (selectedProduit != value)
                    selectedProduit = value;
                RaisePropertyChanged(() => SelectedProduit);
            }
        }

        private Projet selectedProjet;
        public Projet SelectedProjet
        {
            get { return selectedProjet; }
            set
            {
                selectedProjet = value;
            }
        }

        private DevisGenere dGen;
        public DevisGenere DGen
        {
            get { return dGen; }
            set { dGen = value; }
        }

        private string devisActuel;
        public string DevisActuel
        {
            get { return devisActuel; }
            set { devisActuel = value; }
        }

        private ObservableCollection<Produit> listeProduit;
        public ObservableCollection<Produit> ListeProduit
        {
            get
            {
                return listeProduit;
            }
            set
            {
                listeProduit = value;
                RaisePropertyChanged(() => ListeProduit);
            }
        }

        private string titreProjet;
        public string TitreProjet
        {
            get { return titreProjet; }
            set
            {
                titreProjet = value;
                RaisePropertyChanged(() => TitreProjet);
            }
        }

        private string detailsPrixTTCProduit;
        public string DetailsPrixTTCProduit
        {
            get { return detailsPrixTTCProduit; }
            set
            {
                detailsPrixTTCProduit = value;
                RaisePropertyChanged(() => DetailsPrixTTCProduit);
            }
        }

        private string detailsPrixProduit;
        public string DetailsPrixProduit
        {
            get { return detailsPrixProduit; }
            set
            {
                detailsPrixProduit = value;
                RaisePropertyChanged(() => DetailsPrixProduit);
            }
        }

        private string detailsStatutDevisProduit;
        public string DetailsStatutDevisProduit
        {
            get { return detailsStatutDevisProduit; }
            set
            {
                detailsStatutDevisProduit = value;
                RaisePropertyChanged(() => DetailsStatutDevisProduit);
            }
        }

        private string detailsStatutProduit;
        public string DetailsStatutProduit
        {
            get { return detailsStatutProduit; }
            set
            {
                detailsStatutProduit = value;
                RaisePropertyChanged(() => DetailsStatutProduit);
            }
        }

        private bool genBtnActif = false;
        public bool GenBtnActif
        {
            get { return genBtnActif; }
            set
            {
                genBtnActif = value;
                RaisePropertyChanged("GenBtnActif");
            }
        }

        private bool isProduitPresent;
        public bool IsProduitPresent
        {
            get { return isProduitPresent; }
            set
            {
                isProduitPresent = value;
                RaisePropertyChanged(() => IsProduitPresent);
            }
        }

        #endregion

        #region METHODES
        private void ChangeDetailsProduits()
        {
            try
            {
                if (selectedProduit.StatutProduit.Nom == "Valide")
                {
                    DetailsPrixProduit = string.Format("Prix HT : {0} €", Convert.ToString(selectedProduit.Devis.PrixHT));
                    DetailsPrixTTCProduit = string.Format("Prix TTC : {0} €", Convert.ToString(selectedProduit.Devis.PrixTTC));
                    DetailsStatutDevisProduit = string.Format("Statut du devis : {0}", Convert.ToString(selectedProduit.Devis.StatutDevis.Nom));
                    DetailsStatutProduit = string.Format("Statut du Produit : {0}", Convert.ToString(selectedProduit.StatutProduit.Nom));
                    GenBtnActif = true;
                }
                else
                {
                    DetailsPrixProduit = " ----- ";
                    DetailsPrixTTCProduit = " ----- ";
                    DetailsStatutDevisProduit = " ----- ";
                    DetailsStatutProduit = string.Format("Statut du Produit : {0}", Convert.ToString(selectedProduit.StatutProduit.Nom));
                    GenBtnActif = true;
                }
            }
            catch (Exception)
            {
                DetailsPrixProduit = " ----- ";
                DetailsPrixTTCProduit = " ----- ";
                DetailsStatutDevisProduit = " ----- ";
                DetailsStatutProduit = " ----- ";
            }
        }

        private async void Deco()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().LastOrDefault();
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
                    vuePrecedente.Show();
                    vuePrecedente.Close();
                    window.Close();
                    vl.Show();

                }
            }
        }

        private void RetourAdminProjet()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            if (window != null)
            {
                vuePrecedente.Show();
                window.Close();
            }
        }

        private void EditionProduit()
        {
            try
            {
                string arg = string.Empty;
                if(SelectedProduit == null )
                    arg = string.Format("{0} {1}", SelectedProjet.Id, 0);
                else
                    arg = string.Format("{0} {1}", SelectedProjet.Id, SelectedProduit.Id);

                Console.WriteLine("envoi des arguments vers unity : " + arg);
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = AppInfo.AppPath + @"\MaderaHouseEditor",
                    Arguments = arg,
                    WindowStyle = ProcessWindowStyle.Normal
                };
                var process = Process.Start(startInfo);
                ThreadPool.QueueUserWorkItem(WaitForProc, process);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Logger.WriteEx(ex);
            }
        }

        private void CreerUnProduit()
        {
            try
            {
                string arg = string.Format("{0}", SelectedProjet.Id);
                Console.WriteLine("envoi des arguments vers unity : " + arg);
                ProcessStartInfo startInfo = new ProcessStartInfo()
                {
                    FileName = AppInfo.AppPath + @"\MaderaHouseEditor",
                    Arguments = arg,
                    WindowStyle = ProcessWindowStyle.Normal
                };
                var process = Process.Start(startInfo);
                ThreadPool.QueueUserWorkItem(WaitForProc, process);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Logger.WriteEx(ex);
            }
        }

        private async void GenDevis()
        { 
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            int remise = 0;
           
            try
            {
                if (SelectedProduit == null)
                    throw new Exception("La génération de devis impossible sur un produit NULL");
                // Génération du devis 
                List<DataGenerationDevis> listDg = new List<DataGenerationDevis>();
                using (DevisDAL dDal = new DevisDAL(DAL.DAL.Bdd))
                {
                    //Obtenir la réduction
                    remise = dDal.CalculerReduction(SelectedProduit.Id);
                    listDg = dDal.GenererDevis(SelectedProduit);
                }
                if (listDg.Count > 0)
                {
                    // Traitement des données
                    List<string> modulesDistincts = new List<string>();
                    foreach (DataGenerationDevis dg in listDg)
                    {
                        if (!modulesDistincts.Contains(dg.NomModule))
                        {
                            modulesDistincts.Add(dg.NomModule);
                        }
                    }

                    string outputToDevis = @"Devis pour " + listDg.First().NomProduit + " le " + DateTime.Now.ToLongDateString() + Environment.NewLine;
                    outputToDevis += string.Format(@"Client : {0} {1}" + Environment.NewLine + Environment.NewLine, listDg.First().Client.Nom, listDg.First().Client.Prenom);
                    outputToDevis += @"Détails des modules sélectionnés :" + Environment.NewLine;
                    decimal prixTotal = 0;
                    decimal tva = 1.2M;
                    List<string> modulesToGrid = new List<string>();
                    foreach (string s in modulesDistincts)
                    {
                        decimal prixModule = 0;
                        foreach (DataGenerationDevis dg in listDg)
                        {
                            if (s == dg.NomModule && !string.IsNullOrEmpty(dg.PrixComposant) && dg.NombreComposant != 0)
                            {
                                prixModule += Convert.ToDecimal(dg.PrixComposant) * dg.NombreComposant;
                            }
                        }
                        prixTotal += prixModule;
                        string outputModule = string.Format("Module : {0} | Prix HT : {1} € \n", s, Convert.ToString(prixModule));
                        modulesToGrid.Add(outputModule);
                        outputToDevis += outputModule;
                    }
                    string mursPorteurs = "4 x Murs low-cost | Prix HT : 4032 € \n";
                    outputToDevis += mursPorteurs;
                    modulesToGrid.Add(mursPorteurs);
                    prixTotal += 4032;
                    decimal prixHT = prixTotal;
                    decimal prixTTC = prixTotal * tva;
                    decimal prixTTCRemise = Remise.CalculerPrixRemise(remise, prixTTC);

                    string prixFinal = string.Format(Environment.NewLine + "Prix Total HT : {0} € | Prix Total TTC : {1} € \n", Convert.ToString(prixTotal), Convert.ToString(prixTTC));
                    string valeurRemise = string.Format(Environment.NewLine + "Remise : {0} %", Convert.ToString(remise));
                    string montant = string.Format(Environment.NewLine + "Montant du devis : {0} €", Convert.ToString(prixTTCRemise));
                    outputToDevis = outputToDevis + prixFinal + valeurRemise + montant;
                   
                    DGen = new DevisGenere()
                    {
                        Output = outputToDevis,
                        PrixHT = Convert.ToString(prixHT),
                        PrixTTC = Convert.ToString(prixTTC),
                        PrixTTCRemise = Convert.ToString(prixTTCRemise),
                        Modules = modulesToGrid,
                        client = listDg.First().Client
                    };

                    // Création du PDF
                    GenererPdfDevis();

                    // Création du devis à insérer en BDD
                    Devis d = new Devis()
                    {
                        DateCreation = DateTime.Now,
                        Nom = listDg.First().NomProduit,
                        PrixHT = prixTotal,
                        PrixTTC = Convert.ToDecimal(prixTTC),
                        StatutDevis = new StatutDevis() { Id = 2 },
                        Pdf = File.ReadAllBytes(AppInfo.AppPath + @"\Devis\" + DevisActuel),
                        MiseAJour = null,
                        Suppression = null,
                        Creation = DateTime.Now
                    };


                    try
                    {
                        int insertDevis = 0;
                        Produit pUpdate = new Produit()
                        {
                            Nom = listDg.First().NomProduit
                        };
                        using (DevisDAL dDAl = new DevisDAL(DAL.DAL.Bdd))
                        {
                            insertDevis = dDAl.InsertDevis(d);
                            pUpdate.Devis = dDAl.GetDevisByIdProduit(pUpdate);
                            Console.WriteLine("update devis " + pUpdate.Nom);
                        }
                        using (ProduitDAL pDal = new ProduitDAL(DAL.DAL.Bdd))
                        {
                            int i = pDal.UpdateDevisProduit(pUpdate);
                            pDal.UpdateStatutProduit(pUpdate);
                            Console.WriteLine("result update devis " + i);
                        }

                        if (insertDevis > 0)
                        {
                            VueGenererDevis vgd = new VueGenererDevis();
                            ((VueModeleGenererDevis)vgd.DataContext).TitreVue = TitreProjet;
                            ((VueModeleGenererDevis)vgd.DataContext).DGen = DGen;
                            ((VueModeleGenererDevis)vgd.DataContext).Remise = string.Format("{0} %",Convert.ToString(remise));
                            ((VueModeleGenererDevis)vgd.DataContext).Montant = string.Format("{0} %", Convert.ToString(prixTTCRemise));
                            vgd.Show();
                        }
                        else
                        {
                            if (window != null)
                            {
                                await window.ShowMessageAsync("Erreur", "Le devis n'a pas été inséré en base.");
                            }
                        }
                    }
                    catch (Exception ex)
                    {

                        await window.ShowMessageAsync("Erreur", ex.Message);
                        Logger.WriteEx(ex);
                    }
                }
                else
                {
                    await window.ShowMessageAsync("Erreur", "La maison ne comporte pas de modules pour la génération du devis.");
                }

            }
            catch (InvalidOperationException)
            {
                await window.ShowMessageAsync("Erreur", "La maison ne comporte pas de modules pour la génération du devis.");
            }
        }

        private string GenererPdfDevis()
        {
            DevisActuel = @"devis" + DateTime.Now.ToString("ddMMyyyyHHmmss") + ".pdf";
            Console.WriteLine(AppInfo.AppPath + @"\Devis\" + DevisActuel);
            if (!Directory.Exists(AppInfo.AppPath + @"\Devis"))
            {
                Directory.CreateDirectory(AppInfo.AppPath + @"\Devis");
            }
            FileStream fs = new FileStream(AppInfo.AppPath + @"\Devis\" + DevisActuel, FileMode.Create);
            Document document = new Document(PageSize.A4, 25, 25, 30, 30);

            PdfWriter writer = PdfWriter.GetInstance(document, fs);
            document.AddAuthor("Madera");
            document.AddCreator("Société Madera");
            document.AddKeywords("Devis généré par l'application Mader'house");
            document.AddTitle("Devis généré le " + DateTime.Now.ToLongDateString());
            document.Open();
            document.Add(new Paragraph(DGen.Output));
            document.Close();
            writer.Close();
            fs.Close();
            return DevisActuel;
        }

        private async void GenPlan()
        {
            var window = Application.Current.Windows.OfType<MetroWindow>().Last();
            var rand = new Random();
           
            try
            {
                var files = Directory.GetFiles(AppInfo.AppPath + @"\Plans\", "*.pdf");
                int i = rand.Next(files.Length);
                if (i == 0)
                {
                    i = 1;
                }
                Process.Start(AppInfo.AppPath + @"\Plans\" + i + ".pdf");
            }
            catch (Exception e)
            {
                Logger.WriteEx(e);
                await window.ShowMessageAsync("Erreur", "Impossible d'ouvrir le plan");
            }
        }

        private void ActualiserTitreForm()
        {
            TitreProjet = @"Détails du projet " + selectedProjet.Nom;
            RaisePropertyChanged(() => TitreProjet);
        }

        private void WindowLoadedEvent()
        {
            Console.WriteLine("window loaded event");
            // Actions à effectuer au lancement du form :
            RecupProduitsParProjet();
            ActualiserTitreForm();
        }

        private void RecupProduitsParProjet()
        {
            IsProduitPresent = true;
            using (var dal = new ProduitDAL(DAL.DAL.Bdd))
            {
                ListeProduit = dal.GetAllProduitsByProjet(selectedProjet);
            }
            RaisePropertyChanged(() => ListeProduit);
            if (ListeProduit != null)
            {
                if (ListeProduit.Count == 0)
                {
                    IsProduitPresent = false;
                }
            }
            else
            {
                IsProduitPresent = false;
            }
        }

        private void WaitForProc(object obj)
        {
            var proc = (Process)obj;
            proc.WaitForExit();
            Console.WriteLine("end proc");
            thisWindow.BeginInvoke(delegate ()
            {
                try
                {
                    ListeProduit.Clear();
                }
                catch (NullReferenceException)
                {
                    // ignore
                }
                RecupProduitsParProjet();
            });
        }

        private async void DeleteProduit()
        {
            if (thisWindow != null)
            {
                if (SelectedProduit != null)
                {
                    MessageDialogResult result = await thisWindow.ShowMessageAsync("Avertissement", "Voulez-vous vraiment supprimer ce produit ?", MessageDialogStyle.AffirmativeAndNegative, new MetroDialogSettings
                    {
                        AffirmativeButtonText = "Oui",
                        NegativeButtonText = "Non",
                        AnimateHide = false,
                        AnimateShow = true
                    });

                    int delProduit = 0;
                    if (result == MessageDialogResult.Affirmative)
                    {
                        using (var dal = new ProduitDAL(DAL.DAL.Bdd))
                        {
                            SelectedProduit.Suppression = new DateTime();
                            SelectedProduit.Suppression = DateTime.Now;
                            delProduit = dal.DeleteModele(SelectedProduit);
                        }

                        if (delProduit > 0)
                        {
                            ListeProduit.Remove(SelectedProduit);
                            RaisePropertyChanged(() => ListeProduit);
                            await thisWindow.ShowMessageAsync("Information", "Le produit est bien marqué pour suppression.");
                        }
                        else
                        {
                            await thisWindow.ShowMessageAsync("Erreur", "Le produit n'a pas pu être supprimé.");

                        }
                    }
                }
                else
                {
                    await thisWindow.ShowMessageAsync("Avertissement", "Merci de sélectionner un produit");
                }

            }
        }

        #endregion



    }
}