using ConsoleAppEFCore.Data;
using ConsoleAppEFCore.Models;

// PM >
// add-migration InitialCreate
// update-database
// add-migration UpdateSomething
// update-database

// Create DB + Models + classes
// https://www.youtube.com/watch?v=SryQxUeChMc&list=PLdo4fOcmZ0oX7uTkjYwvCJDG2qhcSzwZ6

// Reverse engineer to get your models + classes
// https://www.youtube.com/watch?v=DCYVfLT5_QI&list=PLdo4fOcmZ0oX7uTkjYwvCJDG2qhcSzwZ6&index=2
// https://www.youtube.com/watch?v=NX1w_2_BeOo

using ContosoPizzaContext context = new ContosoPizzaContext();

// PART 1
//Product veggieSpecial = new Product()
//{
//    Name = "Veggie Special Pizza",
//    Price = 9.99M
//};

//context.Products.Add(veggieSpecial);

//Product deluxeMeat = new Product()
//{
//    Name = "Deluxe Meat Pizza",
//    Price = 12.99M
//};

//context.Add(deluxeMeat);
//context.SaveChanges();

// PART 3 Update DB

var veggieSpecial = context.Products
    .Where(p => p.Name == "Veggie Special Pizza")
    .FirstOrDefault();

if (veggieSpecial is Product)
{
    veggieSpecial.Price = 10.99M;

    // PART 4 DELETE
    context.Remove(veggieSpecial);
}

context.SaveChanges();

// PART 2.1
//var products = context.Products
//    .Where(p => p.Price > 10.0M)
//    .OrderBy(p => p.Name);

// PART 2.2
var products = from product in context.Products
               where product.Price > 10.0M
               orderby product.Name
               select product;

foreach (var p in products)
{
    Console.WriteLine($"Id:     {p.Id}");
    Console.WriteLine($"Name:   {p.Name}");
    Console.WriteLine($"Price:  {p.Price}");
    Console.WriteLine(new string('-', 20));
}