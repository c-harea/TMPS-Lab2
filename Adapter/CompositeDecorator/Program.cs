using System;
using System.Collections.Generic;
using System.IO;

namespace RecipeManagementSystem
{
    // Component interface
    public interface IIngredient
    {
        string Name { get; }
        string Description { get; }
        double Price { get; }
        void Print();
    }

    // Leaf class
    public class Ingredient : IIngredient
    {
        public string Name { get; }
        public string Description { get; }
        public double Price { get; }

        public Ingredient(string name, string description, double price)
        {
            Name = name;
            Description = description;
            Price = price;
        }

        public void Print()
        {
            Console.WriteLine($"{Name} - {Description} (${Price:F2})");
        }
    }

    // Decorator abstract class
    public abstract class IngredientDecorator : IIngredient
    {
        protected IIngredient _ingredient;

        public IngredientDecorator(IIngredient ingredient)
        {
            _ingredient = ingredient;
        }

        public string Name => _ingredient.Name;
        public string Description => _ingredient.Description;
        public double Price => _ingredient.Price;
        public abstract void Print();
    }

    // Concrete decorator class
    public class OrganicDecorator : IngredientDecorator
    {
        public OrganicDecorator(IIngredient ingredient) : base(ingredient)
        {
        }

        public override void Print()
        {
            _ingredient.Print();
            Console.WriteLine("- organic");
        }
    }

    // Concrete decorator class
    public class GlutenFreeDecorator : IngredientDecorator
    {
        public GlutenFreeDecorator(IIngredient ingredient) : base(ingredient)
        {
        }

        public override void Print()
        {
            _ingredient.Print();
            Console.WriteLine("- gluten-free");
        }
    }

    // Composite class
    public class Recipe : IIngredient
    {
        protected readonly List<IIngredient> _components = new List<IIngredient>();

        public string Name { get; }
        public string Description { get; }
        public double Price { get; private set; }

        public Recipe(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public void AddComponent(IIngredient component)
        {
            _components.Add(component);
            Price += component.Price;
        }

        public void RemoveComponent(IIngredient component)
        {
            _components.Remove(component);
            Price -= component.Price;
        }

        public void Print()
        {
            Console.WriteLine($"{Name} - {Description} (${Price:F2})");
            Console.WriteLine("Contents:\n");
            foreach (var component in _components)
            {
                component.Print();
            }
            Console.WriteLine();
        }
    }

    public class Menu : Recipe
    {
        public Menu(string name, string description) : base(name, description)
        {
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Create some ingredients
            var flour = new Ingredient("Flour", "Organic flour", 2.0);
            var sugar = new Ingredient("Sugar", "White sugar", 1.5);
            var eggs = new Ingredient("Eggs", "Free-range eggs", 3.0);

            // Decorate ingredients with additional functionality
            var organicFlour = new OrganicDecorator(flour);
            var glutenFreeFlour = new GlutenFreeDecorator(flour);
            var organicSugar = new OrganicDecorator(sugar);

            // Create a recipe
            var cake = new Recipe("Cake", "A delicious cake");
            cake.AddComponent(organicFlour);
            cake.AddComponent(organicSugar);
            cake.AddComponent(eggs);

            // Create another recipe
            var cookies = new Recipe("Cookies", "Some tasty cookies");
            cookies.AddComponent(glutenFreeFlour);
            cookies.AddComponent(sugar);
            cookies.AddComponent(eggs);

            // Add the recipes to a menu
            var menu = new Menu("Menu", "Our menu");
            menu.AddComponent(cake);
            menu.AddComponent(cookies);

            // Print the menu
            menu.Print();

        }
    }
}