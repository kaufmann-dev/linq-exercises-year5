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

            
            var query =
                from t in traders
                join transaction in transactions on t equals transaction.Trader
                group transaction by t into transactionGroup 
                where transactionGroup.Sum(t => t.Value) > 10000
                orderby transactionGroup.Key.Name
                select new { Name = transactionGroup.Key.Name, Value = transactionGroup.Sum(t => t.Value)};

            var query2 = from t in traders
                join tr in transactions on t equals tr.Trader
                group tr by tr.Trader into transactionGroup
                select new { name = transactionGroup.Key.Name, };
            
            Assert.AreEqual(6, query.ToList().Count);
        }
        
        /*
         * 2.2) Beispiel: Geben Sie fuer jeden Trader den Wert der Summe
         *      seiner Transaktionen an. Geben Sie den Namen des Tader
         *      und die Summe aus. Ordnen Sie das Ergebnis
         *      nach dem Namen des Traders.
         * 
         */
        [Test]
        public void GroupTransaction() {
            List<Transaction> transactions = TransactionFactory.Instance.CreateTransactions();
            List<Trader> traders = TraderFactory.Instance.CreateTraders();

            var maxRevenue = (from t in transactions
                group t by t.Trader.Name
                into traderGroup
                select traderGroup.Sum(t5 => t5.Value)).Max();
            
            var query =
                from t in transactions
                group t by t.Trader.Name into traderGroup 
                where traderGroup.Sum(t => t.Value) > maxRevenue
                orderby traderGroup.Key
                select new { Trader = traderGroup.Key, TransactionAmount = traderGroup.Sum(t => t.Value)};

            // foreach (var trader in query) {
            //     Console.WriteLine($"{trader.Name} {trader.Value}");
            // }
            
            Assert.AreEqual(5, query.ToList().Count);
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

            var query =
                from t in traders
                join transaction in transactions on t equals transaction.Trader
                group transaction by t into transactionGroup 
                where transactionGroup.Sum(t => t.Value) > 10000
                orderby transactionGroup.Key.Name
                select new { Name = transactionGroup.Key.Name, Value = transactionGroup.Sum(t => t.Value)};

            foreach (var trader in query) {
                Console.WriteLine($"{trader.Name} {trader.Value}");
            }
            
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

            var query = from t in traders 
                where t.Name.StartsWith("A") || t.Name.StartsWith("P")
                group t by t.Name[0] into traderGroup
                select new { NameCat = traderGroup.Key, TraderAmount = traderGroup.Count()};

            foreach (var tradeGroup in query) {
                Console.WriteLine($"{tradeGroup.NameCat} {tradeGroup.TraderAmount}");
            }
            
            Assert.AreEqual(2, query.ToList().Count);
        }

    }
    
}