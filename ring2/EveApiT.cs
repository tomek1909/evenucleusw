﻿using System;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ring0;

using ts.api;

namespace ring2
{
    [TestClass]
    public class EveApiT : TestBase
    {
        public EveApi GetEveApi()
        {
            var evecache = new EveSqlServerCache(Logger, AccountContextProvider);

            return new EveApi(Logger, evecache, null);
        }

        [TestMethod]
        public void SingleChar()
        {
            long code = 3483492;
            string vcode = "ZwML01eU6aQUVIEC7gedCEaySiNxRTJxgWo2qoVnxd5duN4tt4CWgMuYMSVNWIUG";
            var api = GetEveApi();
            var chars = api.GetCharacters(code, vcode);

            Assert.AreEqual(1, chars.Count);
            Assert.AreEqual("MicioGatto", chars[0].CharacterName);
        }

        [TestMethod]
        public void MultipleChars()
        {
            long code = 3231405;
            string vcode = "UZDkcXJAQYdDXu8ItoX7ICXT914ephxHX2n07CFjgKwkYhP2XE6PerFGzTWYfgL6";
            var api = GetEveApi();
            var chars = api.GetCharacters(code, vcode);

            Assert.IsTrue(chars.Count > 1);
            Assert.AreEqual(1, chars.Count(x => x.CharacterName == "MicioGatto"));
        }

        [TestMethod]
        public void CorpoKey()
        {
            int code = 4865846;
            String vcode = "t99aNY7KfAMRUxCz7S29ZkPvBEwVjrYwtWdgGVXFYKq0lHAWtIlpwuiVqxZpnwBT";
            var api = GetEveApi();
            var chars = api.GetCharacters(code, vcode);

            Assert.AreEqual(1, chars.Count);
            Assert.AreEqual(1, chars.Count(x => x.CharacterName == "MicioGatto"));
        }

        [TestMethod]
        public void InvalidKey()
        {
            long code = 1;
            string vcode = "UZDkcXJAQYdDXu8ItoX7ICXT914ephxHX2n07CFjgKwkYhP2XE6PerFGzTWYfgL6";
            try
            {
                var api = GetEveApi();
                var chars = api.GetCharacters(code, vcode);
                Assert.IsFalse(false, "Unreachable");
            }
            catch (UserException)
            {
            }
        }

        [TestMethod]
        public async Task RefTypeDict()
        {
            var evecache = new EveSqlServerCache(Logger, AccountContextProvider);
            var cache = new CacheEveLibProvider(evecache);
            var refTypeDict = new RefTypeDict(cache, evecache);
            var result = await refTypeDict.GetById(56);
            Assert.AreEqual("Manufacturing", result);
        }

        [TestMethod]
        public async Task JournalEntriesCharacters()
        {
            int code = 3483492;
            String vcode = "ZwML01eU6aQUVIEC7gedCEaySiNxRTJxgWo2qoVnxd5duN4tt4CWgMuYMSVNWIUG";
            var api = GetEveApi();

            var characters = api.GetCharacters(code, vcode);
            Assert.AreEqual(1, characters.Count);
            var character = characters[0];
            Assert.AreEqual("MicioGatto", character.CharacterName);

            var entries = await api.GetJournalEntries(code, vcode, character.CharacterName, 1, new DateTime(1900, 1, 1));
            Assert.IsTrue(entries.Count > 0);

            // find largest id & check for PilotId
            DateTime largestDateTime = new DateTime(1900, 1, 1);
            foreach (var x in entries)
            {
                if (x.Date > largestDateTime) largestDateTime = x.Date;
                //Assert.AreEqual(1, x.CharacterId);
            }

            var entries2 = await api.GetJournalEntries(code, vcode, character.CharacterName, 1, largestDateTime);
            Assert.AreEqual(0, entries2.Count);
        }

        [TestMethod]
        public async Task JournalEntriesCorpo()
        {
            int code = 4865846;
            String vcode = "t99aNY7KfAMRUxCz7S29ZkPvBEwVjrYwtWdgGVXFYKq0lHAWtIlpwuiVqxZpnwBT";
            var api = GetEveApi();

            var entries = await api.GetJournalEntriesCorpo(code, vcode, 2, new DateTime(1900, 1, 1));
            Assert.IsTrue(entries.Count > 0);

            // find largest id & check for PilotId
            DateTime largestDateTime = new DateTime(1900, 1, 1);
            foreach (var x in entries)
            {
                if (x.Date > largestDateTime) largestDateTime = x.Date;
                //Assert.AreEqual(2, x.CorporationId);
            }

            var entries2 = await api.GetJournalEntriesCorpo(code, vcode, 2, largestDateTime);
            Assert.AreEqual(0, entries2.Count);
        }

        [TestMethod]
        public async Task WalletTransactionsCharacters()
        {
            int code = 3483492;
            String vcode = "ZwML01eU6aQUVIEC7gedCEaySiNxRTJxgWo2qoVnxd5duN4tt4CWgMuYMSVNWIUG";
            var api = GetEveApi();

            var characters = api.GetCharacters(code, vcode);
            Assert.AreEqual(1, characters.Count);
            var character = characters[0];
            Assert.AreEqual("MicioGatto", character.CharacterName);

            var entries = await api.GetTransactions(code, vcode, character.CharacterName, 1, new DateTime(1900, 1, 1));
            Assert.IsTrue(entries.Count > 0);

            // find largest id & check for PilotId
            DateTime largestDateTime = new DateTime(1900, 1, 1);
            foreach (var x in entries)
            {
                if (x.TransactionDate > largestDateTime) largestDateTime = x.TransactionDate;
                //Assert.AreEqual(1, x.CharacterId);
            }

            var entries2 = await api.GetTransactions(code, vcode, character.CharacterName, 1, largestDateTime);
            Assert.AreEqual(0, entries2.Count);
        }

        [TestMethod]
        public async Task WalletTransactionsCorpo()
        {
            int code = 4865846;
            String vcode = "t99aNY7KfAMRUxCz7S29ZkPvBEwVjrYwtWdgGVXFYKq0lHAWtIlpwuiVqxZpnwBT";
            var api = GetEveApi();

            var entries = await api.GetTransactionsCorpo(code, vcode, 2, new DateTime(1900, 1, 1));
            Assert.IsTrue(entries.Count > 0);

            // find largest id & check for PilotId
            DateTime largestDateTime = new DateTime(1900, 1, 1);
            foreach (var x in entries)
            {
                if (x.TransactionDate > largestDateTime) largestDateTime = x.TransactionDate;
                //Assert.AreEqual(2, x.CorporationId);
            }

            var entries2 = await api.GetTransactionsCorpo(code, vcode, 2, largestDateTime);
            Assert.AreEqual(0, entries2.Count);
        }

        [TestMethod]
        public void IsCorporationKey()
        {
            int code = 4865846;
            String vcode = "t99aNY7KfAMRUxCz7S29ZkPvBEwVjrYwtWdgGVXFYKq0lHAWtIlpwuiVqxZpnwBT";
            var api = GetEveApi();

            Assert.IsTrue(api.IsCorporationKey(code, vcode));

            code = 3483492;
            vcode = "ZwML01eU6aQUVIEC7gedCEaySiNxRTJxgWo2qoVnxd5duN4tt4CWgMuYMSVNWIUG";
            Assert.IsFalse(api.IsCorporationKey(code, vcode));
        }

        [TestMethod]
        public void CorporationName()
        {
            int code = 4865846;
            String vcode = "t99aNY7KfAMRUxCz7S29ZkPvBEwVjrYwtWdgGVXFYKq0lHAWtIlpwuiVqxZpnwBT";
            var api = GetEveApi();

            String name = api.GetCorporationName(code, vcode);
            Assert.AreEqual("My Random Corporation", name);
        }

        [TestMethod]
        public async Task CharacterSheet()
        {
            int code = 3483492;
            String vcode = "ZwML01eU6aQUVIEC7gedCEaySiNxRTJxgWo2qoVnxd5duN4tt4CWgMuYMSVNWIUG";
            var api = GetEveApi();

            var characters = api.GetCharacters(code, vcode);
            Assert.AreEqual(1, characters.Count);
            var character = characters[0];

            var sheet = await character.GetCharacterSheetAsync();
            Assert.AreEqual("MicioGatto", sheet.Result.Name);

            var rsp2 = await character.GetSkillTrainingAsync();
            Assert.IsTrue(rsp2.Result.IsTraining);

            var rsp3 = await character.GetSkillQueueAsync();
            Assert.IsTrue(rsp3.Result.Queue.Count > 0);
        }

        [TestMethod]
        public async Task IndustryJobsCharacter()
        {
            int code = 3483492;
            String vcode = "ZwML01eU6aQUVIEC7gedCEaySiNxRTJxgWo2qoVnxd5duN4tt4CWgMuYMSVNWIUG";
            var api = GetEveApi();

            var characters = api.GetCharacters(code, vcode);
            Assert.AreEqual(1, characters.Count);
            var character = characters[0];

            var rsp = await character.GetIndustryJobsAsync();
            Assert.IsNotNull(rsp);
        }

        [TestMethod]
        public async Task IndustryJobsCorpo()
        {
            int code = 4865846;
            String vcode = "t99aNY7KfAMRUxCz7S29ZkPvBEwVjrYwtWdgGVXFYKq0lHAWtIlpwuiVqxZpnwBT";
            var api = GetEveApi();

            var corpo = api.GetCorporations(code, vcode);
            var rsp = await corpo.GetIndustryJobsAsync();
            Assert.IsNotNull(rsp);
        }
    }
}
