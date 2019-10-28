using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peli
{

    public class Kartta
    {

        public KoordinaattiMääreet Sijainti { get; set; }

        public List<Ruoka> dummyruoat = new List<Ruoka>(); 
        // Ensin luodaan dummyruoat lista jonne lisätään kaikki löydettävät ruoat, 
        // eli kaikki vaihtoehdot jotka voi löytää kartasta, tässä tapauksessa siis alempana
        // olevat RandomRuokaa konstruktorin sisällä olevat: omena, siemen ja salmiakki

        public List<Ruoka> löydetyt = new List<Ruoka>(); 
        // Uusi lista jonne lisätään löydetyt ruoat jotka on random-
        // generoitu vaihtoehdoista jotka olivat dummyruoat listan sisällä

        int palkinto = 0;

        #region löydettävien ruokien lisääminen dummy-listaan ja löydetyt-listan tyhjennys
        public void RandomRuokaa()
        {
            Ruoka ruoka = new Ruoka("omena", 2); // luodaan omena
            dummyruoat.Add(ruoka); // lisätään omena dummyruoat listaan
            Ruoka siemen = new Ruoka("siemen", 1); // luodaan siemen
            dummyruoat.Add(siemen); // lisätään siemen dummyruoat listaan
            Ruoka salmiakki = new Ruoka("salmiakki", 3); // luodaan salmiakki
            dummyruoat.Add(salmiakki); // lisätään salmiakki dummyruoat listaan
        }

        public void TyhjennäLöydettyjenLista() 
        {
            foreach (var ruoka in löydetyt)
            { // tyhjennetään löydetyt lista jotta lista olisi aina tyhjä kun karttapeli käynnistetään uudelleen
              // eikä kävisi niin että kerran löydetyt tavarat lisätään aina vaan uudelleen ja uudelleen
              // lemmikin varastoinventoryyn kun karttapeli lopetetaan
                löydetyt.Remove(ruoka);
            }
        }
        #endregion

        #region pelin toiminta, palauttaa listan löydetyistä ruoista
        public List<Ruoka> NäytäKartta()
        {

            #region peliin liittyvien muuttujien asettaminen ja pelin käynnistys

            Console.OutputEncoding = Encoding.UTF8;  //mahdollistetaan emojien näyttäminen utf8-koodilla
            RandomRuokaa(); 
            TyhjennäLöydettyjenLista();

            Console.WindowHeight = 26; // määritellään konsoli-ikkunan korkeus
            Console.WindowWidth = 64; // määritellään konsoli-ikkunan leveys
            int näytönleveys = Console.WindowWidth;
            int näytönkorkeus = Console.WindowHeight;

            Random randomnumber = new Random(); // kutsutaan random luokkaa

            int itemix = randomnumber.Next(1, näytönleveys - 2); //ensimmäisen tavaran lokaatio kun peli käynnistyy
            int itemiy = randomnumber.Next(1, näytönkorkeus - 2); 

            //löydettyjen itemien määrä on nyt 0
            int itemisumma = 0;

            do
            {
                KarttaPeli();
            } // Suorittaa karttapeliä niin kauan kunnes kartasta on löydetty kolme tavaraa
            while (itemisumma < 3);
            #endregion

            #region pelin toiminnallisuus

            void KarttaPeli() // Peli käynnistyy
            {
                Taustaväri();
                Itemit();

                Sijainti = new KoordinaattiMääreet()
                {
                    X = 0,
                    Y = 0
                };

                Liikkuminen(0, 0);

                ConsoleKeyInfo Näppäimet; // kuvataan käyttäjän painamia näppäimiä
                while ((Näppäimet = Console.ReadKey(true)).Key != ConsoleKey.Escape)
                {
                    switch (Näppäimet.Key)
                    {
                        case ConsoleKey.UpArrow: // nuoli ylöspäin
                            Liikkuminen(0, -1);
                            break;

                        case ConsoleKey.RightArrow: // nuoli oikealle
                            Liikkuminen(1, 0);
                            break;

                        case ConsoleKey.DownArrow: // nuoli alaspäin
                            Liikkuminen(0, 1);
                            break;

                        case ConsoleKey.LeftArrow: // nuoli vasemmalle
                            Liikkuminen(-1, 0);
                            break;
                    }

                    if (itemix == Sijainti.X && itemiy == Sijainti.Y) // jos lemmikki ja tavara on samassa kohtaa
                        
                    {
                        itemix = randomnumber.Next(1, näytönleveys - 2); //arvotaan tavaroille uusi paikka
                        itemiy = randomnumber.Next(1, näytönkorkeus - 2);
                        Console.Beep(750, 550); // Piippausääni kun kartasta löytää tavaran

                        itemisumma++;

                        Random random = new Random(); // kutsutaan random luokkaa
                        palkinto = random.Next(0, dummyruoat.Count); 

                        löydetyt.Add(dummyruoat[palkinto]); // lisätään löydetty tavara dummyruoat listaan

                        Itemit(); // kutsutaan Itemit metodia

                        return;
                    }
                }

                void Itemit()
                {
                    Console.SetCursorPosition(itemix, itemiy);
                    Console.BackgroundColor = ConsoleColor.Red; // Taustaväri kartassa liikkuvan lemmikin alle
                    Console.Write("X"); // kuva joka piirtyy kartassa liikkuvan lemmikin päälle
                }
            }
            return löydetyt;
            #endregion
        }
        #endregion

        #region pelin käyttämiä metodeita
        public void Liikkuminen(int x, int y)
        {
            KoordinaattiMääreet newSijainti = new KoordinaattiMääreet()
            {
                X = Sijainti.X + x,
                Y = Sijainti.Y + y
            };

            if (Liiku(newSijainti))  
            {                       // kysyy Liiku metodilta onko lemmikki liikkunut ja jos on niin silloin :
                PolkuPerässä(); // kutsutaan metodia joka piirtää janaa kartassa liikkuvan lemmikin perässä

                Console.BackgroundColor = ConsoleColor.Red;
                Console.SetCursorPosition(newSijainti.X, newSijainti.Y);
                Console.Write("■"); // Tässä määritellään kuva lemmikin päällä kartassa

                Sijainti = newSijainti;
            }
        }

        public void PolkuPerässä() // Tämä Piirtää väriä ja reittiä lemmikin perässä, eli tummanvihreää
        {
            //Console.OutputEncoding = Encoding.UTF8;
            //var jalanjäljet = char.ConvertFromUtf32(0x1F642);

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.SetCursorPosition(Sijainti.X, Sijainti.Y);
            Console.Write('\u2665'); // printtaa jalanjäljet kartassa liikkuvan lemmikin perään
            //Console.Write("👣");
        }

        public bool Liiku(KoordinaattiMääreet koordinaatti) // Liikkuminen kartalla
        { // katsoo jos lemmikki on liikkunut, silloin palauttaa truen 
            if (koordinaatti.X < 0 || koordinaatti.X >= Console.WindowWidth)
                return false; //jos liikuttaa kartalla lemmikkiä

            if (koordinaatti.Y < 0 || koordinaatti.Y >= Console.WindowHeight)
                return false;

            return true;
        }

        public void Taustaväri()
        {
            Console.BackgroundColor = ConsoleColor.Green; // Määrittelee koko konsolisivun/kartan taustavärin, eli vihreä
            Console.Clear(); 
        }

        #endregion
    }

    public class KoordinaattiMääreet
    {
        public int X { get; set; } //X koordinaatti property
        public int Y { get; set; } //Y koordinaatti property
    }




}

