using Microsoft.VisualStudio.TestTools.UnitTesting;
using Peli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Peli.Tests
{
    [TestClass()]
    public class Form1Tests
    {
        //[TestMethod()]   MITEN TÄMÄ TOIMIMAAN ??????
        //public void Button2KlikattuTest()
        //{
        //    var form1 = new Form1();
        //    form1.Button2Klikattu("harjaa");
        //    Assert.AreEqual(result, form1.Button2Klikattu);
        //}

        [TestMethod]
        public void VastaLuodunLemmikinOverAllHealthOnNolla()
        {
            var lemmikki = new Lemmikki();
            int result = 0;
            Assert.AreEqual(result, lemmikki.OverAllHealth);
        }

        [TestMethod]
        public void LemmikinMaksimiHealthPisteet100()
        {
            var lemmikki = new Lemmikki();
            lemmikki.OverAllHealth = 110;
            int expected = 100;
            Assert.AreEqual(expected, lemmikki.OverAllHealth);
        }

        [TestMethod]
        public void LemmikinMaksimiMieliAlaPisteet30()
        {
            var lemmikki = new Lemmikki();
            lemmikki.Mieliala = 78;
            int expected = 30;
            Assert.AreEqual(expected, lemmikki.Mieliala);
        }

        [TestMethod]
        public void LemmikinMaksimiHygieniaPisteet30()
        {
            var lemmikki = new Lemmikki();
            lemmikki.Hygiene = 127;
            int expected = 30;
            Assert.AreEqual(expected, lemmikki.Hygiene);
        }

        [TestMethod]
        public void LemmikinMaksimiRuokaPisteet40()
        {
            var lemmikki = new Lemmikki();
            lemmikki.Hunger = 247;
            int expected = 40;
            Assert.AreEqual(expected, lemmikki.Hunger);
        }

        [TestMethod]
        public void LemmikinMaksimiOverAllHealthPisteet()
        {
            var lemmikki = new Lemmikki();
            lemmikki.OverAllHealth = 346;
            int expected = 100;
            Assert.AreEqual(expected, lemmikki.OverAllHealth);
        }
    }
}