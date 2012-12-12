using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using PC.Framework.Utilities;

using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Security.Cryptography;

namespace PebbleCode.Tests.Unit.UtilityTests
{
    [TestClass]
    public class SaltedHashTest
    {
        [TestMethod]
        public void CheckSaltLength()
        {
            CheckSaltLength(1, 1);
            CheckSaltLength(1, 10);
            CheckSaltLength(1, 20);
            CheckSaltLength(5, 1);
            CheckSaltLength(5, 10);
            CheckSaltLength(5, 20);
            CheckSaltLength(10, 1);
            CheckSaltLength(10, 10);
            CheckSaltLength(10, 20);
            CheckSaltLength(30, 1);
            CheckSaltLength(30, 10);
            CheckSaltLength(30, 20);
        }

        private void CheckSaltLength(int saltLength, int dataLength)
        {
            byte[] binData = new byte[dataLength];
            byte[] binHash, binSalt;
            string strData = string.Empty;
            string strHash, strSalt;

            for (int index = 0; index < dataLength; index++)
            {
                binData[index] = (byte)index;
                strData += index.ToString().Last();
            }

            SaltedHash sh = new SaltedHash(new SHA256Managed(), saltLength);
            sh.GetHashAndSalt(binData, out binHash, out binSalt);
            Assert.AreEqual(saltLength, binSalt.Length, "Binary salt is wrong length");
            sh.GetHashAndSalt(strData, out strHash, out strSalt);
            byte[] strSaltAsBin = Convert.FromBase64String(strSalt);
            Assert.AreEqual(saltLength, strSaltAsBin.Length, "String salt is wrong length");
        }

        [TestMethod]
        public void HashAndVerifyData()
        {
            HashAndVerify(1, 0);
            HashAndVerify(1, 1);
            HashAndVerify(1, 10);
            HashAndVerify(1, 300);
            HashAndVerify(4, 0);
            HashAndVerify(4, 1);
            HashAndVerify(4, 10);
            HashAndVerify(4, 300);
            HashAndVerify(15, 0);
            HashAndVerify(15, 1);
            HashAndVerify(15, 10);
            HashAndVerify(15, 300);
        }

        private void HashAndVerify(int saltLength, int dataLength)
        {
            byte[] binData = new byte[dataLength];
            byte[] binHash, binSalt;
            string strData = string.Empty;
            string strHash, strSalt;

            for (int index = 0; index < dataLength; index++)
            {
                binData[index] = (byte)index;
                strData += index.ToString().Last();
            }

            SaltedHash sh = new SaltedHash(new SHA256Managed(), saltLength);
            sh.GetHashAndSalt(binData, out binHash, out binSalt);
            Assert.IsTrue(sh.VerifyHash(binData, binHash, binSalt), "Verfiy failed");
            sh.GetHashAndSalt(strData, out strHash, out strSalt);
            Assert.IsTrue(sh.VerifyHash(strData, strHash, strSalt), "Verfiy failed");
        }

        [TestMethod]
        public void MakeFail()
        {
            int[] saltLengths = new int[] { 1, 4, 10, 30 };
            foreach (int saltLength in saltLengths)
            {
                MakeFail("password", "Password", saltLength);
                MakeFail("password", "assword", saltLength);
                MakeFail("password", "passwor", saltLength);
                MakeFail("password", "password ", saltLength);
                MakeFail("password", "pass-word", saltLength);
                MakeFail("password", "abcdefgh", saltLength);
                MakeFail("password", "", saltLength);
                MakeFail("password", "PASSWORD", saltLength);
                MakeFail("Password", "password", saltLength);
                MakeFail("a", "A", saltLength);
                MakeFail("A", "a", saltLength);
                MakeFail("a", "b", saltLength);
            }
        }

        private void MakeFail(string strData, string testValue, int saltLength)
        {
            string strHash, strSalt;
            SaltedHash sh = new SaltedHash(new SHA256Managed(), saltLength);

            sh.GetHashAndSalt(strData, out strHash, out strSalt);
            Assert.IsFalse(sh.VerifyHash(testValue, strHash, strSalt), "Should not verify");
        }
    }
}
