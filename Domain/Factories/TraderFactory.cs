using Domain.Entities;
using Domain.Enums;

namespace Domain.Factories; 

public class TraderFactory {

    public static TraderFactory Instance { get; } = new();

    public List<Trader> CreateTraders() {
        List<Trader> traders = new List<Trader> {
            new("Peppi",ECity.VIENNA),
            new("Gustaf",ECity.VIENNA),
            new("Abdul",ECity.VIENNA),
            new("Luka",ECity.MILAN),
            new("Enzo", ECity.MILAN),
            new("Gianni", ECity.MILAN),
            new("Romeo",ECity.MILAN),
            new("Fabricio",ECity.MILAN),
            new("Alan", ECity.LONDON),
            new("Eric",ECity.LONDON)
        };

        return traders;
    }
}