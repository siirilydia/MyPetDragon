using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
namespace Peli
{
    public class Lemmikki
    {
        #region lemmikkiä koskevat muuttujat

        Musiikit musiikit = new Musiikit();

        public string name;
        public int overAllHealth;
        #region overallhealth-property
        public int OverAllHealth
        {
            get { return overAllHealth; }
            //health ei saa olla yli 100 tai alle 0
            set
            {
                if (value > 100)
                    value = 100;
                else if (value < 0)
                    value = 0;
                overAllHealth = value;
            }
        }
        #endregion
        private int mieliala = 20;
        #region mieliala-property
        public int Mieliala
        {
            get { return mieliala; }
            //mieliala ei saa olla yli 30
            set
            {
                if (value >= 30)
                    value = 30;
                mieliala = value;
            }
        }
        #endregion
        private int hygiene = 20;
        #region hygienia-property
        public int Hygiene
        {
            get { return hygiene; }
            //hygienia ei saa olla yli 30
            set
            {
                if (value >= 30)
                    value = 30;
                hygiene = value;
            }
        }
        #endregion
        private int hunger = 20;
        #region nälkä-property
        public int Hunger
        {
            get { return hunger; }
            //hunger ei saa olla yli 40
            set
            {
                if (value >= 40)
                    value = 40;
                hunger = value;
            }
        }
        #endregion
        public int ikä = 0;

        #endregion

        #region listojen tekeminen

        public List<Pesutapa> dummypesut = new List<Pesutapa>(); // luodaan dummypesut-niminen lista Pesutapa tietotyypillä
        Pesutapa pesu1 = new Pesutapa("hopeashampoo", 7); // luodaan dummypesut listaan hopeashampoo olio
        Pesutapa sieni = new Pesutapa("pesusieni", 5); // luodaan dummypesut listaan pesusieni olio
        Pesutapa kylpy1 = new Pesutapa("vaahtokylpy", 10); // luodaan dummypesut listaan vaahtokylpy olio
        Pesutapa suihku1 = new Pesutapa("suihku", 3); // luodaan dummypesut listaan suihku olio

        public List<Ruoka> ruoat = new List<Ruoka>();
        public List<Pesutapa> pesut = new List<Pesutapa>();
        public List<Leikki> leikit = new List<Leikki>();

        public void LisääDummyPesut()
        {
            //tämä lisää pesutapoja dummylistaan, josta ne arvotaan lemmikille pelin alkaessa
            dummypesut.Add(sieni);
            dummypesut.Add(pesu1);
            dummypesut.Add(kylpy1);
            dummypesut.Add(suihku1);
        }
        #endregion
        public Lemmikki(bool teeuusi)
        {
            //tätä konstruktoria käytetään kun tehdään uusi lemmikki kuolleen tilalle

            LisääDummyPesut(); //metodi lisää dummylistaan pesutavat, tästä arvotaan lemmikille pesutapoja
            this.OverAllHealth = Hygiene + Hunger + Mieliala;

            #region aloituksessa olevan ruoan määrän arpominen

            Random rnd = new Random();
            int ruoanmäärä = rnd.Next(2, 5);

            Ruoka ruoka = new Ruoka("omena", 2);
            Ruoka siemen = new Ruoka("siemen", 1);
            Ruoka salmiakki = new Ruoka("salmiakki", 3);

            for (int i = 0; i <= ruoanmäärä; i++)
            {
                ruoat.Add(ruoka);
                ruoat.Add(siemen);
                ruoat.Add(salmiakki);
            }

            #endregion

            #region aloituksessa olevien pesutapojen arpominen
            int arvotutpesut;

            for (int i = 0; i < 3; i++)
            {
                arvotutpesut = rnd.Next(0, dummypesut.Count);
                pesut.Add(dummypesut[arvotutpesut]);
                dummypesut.Remove(dummypesut[arvotutpesut]);
            }

            #endregion

            #region leikkitapojen lisääminen

            Leikki pallo = new Leikki("pallonpotkiminen", 8);
            Leikki kutitus = new Leikki("kutitus", 7);
            Leikki pelaa = new Leikki("miinaharava", -5);
            Leikki tyyny = new Leikki("tyynysota", 5);
            leikit.Add(kutitus);
            leikit.Add(pallo);
            leikit.Add(pelaa);
            leikit.Add(tyyny);

            #endregion
        }

        public Lemmikki()
        {
            // tyhjä konstruktori, tätä käytetään tiedoston lukemisessa
        }

        public int LaskeOverall()
        {
            OverAllHealth = hygiene + mieliala + hunger; //laskee lemmikin healthit yhteen kolmelta eri osa-alueelta
            return OverAllHealth;
        }

        #region eläimen hoitamiseen liittyvät metodit
        public bool Pese(string pesu)
        {
            bool löytyykö = true;

            for (int i = 0; i < pesut.Count; i++)
            {
                if (pesut[i].Nimi.Equals(pesu))
                {

                    löytyykö = true;
                    Hygiene += pesut[i].Pisteet;
                    LaskeOverall();
                    break;
                }
                else
                    löytyykö = false;
            }
            return löytyykö;
        }
        public bool Syötä(string ruoka)
        {
            bool löytyykö = true;
            for (int i = 0; i < ruoat.Count; i++)
            {
                if (ruoat[i].ruoanNimi.Equals(ruoka))
                {
                    Hunger += ruoat[i].pisteet;
                    ruoat.Remove(ruoat[i]);
                    LaskeOverall();
                    löytyykö = true;

                    musiikit.PositiivinenPalauteMusa();

                    break;
                }
                else
                    löytyykö = false;
            }
            return löytyykö;
        }
        public bool Leiki(string leikki)
        {
            bool löytyykö = true;
            for (int i = 0; i < leikit.Count; i++)
            {
                if (leikit[i].nimi.Equals(leikki))
                {
                    Mieliala += leikit[i].pisteet;
                    LaskeOverall();
                    löytyykö = true;

                    musiikit.PositiivinenPalauteMusa();

                    break;
                }
                else
                    löytyykö = false;
            }
            return löytyykö;
        }
        internal void Paijaa()
        {
            Mieliala += 3;
            musiikit.PositiivinenPalauteMusa();
            LaskeOverall();
        }
        public void Harjaa()
        {
            Mieliala += 2;
            hygiene += 3;
            musiikit.PositiivinenPalauteMusa();
            LaskeOverall();
        }
        #endregion

        #region tallennus ja tiedoston luku
        public void Tallenna(Lemmikki tallennettava)
        {
            //tallennetaan lemmikki-olion tiedot xml filuun serialize-metodilla
            XmlSerializer serializerTallenna = new XmlSerializer(typeof(Lemmikki));
            using (StreamWriter myWriter = new StreamWriter(@"..\..\TallennusXML\Tallennus.xml", false))
            {
                serializerTallenna.Serialize(myWriter, tallennettava);
            }
        }
        public Lemmikki LataaTallennettu<Lemmikki>()
        {
            //ladataan tallennetun lemmikki-olion tiedot deserialize-metodilla

            XmlSerializer serializerLataa = new XmlSerializer(typeof(Lemmikki));

            Lemmikki luettu = default(Lemmikki);

            if (string.IsNullOrEmpty(@"..\..\TallennusXML\Tallennus.xml")) return default(Lemmikki);
            try
            {
                StreamReader xmlStream = new StreamReader(@"..\..\TallennusXML\Tallennus.xml");
                luettu = (Lemmikki)serializerLataa.Deserialize(xmlStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            return luettu;
        }

        #endregion
    }
}