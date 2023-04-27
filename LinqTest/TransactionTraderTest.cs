using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Factories;
using NUnit.Framework;

namespace LinqTest
{
    public class TransactionDomainTest {

    [SetUp]
    public void Setup() {
        
    }
    
     /*
         * 2.1) Beispiel: Geben Sie fuer jeden Trader den Wert der Summe
         *      seiner Transaktionen an. Geben Sie den Namen des Tader
         *      und die Summe aus. Ordnen Sie das Ergebnis nach dem
         *      Namen des Traders.
         * 
         */
        [Test]
        public void GroupByTrader() {
            List<Transaction> transactions = TransactionFactory.Instance.CreateTransactions();
            List<Trader> traders = TraderFactory.Instance.CreateTraders();
            
            // Query
            var query = from t in traders
                join tr in transactions on t equals tr.Trader
                group tr by tr.Trader into transactionGroup
                orderby transactionGroup.Key.Name
                select new {
                    Name = transactionGroup.Key.Name,
                    TransactionAmount = transactionGroup.Sum(t=>t.Value)
                };
            
            // Console
            foreach (var trader in query){
                Console.WriteLine($"{trader.Name}: {trader.TransactionAmount}");
            }
            
            // Asserts
            Assert.AreEqual(6, query.ToList().Count);
        }

        /*
         * 2.2) Beispiel: Geben Sie den Namen des Traders mit der höchsten
         *                Summe seiner Transaktionen und die Summe seiner
         *                Transaktionen an.
         */
        [Test] public void MaxTransaction() {
            List<Transaction> transactions = TransactionFactory.Instance.CreateTransactions();

            // Query
            var maxRevenue = (from t in transactions
                group t by t.Trader.Name
                into traderGroup
                select traderGroup.Sum(t5 => t5.Value)).Max();
            
            var query =
                from t in transactions
                group t by t.Trader.Name into traderGroup 
                where traderGroup.Sum(t => t.Value).Equals(maxRevenue)
                orderby traderGroup.Key
                select new { Trader = traderGroup.Key, TransactionAmount = traderGroup.Sum(t => t.Value)};

            // Console
            Console.WriteLine($"{query.FirstOrDefault().Trader}: {query.FirstOrDefault().TransactionAmount}");
            
            // Asserts
            Assert.AreEqual(query.ToList().Count(), 1);
            Assert.AreEqual(query.FirstOrDefault().TransactionAmount, 1790000);
        }
        
        /*
         * 2.3) Beispiel: Geben Sie fuer jeden Trader den Wert der Summe
         *      seiner Transaktionen an. Geben Sie den Namen des Tader
         *      und die Summe aus. Berücksichtigen Sie nur Trader die in
         *      Summe mehr als 10000 veranlagt haben. Ordnen Sie das Ergebnis
         *      nach dem Namen des Traders.
         * 
         */
        [Test]
        public void GroupTransactionsHaving() {
            List<Transaction> transactions = TransactionFactory.Instance.CreateTransactions();
            List<Trader> traders = TraderFactory.Instance.CreateTraders();
            
            // Query
            var query =
                from t in traders
                join transaction in transactions on t equals transaction.Trader
                group transaction by t into transactionGroup 
                where transactionGroup.Sum(t => t.Value) > 10000
                orderby transactionGroup.Key.Name
                select new { Name = transactionGroup.Key.Name, Value = transactionGroup.Sum(t => t.Value)};
            
            // Console
            foreach (var trader in query){
                Console.WriteLine($"{trader.Name}: {trader.Value}");
            }
            
            // Asserts
            Assert.AreEqual(5, query.ToList().Count);
        }
        
        /*
         * 2.4) Beispiel: Wieviele Trader gibt es deren Name mit einem A bzw. P
         *      beginnnen. Geben Sie jeweils die Anzahl der Trader aus einen
         *      entsprechenden Namensanfangsbuchstaben haben.
         */
        [Test]
        public void GroupTraderByName() {
            List<Trader> traders = TraderFactory.Instance.CreateTraders();

            // Query
            var query =
                from t in traders 
                where t.Name.StartsWith("A") || t.Name.StartsWith("P")
                group t by t.Name[0] into traderGroup
                select new { NameCat = traderGroup.Key, TraderAmount = traderGroup.Count()};

            // Console
            foreach (var tradeGroup in query) {
                Console.WriteLine($"{tradeGroup.NameCat}: {tradeGroup.TraderAmount}");
            }
            
            // Asserts
            Assert.AreEqual(2, query.ToList().Count);
        }

    }
    
}