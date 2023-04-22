using Domain.Entities;

namespace Domain.Factories;

public class TransactionFactory
{
    public static TransactionFactory Instance { get; } = new();
    private readonly List<Trader> _traders = TraderFactory.Instance.CreateTraders();

    public  List<Transaction> CreateTransactions() {
        List<Transaction> transactions = new List<Transaction>() {
            new(_traders[0], 2020, 50000),
            new(_traders[0], 2020,340000),
            new(_traders[0], 2020,210000),
            new(_traders[0], 2019,20000),
            new(_traders[0], 2019,10000),
            new(_traders[1], 2019,450000),
            new(_traders[2], 2019,100),
            new(_traders[3], 2018,320000),
            new(_traders[3], 2020,560000),
            new(_traders[3], 2020,230000),
            new(_traders[3], 2020,120000),
            new(_traders[3], 2019,560000),
            new(_traders[6], 2019,430000),
            new(_traders[6], 2020,110000),
            new(_traders[6], 2020,320000),
            new(_traders[6], 2019,350000),
            new(_traders[7], 2020,120000),
            new(_traders[7], 2020,560000),
            new(_traders[7], 2020,230000),
            new(_traders[7], 2018,120000)
        };

        return transactions;
    }
}