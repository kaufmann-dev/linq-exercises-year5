using System;
using System.Collections.Generic;
using System.Linq;
using Domain.Dishes;
using NUnit.Framework;

namespace LinqTest;

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
        List<Dish> dishes = DishFactory.GetInstance().CreateDishes();

        var query = from dish in dishes where dish.Calories < 400 select dish.Name;
        Assert.That(query.Count(), Is.EqualTo(2));

        var query2 = dishes
            .Where(d => d.Calories < 400)
            .Select(d => d.Name);

        Assert.That(query2.Count(), Is.EqualTo(2));
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
        List<Dish> dishes = DishFactory.GetInstance().CreateDishes();

        var query = from dish in dishes
            where (dish.Type == EDishType.FISH || dish.Type == EDishType.MEAT)
                  && dish.Name.StartsWith("C")
            select dish.Name;
        Assert.That(query.Count(), Is.EqualTo(2));

        var query2 = dishes.Where(d => d.Name.StartsWith("C"))
            .Where(d => d.Type == EDishType.FISH || d.Type == EDishType.MEAT)
            .Select(d => d.Name);
        Assert.That(query2.Count(), Is.EqualTo(2));
    }

    /*
     * 1.3) Beispiel: Ermitteln Sie alle Gerichte die mehr als
     *      7 Zutaten haben. Geben Sie die Namen der Gerichte aus.
     */
    [Test]
    public void CalculateMaxCalorieLevel2() {
        List<Dish> dishes = DishFactory.GetInstance().CreateDishes();

        var query = from dish in dishes where dish.Ingredients.Count > 7 select dish.Name;
        Assert.That(query.Count(), Is.EqualTo(5));

        var query2 = dishes.Where(d => d.Ingredients.Count > 5)
            .Select(d => d.Name);
        Assert.That(query2.Count(), Is.EqualTo(5));
    }

    /*
     * 1.4) Beispiel: Ermitteln Sie alle Gerichte die Pilze als Zutat beinhalten.
     */
    [Test]
    public void CalculateMaxCalorieLevel3() {
        List<Dish> dishes = DishFactory.GetInstance().CreateDishes();

        var query = from dish in dishes
            where dish.Ingredients.Contains(EIngredient.MUSHROOM)
            select dish;

        Assert.That(query.Count(), Is.EqualTo(2));
    }

    /*
     * 1.5) Beispiel: Berechnen Sie die Anzahl der Kalorien aller Fleischgerichte.
     */
    [Test]
    public void CalculateMaxCalorieLevel() {
        List<Dish> dishes = DishFactory.GetInstance().CreateDishes();

        var query = from dish in dishes
            where dish.Type == EDishType.MEAT
            group dish by dish.Type
            into dishGroup
            select new { Calories = dishGroup.Sum(d => d.Calories), DishType = dishGroup.Key };

        var data = query.ToList();

        Assert.That(data.Count, Is.EqualTo(1));
        Assert.That(data[0].Calories, Is.EqualTo(1600));
        Assert.That(data[0].DishType, Is.EqualTo(EDishType.MEAT));

        var query2 = dishes.Where(d => d.Type == EDishType.MEAT)
            .GroupBy(d => d.Type, (key, dishData) => new { Calories = dishData.Sum(d => d.Calories), DishType = key });
        var data2 = query2.ToList();

        Assert.That(data2.Count, Is.EqualTo(1));
        Assert.That(data2[0].Calories, Is.EqualTo(1600));
        Assert.That(data2[0].DishType, Is.EqualTo(EDishType.MEAT));

        // Assert.AreEqual(1600, calorieCount);
    }

    /*
     * 1.6) Beispiel: Gruppieren Sie die Gerichte nach ihrem Typ. Geben
     *      Sie die gruppierten Gerichte zurück
     * 
     */
    [Test]
    public void GroupDishByType() {
        List<Dish> dishes = DishFactory.GetInstance().CreateDishes();
        var query = from dish in dishes
            group dish by dish.Type
            into dishGroup
            select dishGroup;

        var data = query.ToList();


        foreach (var item in data) {
            var type = item.Key;
            var dishesList = item.ToList();

            Assert.True(Enum.GetValues<EDishType>().Contains(type));
        }
    }


    /*
     * 1.7) Beispiel: Berechnen Sie die Anzahl der Elemente für jede DishTyp Gruppe.
     *                
     */
    [Test]
    public void GroupDishByTypeCountingElements() {
        List<Dish> dishes = DishFactory.GetInstance().CreateDishes();
        // var query = 
        var query = from dish in dishes
            group dish by dish.Type
            into dishGroup
            select new {
                type = dishGroup.Key, count = dishGroup.Count()
            };

        var data = query.ToList();
        Assert.That(data.Count, Is.EqualTo(2));

        var query2 = dishes.GroupBy(d => d.Type, (key, dishData) => new { type = key, dishCount = dishData.Count() });
        var data2 = query2.ToList();
        
        Assert.That(data2.Count, Is.EqualTo(2));
    }

    /*
     * 1.8) Beispiel: Welche Zutaten befinden sich in jedem Gericht?
     *      Geben Sie die Zutaten geordnet nach ihrem Namen aus.
     *                
     */
    [Test]
    public void CommonIngredient() {
        List<Dish> dishes = DishFactory.GetInstance().CreateDishes();
        // var query = 

        var query = dishes.Select(d => d.Ingredients)
            .Aggregate((ingredients, next) => ingredients.Intersect(next).ToList())
            .OrderBy( i => i);
        
        
        // foreach (var ingredient in query) {
        //    Console.WriteLine(ingredient);
        // }

        // Assert.AreEqual(1, query.Count());
    }
}