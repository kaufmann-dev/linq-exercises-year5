using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Entities;
using Domain.Enums;
using Domain.Factories;
using NUnit.Framework;

namespace LinqTest;

using System.Threading.Channels;

public class DishDomainTest {
    [SetUp]
    public void Setup() {
    }

    /*
        * 1.1) Beispiel: Ermitteln Sie alle Gerichte die weniger
        *      als 400 Kalorien haben. Geben Sie die Namen der
        *      entsprechenden Gerichte aus.
        * 
        */
    [Test]
    public void GetDishByCalories() {
        List<Dish> dishes = DishFactory.Instance.CreateDishes();

        // Query Syntax
        var queryQ =
            from dish in dishes
            where dish.Calories < 400
            select dish.Name;

        // Method Syntax
        var queryM = dishes
            .Where(d => d.Calories < 400)
            .Select(d => d.Name);
        
        // Console
        foreach (var dish in queryM){
            Console.WriteLine($"{dish}");
        }
        
        // Asserts
        Assert.AreEqual(queryQ.Count(), 2);
        
        Assert.That(queryM.Count(), Is.EqualTo(2));
    }

    /*
     * 1.2) Beispiel: Ermitteln Sie die Fisch- und Fleischgerichte, die nicht
     *      mit einem C beginnen.
     *
     *      Geben Sie die Namen der entsprechenden Gerichte aus. Sortieren Sie
     *      das Ergebnis nach den Namen der Gerichte.
     */
    [Test]
    public void GetDishByDishType() {
        List<Dish> dishes = DishFactory.Instance.CreateDishes();
        
        // Query Syntax
        var queryQ =
            from dish in dishes
            where (dish.Type == EDishType.FISH || dish.Type == EDishType.MEAT) && dish.Name.StartsWith("C")
            select dish.Name;

        // Method Syntax
        var queryM = dishes
            .Where(d => d.Name.StartsWith("C"))
            .Where(d => d.Type == EDishType.FISH || d.Type == EDishType.MEAT)
            .Select(d => d.Name);
        
        // Console
        var dishList = new List<string>();
        dishList.AddRange(queryM);
        dishList.Sort();
        foreach (var dish in dishList){
            Console.WriteLine($"{dish}");
        }

        // Asserts
        Assert.That(queryQ.Count(), Is.EqualTo(2));
        
        Assert.That(queryM.Count(), Is.EqualTo(2));
    }

    /*
     * 1.3) Beispiel: Ermitteln Sie alle Gerichte die mehr als
     *      7 Zutaten haben. Geben Sie die Namen der Gerichte aus.
     */
    [Test]
    public void CalculateMaxCalorieLevel2() {
        List<Dish> dishes = DishFactory.Instance.CreateDishes();

        // Query Syntax
        var queryQ =
            from dish in dishes
            where dish.Ingredients.Count > 7
            select dish.Name;

        // Method Syntax
        var queryM = dishes
            .Where(d => d.Ingredients.Count > 7)
            .Select(d => d.Name);
        
        // Console
        foreach (var dish in queryM){
            Console.WriteLine($"{dish}");
        }
        
        // Asserts
        Assert.That(queryQ.Count(), Is.EqualTo(5));
        
        Assert.That(queryM.Count(), Is.EqualTo(5));
    }

    /*
     * 1.4) Beispiel: Ermitteln Sie alle Gerichte die Pilze als Zutat beinhalten.
     */
    [Test]
    public void CalculateMaxCalorieLevel3() {
        List<Dish> dishes = DishFactory.Instance.CreateDishes();

        // Query Syntax
        var queryQ =
            from dish in dishes
            where dish.Ingredients.Contains(EIngredient.MUSHROOM)
            select dish;
        
        // Method Syntax
        var queryM = dishes
            .Where(d => d.Ingredients.Contains(EIngredient.MUSHROOM));

        // Asserts
        Assert.AreEqual(queryQ.Count(), 2);
        
        Assert.That(queryM.Count(), Is.EqualTo(2));
        
    }

    /*
     * 1.5) Beispiel: Berechnen Sie die Anzahl der Kalorien aller Fleischgerichte.
     */
    [Test]
    public void CalculateMaxCalorieLevel() {
        List<Dish> dishes = DishFactory.Instance.CreateDishes();

        // Query Syntax
        var queryQ =
            from dish in dishes
            where dish.Type == EDishType.MEAT
            group dish by dish.Type
            into dishGroup
            select new {
                Calories = dishGroup.Sum(d => d.Calories),
                DishType = dishGroup.Key
            };

        var listQ = queryQ.ToList();
        
        // Method Syntax
        var queryM = dishes
            .Where(d => d.Type == EDishType.MEAT)
            .GroupBy(d => d.Type, (key, dishData)
                => new {
                    Calories = dishData.Sum(d => d.Calories),
                    DishType = key
                });
        
        var listM = queryM.ToList();

        // Asserts
        Assert.That(listQ.Count, Is.EqualTo(1));
        Assert.That(listQ[0].Calories, Is.EqualTo(1600));
        Assert.That(listQ[0].DishType, Is.EqualTo(EDishType.MEAT));
        
        Assert.That(listM.Count, Is.EqualTo(1));
        Assert.That(listM[0].Calories, Is.EqualTo(1600));
        Assert.That(listM[0].DishType, Is.EqualTo(EDishType.MEAT));
    }

    /*
     * 1.6) Beispiel: Gruppieren Sie die Gerichte nach ihrem Typ. Geben
     *      Sie die Anzahl der gruppierten Gerichte zurück
     * 
     */
    [Test]
    public void GroupDishByType() {
        List<Dish> dishes = DishFactory.Instance.CreateDishes();
        
        // Query Syntax
        var queryQ =
            from dish in dishes
            group dish by dish.Type
            into dishGroup
            select new {
                Type = dishGroup.Key,
                Amount = dishGroup.Count()
            };
        
        var listQ = queryQ.ToList();
        
        // Method Syntax
        var queryM = dishes
            .GroupBy(d => d.Type, (key, data)
                => new {
                    DishType = key,
                    DishCount = data.Count()
                });

        // Console & Asserts
        foreach (var x in queryM){
            Console.WriteLine($"{x.DishType}: {x.DishCount}");
            
            Assert.True(Enum.GetValues<EDishType>().Contains(x.DishType));
        }
    }


    /*
     * 1.7) Beispiel: Berechnen Sie die Anzahl der Elemente für jede DishTyp Gruppe.
     *                
     */
    [Test]
    public void GroupDishByTypeCountingElements() {
        List<Dish> dishes = DishFactory.Instance.CreateDishes();
        
        // Query Syntax
        var queryQ =
            from dish in dishes
            group dish by dish.Type
            into dishGroup
            select new {
                type = dishGroup.Key,
                count = dishGroup.Count()
            };

        var listQ = queryQ.ToList();

        // Method Syntax
        var queryM = dishes
            .GroupBy(d => d.Type, (key, dishData)
                => new {
                    type = key,
                    dishCount = dishData.Count()
                });
        
        var listM = queryM.ToList();
        
        // Console
        foreach (var dish in queryQ)
        {
            Console.WriteLine($"{dish.type}: {dish.count}");
        }
        
        // Asserts
        Assert.That(listQ.Count, Is.EqualTo(2));
        Assert.That(listM.Count, Is.EqualTo(2));
    }

    /*
     * 1.8) Beispiel: Welche Zutaten befinden sich in jedem Gericht?
     *      Geben Sie die Zutaten geordnet nach ihrem Namen aus.
     *                
     */
    [Test]
    public void CommonIngredient() {
        List<Dish> dishes = DishFactory.Instance.CreateDishes();
        
        // Query Syntax
        var queryQ =
            from d in dishes
            select new
            {
                d.Name,
                Ingredients = (
                    from i in d.Ingredients
                    orderby i.ToString()
                    select i.ToString()
                ).ToList()
            };
        
        var listQ = queryQ.ToList();
        
        // Method Syntax 
        var queryM = dishes.Select(d => new
        {
            d.Name,
            Ingredients = d.Ingredients
                .Select(i => i.ToString())
                .OrderBy(i => i)
                .ToList()
        });

        var listM = queryM.ToList();
        
        // Console
        foreach (var x in listM){
            Console.WriteLine($"{x.Name}: ");
            for (int i = 0; i < x.Ingredients.Count(); i++){
                Console.Write(x.Ingredients[i]);
                if (i < x.Ingredients.Count() - 1){
                    Console.Write(", ");
                }
            }
            Console.WriteLine($"\n");
        }
    }
}