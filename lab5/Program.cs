using System;
using System.Collections.Generic;
using System.Linq;

// Клас "Товар"
public class Product
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string Description { get; set; }
    public string Category { get; set; }
    public int Rating { get; set; }

    public Product(string name, decimal price, string description, string category, int rating)
    {
        Name = name;
        Price = price;
        Description = description;
        Category = category;
        Rating = rating;
    }
}

// Клас "Користувач"
public class User
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Order> PurchaseHistory { get; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
        PurchaseHistory = new List<Order>();
    }
}

// Клас "Замовлення"
public class Order
{
    public List<Product> Products { get; }
    public int Quantity { get; set; }
    public decimal TotalPrice { get; set; }
    public string Status { get; set; }

    public Order()
    {
        Products = new List<Product>();
        Status = "В обробці"; // Початковий статус замовлення
    }
}

// Інтерфейс для пошуку товарів
public interface ISearchable
{
    List<Product> SearchByPrice(decimal maxPrice);
    List<Product> SearchByCategory(string category);
    List<Product> SearchByRating(int minRating);
}

// Клас "Магазин"
public class Store : ISearchable
{
    public List<Product> Products { get; }
    public List<User> Users { get; }
    public List<Order> Orders { get; }

    public Store()
    {
        Products = new List<Product>();
        Users = new List<User>();
        Orders = new List<Order>();
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }

    public void AddUser(User user)
    {
        Users.Add(user);
    }

    public User Login(string username, string password)
    {
        return Users.FirstOrDefault(user => user.Username == username && user.Password == password);
    }

    public void PlaceOrder(User user, Order order)
    {
        if (Users.Contains(user) && user.PurchaseHistory != null)
        {
            user.PurchaseHistory.Add(order);
            Orders.Add(order);
            // Логіка обробки замовлення, обчислення ціни тощо
        }
    }

    public List<Product> SearchByPrice(decimal maxPrice)
    {
        return Products.Where(product => product.Price <= maxPrice).ToList();
    }

    public List<Product> SearchByCategory(string category)
    {
        return Products.Where(product => product.Category == category).ToList();
    }

    public List<Product> SearchByRating(int minRating)
    {
        return Products.Where(product => product.Rating >= minRating).ToList();
    }
}

public class Program
{
    public static void Main()
    {
        // Створення магазину і додавання деяких товарів та користувачів
        Store store = new Store();
        store.AddProduct(new Product("Лаптоп", 800, "Потужний лаптоп", "Електроніка", 4));
        store.AddProduct(new Product("Смартфон", 400, "Сучасний смартфон", "Електроніка", 5));
        store.AddProduct(new Product("Книга", 20, "Цікава книга", "Книги", 4));
        store.AddUser(new User("user1", "password1"));
        store.AddUser(new User("user2", "password2"));

        // Спроба авторизуватися користувачам та розмістити замовлення
        User user1 = store.Login("user1", "password1");
        if (user1 != null)
        {
            Order order1 = new Order();
            order1.Products.Add(store.Products[0]);
            order1.Quantity = 1;
            order1.TotalPrice = store.Products[0].Price * order1.Quantity;
            store.PlaceOrder(user1, order1);
        }

        User user2 = store.Login("user2", "password2");
        if (user2 != null)
        {
            Order order2 = new Order();
            order2.Products.Add(store.Products[1]);
            order2.Quantity = 2;
            order2.TotalPrice = store.Products[1].Price * order2.Quantity;
            store.PlaceOrder(user2, order2);
        }

        // Пошук товарів за різними критеріями
        List<Product> affordableProducts = store.SearchByPrice(500);
        List<Product> electronics = store.SearchByCategory("Електроніка");
        List<Product> highlyRatedProducts = store.SearchByRating(4);

        // Вивід результатів пошуку
        Console.WriteLine("Доступні товари за ціною до $500:");
        foreach (var product in affordableProducts)
        {
            Console.WriteLine($"{product.Name} - ${product.Price}");
        }

        Console.WriteLine("\nЕлектроніка в асортименті:");
        foreach (var product in electronics)
        {
            Console.WriteLine($"{product.Name} - ${product.Price}");
        }

        Console.WriteLine("\nВисокорейтингові товари (рейтинг 4 і більше):");
        foreach (var product in highlyRatedProducts)
        {
            Console.WriteLine($"{product.Name} - Рейтинг {product.Rating}");
        }
    }
}

