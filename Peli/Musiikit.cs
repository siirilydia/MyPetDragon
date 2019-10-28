using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peli
{
    public class Musiikit
    {
        public string Musiikki { get; set; }
        public Musiikit()
        {
            
        }

        public void VoittoMusa() // Musiikki jota soitetaan kun lemmikin health menee 100%!
        {
            Console.Beep(2500, 400);
            Console.Beep(2500, 200);
            Console.Beep(2500, 200);
            Console.Beep(3200, 300);
            //Console.Beep(3200, 200);
            //Console.Beep(3200, 200);
            Console.Beep(3200, 400);
            Console.Beep(3200, 300);
            Console.Beep(2500, 200);
            Console.Beep(2500, 200);
            //Console.Beep(3200, 400);
            //Console.Beep(2500, 500);
            //Console.Beep(2500, 300);
            Console.Beep(2500, 300);
            Console.Beep(3200, 1000);
        }

        public void GameOverMusa() // Musiikki jota soitetaan kun lemmikki kuolee
        {
            Console.Beep(500, 1000);
        }

        public void PositiivinenPalauteMusa()
        {
            Console.Beep(800, 100);
        }
    }
}



//public void VakioPerusMusa() // Musiikki jota soitetaan vähän joka tilanteessa, mutta EI KÄYTÖSSÄ
//{
//    Console.Beep(3200, 140);
//    Console.Beep(3200, 140);
//    Console.Beep(3200, 700);
//}

//public void Alle20Musa() // Musiikki jota soitetaan kun health menee alle 20, mutta EI KÄYTÖSSÄ
//{
//    Console.Beep(2500, 300);
//    Console.Beep(2500, 300);
//    Console.Beep(2000, 300);
//    Console.Beep(2500, 300);
//    Console.Beep(2500, 300);
//    Console.Beep(2000, 300);
//    Console.Beep(2000, 300);
//    Console.Beep(2500, 300);
//    Console.Beep(2500, 300);
//}