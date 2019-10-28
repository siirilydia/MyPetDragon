using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace Peli
{

    public partial class Form1 : Form
    {
        #region konsolipelin asettelu
        //KONSOLIPELIN ASETTELUUN LIITTYVÄ KOODI ALKAA
        private const int HWND_TOPMOST = -1;
        private const int HWND_NOTOPMOST = -2;
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOMOVE = 0x0002;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_SHOWWINDOW = 0x0040;
        const int offsetX = 100;
        [DllImport("kernel32.dll")]
        static extern IntPtr GetConsoleWindow();

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int x, int y, int cx, int cy, int uFlags);

        const int offsetY = 100;

        const int SW_HIDE = 0;
        const int SW_SHOW = 5;
        bool isVisible = true;
        IntPtr hWnd;
        //KONSOLIPELIN ASETTELUUN LIITTYVÄ KOODI LOPPUU
        #endregion

        Musiikit musiikit = new Musiikit();
        Lemmikki lemmikki = new Lemmikki();
        Kartta kartta = new Kartta();
        
        List<Ruoka> löydetyt = new List<Ruoka>(); //tänne lisätään kartta-pelistä löydetyt ruoat
      
        bool vaihdettu = false; //tämä defaulttina false, jotta if-looppiin kääritty switch pyörii

        bool teeuusi = true; //käytetään erottamaan eri konstruktorit tallennetun olion ja uuden olion välillä

        #region vastaantulijan asiaa
        //Pelin vastaantulijaan liittyvää alapuolella
        bool vastaantulija = false;
        bool tehdäänvaihto = false;
        int vastaantulijanruoka = default;
        public List<Ruoka> randomruoat = new List<Ruoka>();
        Ruoka myrkkysieni = new Ruoka("myrkkysieni", -5);
        Ruoka karkki = new Ruoka("karkki", 15);
        #endregion

        #region ohjeet
        //ohjeet, jotka näkyvät defaulttina textboxissa
        string ohjeet = "Yleisimmät komennot:" + Environment.NewLine + Environment.NewLine
            + "syötä x = syötä haluamasi ruoka (esim. syötä omena)" + Environment.NewLine
            + Environment.NewLine
            + "harjaa = harjaa eläintä" + Environment.NewLine
            + Environment.NewLine
            + "paijaa = paijaa eläintä" + Environment.NewLine
            + Environment.NewLine
            + "pese x = pese eläin +" + Environment.NewLine
            + "(esim. pese pesusieni)" + Environment.NewLine
            + Environment.NewLine + "leiki = leiki (kokeile myös):" 
            + Environment.NewLine + "leiki pallo/kutitus/tyynysota"
            + Environment.NewLine + Environment.NewLine
            + "etsi = etsi ruokaa" + Environment.NewLine
            + Environment.NewLine
            + "poistu = tallenna ja poistu" + Environment.NewLine
            + Environment.NewLine
            + "Psst.." + Environment.NewLine + "uskallatko kokeilla:leiki miinaharava ?";
        #endregion

        #region pelin käynnistyksessä tapahtuvia asioita
        public Form1()
        {
            //nämä tapahtuu pelin käynnistyessä
            InitializeComponent();
            hWnd = GetConsoleWindow();
            randomruoat.Add(myrkkysieni);
            randomruoat.Add(karkki);
            textBox3.Text = ohjeet;
            lemmikki = lemmikki.LataaTallennettu<Lemmikki>();
        }
        #endregion

        #region aloita peli-nappula
        public void Button1_Click(object sender, EventArgs e)
        {
            //aloita peli-nappulan toiminnot
            richTextBox1.ReadOnly = true;
            richTextBox1.ScrollBars = RichTextBoxScrollBars.None;
            richTextBox1.Enabled = false;

            NäytäLemmikinKuva();
            NäytäInventory();
            NäytäHealth();
        }
        #endregion

        #region soitetaan musiikkia healthin mukaan
        public void MusiikkiaHealthinMukaan()
        {
            if (lemmikki.OverAllHealth == 100) //tarkistetaan onko lemmikin health == 100
            {
                musiikit.VoittoMusa(); //  jos lemmikin health on tasan 100 niin soitetaan Musiikit luokassa
                                       //  sävelletty voittomusa!
            }
            //if (lemmikki.OverAllHealth >= 21 && lemmikki.OverAllHealth < 100) 
            //{
            //    musiikit.VakioPerusMusa(); //   // Oli alunperin suunnitelmissa lisätä enemmän musiikkia, mutta
            //}                                      piippailut alkoivat rasittamaan ja luovuimme ajatuksesta!

            //if (lemmikki.OverAllHealth == 20 && lemmikki.OverAllHealth < 20)
            //{   // tämä olisi soittanut musiikkia jatkuvasti kun peli päivittyy ja health on 20 tai sen alle
            //    musiikit.Alle20Musa();
            //}
            if (lemmikki.OverAllHealth <= 0) // tarkistetaan onko lemmikin health 0 tai vähemmän
            {
                musiikit.GameOverMusa(); // tämä soittaa musiikkia joka on sävelletty
            }                            // Musiikit luokassa aina kun lemmikin health on 0 
            return;
        }
        #endregion

        #region lemmikin kuvan tulostus mielialan mukaan
        public void NäytäLemmikinKuva() // Lemmikin kuva vaihdetaan sen healthin mukaan
        {
            switch (lemmikki.OverAllHealth)
            {
                case 0:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\0.rtf");
                    break;
                case int n when n <= 10:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\1.rtf");
                    break;
                case int n when n <= 20:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\2.rtf");
                    break;
                case int n when n <= 30:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\3.rtf");
                    break;
                case int n when n <= 40:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\4.rtf");
                    break;
                case int n when n <= 50:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\5.rtf");
                    break;
                case int n when n <= 60:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\6.rtf");
                    break;
                case int n when n <= 70:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\7.rtf");
                    break;
                case int n when n <= 80:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\8.rtf");
                    break;
                case int n when n <= 90:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\9.rtf");
                    break;
                case int n when n < 100:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\10.rtf");
                    break;
                case 100:
                    richTextBox1.LoadFile(@"..\..\Pelikuvat\11.rtf");
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region inventoryn tulostus
        private void NäytäInventory() // näytetään inventory
        {
            List<string> varasto = new List<string>();
            List<int> varastonmäärä = new List<int>();
            bool löytyi = false;
            //ylläolevat ovat varaston näyttämistä "3x asia"-muodossa varten

            label2.Text = default;

            foreach (var r in lemmikki.ruoat)
            {
            //tutkitaan, löytyykö useampi ruoka samalla nimellä, ettei näytetä varastossa liian montaa riviä
                for (int i = 0; i < varasto.Count; i++)
                {
                    if (varasto[i] == r.ruoanNimi)
                    {
                        varastonmäärä[i]++;
                        löytyi = true;
                    }
                }

                if (löytyi != true)
                {
                    varasto.Add(r.ruoanNimi);
                    varastonmäärä.Add(1);
                }
                löytyi = false;
            }

            for (int i = 0; i < varasto.Count; i++)
            {
                //lisätään tuotteen nimen sisältävään stringin perään sen määrä, esim: omena x 3
                varasto[i] += " x" + varastonmäärä[i].ToString();
            }

            foreach (var ruoka in varasto)
            {
                //tulostetaan jokainen rivi, jossa lukee nyt siis ruoan nimi ja sen määrä
                label2.Text += Environment.NewLine + ruoka;
            }
            foreach (var pesu in lemmikki.pesut)
            {
                //tulostetaan jokainen pesutapa inventoryyn
                label2.Text += Environment.NewLine + pesu.Nimi;
            }
        }

        #endregion

        #region healthin tulostus
        private void NäytäHealth()
        {
            //näyttää eläimen healthin, switchillä määrätään tulostettava kuva
            MusiikkiaHealthinMukaan();

            label1.Text = $"Mieliala: {lemmikki.OverAllHealth}";
            switch (lemmikki.OverAllHealth)
            {
                case 0:
                    textBox1.Text = "Lemmikkisi kuoli. Haluatko aloittaa uuden pelin? Vastaa kyllä/ei.";
                    break;
                case int n when n <= 10:
                    textBox1.Text = "Lemmikkisi on huonossa hapessa. TEE JOTAIN!!!";
                    break;
                case int n when n <= 30:
                    textBox1.Text = "Hoidokkisi sinnittelee...";
                    break;
                case int n when n <= 40:
                    textBox1.Text = "Lemmikkisi vointi voisi olla parempikin.";
                    break;
                case int n when n <= 50:
                    textBox1.Text = "Lemmikkisi vointi on kohtalainen.";
                    break;
                case int n when n <= 60:
                    textBox1.Text = "Lemmikkisi näyttää voivan ihan ok!";
                    break;
                case int n when n <= 70:
                    textBox1.Text = "Hyvä meininki!";
                    break;
                case int n when n <= 80:
                    textBox1.Text = "Teillä pyyhkii hyvin!";
                    break;
                case int n when n <= 90:
                    textBox1.Text = "Lemmikkisi on tyytyväinen. Erinomaista työtä!";
                    break;
                case int n when n < 100:
                    textBox1.Text = "Olet etevä lohikäärmeenhoitaja!";
                    break;
                case 100:
                    textBox1.Text = "Perfection";
                    break;
                default:
                    break;
            }
        }
        #endregion

        #region iän tulostus
        private void NäytäIkä()
        {
            label3.Text = $"Ikä :{lemmikki.ikä.ToString()} vuotta";
        }
        #endregion

        #region ok-napin klikkaus, käyttäjän syötteen lukeminen
        private void Button2_Click(object sender, EventArgs e)
        {
            //OK-buttonia klikattu, kutsutaan metodia jossa käsitellään textbox2:sen input
            var input = textBox2.Text;
            Button2Klikattu(input);
        }

        public void Button2Klikattu(string input)
        {
            //muutetaan input pieneksi typojen korjaamiseksi, ja splitataan input välilyönnin kohdalta
            input = input.ToLower();
            var splitattu = input.Split(' ');

            if (tehdäänvaihto == true)
            {
                //jos on tehty vaihtokauppa vastaantulijan kanssa, tapahtuu tämä buttonia painettaessa
                string vaihdettava = textBox2.Text;
                TeeVaihtoKauppa(vaihdettava);
                tehdäänvaihto = false;
                vaihdettu = true;
            }
            if (!vaihdettu)
            {
                //jos ei ole tehty vaihtokauppaa, niin tutkitaan, mitä käyttäjä halusi tehdä?
                switch (splitattu[0])
                {
                    #region hoitoon liittyvät käskyt
                    case "harjaa":
                        if (splitattu.Length == 1)
                            lemmikki.Harjaa();
                        break;
                    case "pese":
                        if (splitattu.Length == 1)
                            textBox2.Text = "Et antanut esineen nimeä";
                        else {
                            bool onko = lemmikki.Pese(splitattu[1]);
                            if (!onko)
                                textBox2.Text = $"{splitattu[1]} ei ole varastossa";
                        }
                        break;
                    case "syötä":
                        if (splitattu.Length == 1)
                            textBox2.Text = "Et antanut esineen nimeä";
                        else {
                            bool löytyykö = lemmikki.Syötä(splitattu[1]);
                            if(!löytyykö)
                            textBox2.Text = $"{splitattu[1]} ei ole varastossa";
                        }
                        break;
                    case "leiki":
                        if (splitattu.Length == 1)
                            lemmikki.Harjaa();
                        else
                        {
                        bool jooko = lemmikki.Leiki(splitattu[1]);
                        if (!jooko)
                            textBox2.Text = $"{splitattu[1]} ei ole varastossa";
                        }
                        break;
                        
                    case "paijaa":
                        if (splitattu.Length == 1)
                            lemmikki.Paijaa();
                        break;

                    case "potki":
                        textBox2.Text = "Lopeta!! >:(";
                        LaskeMieliAlaa();
                        break;

                    #endregion
                    #region pelin käynnistäminen
                    case "etsi":
                        //etsi-käsky käynnistää pelin.
                        //ensin asetetaan konsoli-ikkuna näkyväksi ja määritellään sen sijainti
                        SetWindowPos(hWnd, IntPtr.Zero.ToInt32(), 100, 100, 0, 0, SWP_NOZORDER | SWP_NOSIZE | (isVisible ? SW_SHOW : SW_HIDE));
                        ShowWindow(hWnd, isVisible ? SW_SHOW : SW_HIDE);
                        isVisible = !isVisible;

                        //käynnistetään peli, löydetyt-lista sisältää ruoka-oliot jotka pelistä palautetaan
                        löydetyt = kartta.NäytäKartta();

                        //peli päättyi, asetetaan konsoli-ikkuna piilotetuksi
                        SetWindowPos(hWnd, IntPtr.Zero.ToInt32(), 100, 100, 0, 0, SWP_NOZORDER | SWP_NOSIZE | (isVisible ? SW_HIDE : SW_SHOW));
                        ShowWindow(hWnd, isVisible ? SW_SHOW : SW_HIDE);
                        isVisible = !isVisible;

                        //Tulostetaan löydetyt ruoat ja lisätään ne inventoryyn
                        textBox3.Text = "Jee! Löysit seuraavat asiat:" + Environment.NewLine;

                        foreach (var ruoka in löydetyt)
                        {
                            lemmikki.ruoat.Add(ruoka);
                            textBox3.Text += ruoka.ruoanNimi + Environment.NewLine;
                        }
                        //kun löydetyt ruoat on lisätty inventoryyn, tyhjennetään lista,
                        //jotta ensi kerralla tulee taas uudet löydökset
                        for (int i = löydetyt.Count - 1; i >= 0; i--)
                        {
                            löydetyt.Remove(löydetyt[i]);
                        }

                        //kutsutaan vastaantulija-metodia, tämä olisi mahdollista tehdä myös arpomalla
                        VastaanTulija();

                        break;

                    #endregion
                    #region kyllä/ei/poistu
                    case "kyllä":
                        //jos lemmikin health oli 0, on vastaus restarttaamiseen ollut kyllä
                        if (lemmikki.OverAllHealth == 0)
                        {
                            Lemmikki uusilemmikki = new Lemmikki(teeuusi);
                            lemmikki = uusilemmikki;
                        }

                        //jos vastaantulijan kanssa suostuttiin vaihtokauppaan, kysytään, mitä haluaa vaihtaa
                        if (vastaantulija == true)
                        {
                            tehdäänvaihto = true;

                            textBox3.Text += Environment.NewLine + Environment.NewLine
                                + "Minkä ruoan haluat antaa vaihdossa?";
                            vastaantulija = false;
                        }
                        break;

                    case "ei":
                        //vastattu ei joko restarttiin tai vaihtokauppaan. Tulostetaan ohjeet näkyville
                        textBox3.Text = ohjeet;
                        break;

                    case "poistu":
                        //tallennetaan ja suljetaan peli
                        lemmikki.Tallenna(lemmikki);
                        this.Close();
                        break;

                    default:

                        foreach (var item in lemmikki.ruoat)
                        {
                            if (splitattu[0] == item.ruoanNimi)
                                tehdäänvaihto = true;
                        }

                        if (!tehdäänvaihto)
                            textBox2.Text = "Virheellinen komento!";
                        break;

                        #endregion 
                }
            }

            //jos vaihtokauppa oli tehty, niin ylläoleva switch skipattiin
            //tämä siksi, ettei switchissä luulla syötteen olevan virheellinen,
            //kun kirjoitetaan että mikä tuote vaihdetaan

            //vaihdon ja switchin skippaamisen jälkeen on bool vaihdettu false,
            //jolloin seuraavalla klikkauksella switch taas toimii.

                else
                vaihdettu = false;

            //switchissä tehtiin asioita käyttäjän syötteen perusteella, oletettavasti inventory ja health muuttuivat
            //tulostetaan siis päivitettynä kuva, inventory ja health ajan tasalle
            NäytäLemmikinKuva();
            NäytäInventory();
            NäytäHealth();
            NäytäIkä();
        }
        #endregion

        #region vastaantulijan metodit
        private void VastaanTulija()
        {
            //tätä kutsutaan pelin pelaamisen jälkeen
            vastaantulija = true;

            //arvotaan vastaantulijalle random ruoka listasta, joka on tehty aiemmin
            Random rnd = new Random();
            vastaantulijanruoka = rnd.Next(0, randomruoat.Count);
            Console.Beep(750, 550);
            Console.Beep(800, 500);
            Thread.Sleep(10);
            Console.Beep(550, 500);
            Thread.Sleep(10);
            Console.Beep(640, 500);
            Thread.Sleep(10);
            Console.Beep(880, 500);
            //kerrotaan käyttäjälle, mitä tapahtui ja kysytään, haluaako tehdä vaihtokaupan?
            textBox3.Text += Environment.NewLine + "Oho! Löysit vastaantulijan." + Environment.NewLine +
                $"Hänellä on {randomruoat[vastaantulijanruoka].ruoanNimi} ja hän haluaisi tehdä kanssasi vaihtokaupan." + Environment.NewLine
                + "Haluatko vaihtaa? Vastaa kyllä/ei.";
        }

        private void TeeVaihtoKauppa(string vaihdettava)
        {
            //käyttäjä halusi tehdä vaihtokaupan. Parametrina on käyttäjän syöttämä asia, jonka hän haluaa vaihtaa.
            for (int i = 0; i < lemmikki.ruoat.Count; i++)
            {
                if (lemmikki.ruoat[i].ruoanNimi.Equals(vaihdettava))
                {
                    //jos/kun käyttäjän vaihdossa tarjoamaa vastaava ruoka on inventoryssa,
                    //poistetaan se inventorysta ja lisätään inventoryyn vastaantulijalle arvottu ruoka.
                    lemmikki.ruoat.Remove(lemmikki.ruoat[i]);
                    lemmikki.ruoat.Add(randomruoat[vastaantulijanruoka]);
                    break;
                }
            }

            //tulostetaan ohjeet taas näkyville ja näytetään päivitetty inventory
            textBox3.Text = ohjeet;
            vastaantulija = false;
            NäytäInventory();
        }

        #endregion

        #region ajastettu mielialan laskeminen (tai potkimisesta)
        private void LaskeMieliAlaa()
        {
            //lasketaan lemmikin mielialaa. Jos arvo on jo 0, ei lasketa enempää, ettei tule negatiivinen arvo.

            if (lemmikki.Mieliala > 0)
                lemmikki.Mieliala = lemmikki.Mieliala - 1;

            if (lemmikki.Hygiene > 0)
                lemmikki.Hygiene = lemmikki.Hygiene - 1;

            if (lemmikki.Hunger > 0)
                lemmikki.Hunger = lemmikki.Hunger - 1;

            lemmikki.OverAllHealth = lemmikki.Mieliala + lemmikki.Hygiene + lemmikki.Hunger;
            NäytäLemmikinKuva();
            NäytäHealth();
        }

        #endregion

        #region timerit
        private void Timer1_Tick(object sender, EventArgs e)
        {
            //kun timerissa määritetty aika on kulunut, kutsutaan LaskeMieliAlaa-metodia
            LaskeMieliAlaa();
        }

        private void Timer2_Tick(object sender, EventArgs e)
        {
            //kun timerissa määritetty aika on kulunut, nostetaan lemmikin ikää yhdellä vuodella
            lemmikki.ikä++;
            NäytäIkä();
        }
        #endregion

        #region TyhjiäMetodeita
        private void Label1_Click_1(object sender, EventArgs e)
        {
        }
        private void Label2_Click(object sender, EventArgs e)
        {
        }
        private void Label3_Click(object sender, EventArgs e)
        {
        }
        private void Label3_Click_1(object sender, EventArgs e)
        {
        }
        private void TextBox1_TextChanged(object sender, EventArgs e)
        {
        }
        private void TextBox2_TextChanged(object sender, EventArgs e)
        {
        }
        private void TextBox3_TextChanged(object sender, EventArgs e)
        {
        }
        private void Form1_Load(object sender, EventArgs e)
        {
        }
        private void RichTextBox1_TextChanged(object sender, EventArgs e)
        {
        }
        #endregion
    }
}
