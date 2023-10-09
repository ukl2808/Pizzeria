using Pizzeria;
using System;
using System.Linq;

class Program
{
    static void Main()
    {
        using (var context = new PizzeriaEntities())
        {
            while (true)
            {
                Console.WriteLine("Выберите действие:");
                Console.WriteLine("1. Добавить пиццу");
                Console.WriteLine("2. Добавить заказ");
                Console.WriteLine("3. Добавить клиента");
                Console.WriteLine("4. Показать все пиццы");
                Console.WriteLine("5. Показать все заказы");
                Console.WriteLine("6. Показать всех клиентов");
                Console.WriteLine("0. Выход");

                int choice = Convert.ToInt32(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        AddPizza(context);
                        break;
                    case 2:
                        AddOrder(context);
                        break;
                    case 3:
                        AddCustomer(context);
                        break;
                    case 4:
                        ShowAllPizzas(context);
                        break;
                    case 5:
                        ShowAllOrders(context);
                        break;
                    case 6:
                        ShowAllCustomers(context);
                        break;
                    case 0:
                        return;
                    default:
                        Console.WriteLine("Неверный выбор. Попробуйте еще раз.");
                        break;
                }
            }
        }
    }

    static void AddPizza(PizzeriaEntities context)
    {
        Pizzas newPizza = new Pizzas();

        Console.Write("Введите название пиццы: ");
        newPizza.name = Console.ReadLine();

        Console.Write("Введите цену пиццы: ");
        newPizza.price = Convert.ToDecimal(Console.ReadLine());

        Console.Write("Введите описание пиццы: ");
        newPizza.description = Console.ReadLine();

        context.Pizzas.Add(newPizza);
        context.SaveChanges();
    }

    static void AddOrder(PizzeriaEntities context)
    {
        Orders newOrder = new Orders();

        ShowAllCustomers(context);
        Console.Write("Введите ID клиента: ");
        int customerId = Convert.ToInt32(Console.ReadLine());

        newOrder.customer_id = customerId;

        decimal? totalAmount = 0; // Объявляем totalAmount как nullable decimal

        do
        {
            ShowAllPizzas(context);
            Console.Write("Введите ID пиццы (0 для завершения): ");
            int pizzaId = Convert.ToInt32(Console.ReadLine());

            if (pizzaId == 0)
            {
                break;
            }

            Pizzas pizza = context.Pizzas.Find(pizzaId);
            if (pizza != null)
            {
                Order_Items orderItem = new Order_Items
                {
                    pizza_id = pizzaId,
                    quantity = 1
                };

                newOrder.Order_Items.Add(orderItem);
                totalAmount += pizza.price; // Добавляем цену пиццы к общей сумме заказа
            }
            else
            {
                Console.WriteLine("Пицца с указанным ID не найдена.");
            }

        } while (true);

        newOrder.order_date = DateTime.Now;

        if (totalAmount != null)
        {
            newOrder.total_amount = totalAmount.Value; // Приводим к decimal
        }
        else
        {
            newOrder.total_amount = 0; // Если нет выбранных пицц, сумма равна 0.
        }

        context.Orders.Add(newOrder);
        context.SaveChanges();
    }



    static void AddCustomer(PizzeriaEntities context)
    {
        Customers newCustomer = new Customers();

        Console.Write("Введите имя: ");
        newCustomer.first_name = Console.ReadLine();

        Console.Write("Введите фамилию: ");
        newCustomer.last_name = Console.ReadLine();

        Console.Write("Введите номер телефона: ");
        newCustomer.phone_number = Console.ReadLine();

        Console.Write("Введите email: ");
        newCustomer.email = Console.ReadLine();

        context.Customers.Add(newCustomer);
        context.SaveChanges();
    }

    static void ShowAllPizzas(PizzeriaEntities context)
    {
        var pizzas = context.Pizzas.ToList();

        foreach (var pizza in pizzas)
        {
            Console.WriteLine($"ID: {pizza.pizza_id}, Name: {pizza.name}, Price: {pizza.price:C}, Description: {pizza.description}");
        }
    }

    static void ShowAllOrders(PizzeriaEntities context)
    {
        var orders = context.Orders.ToList();

        foreach (var order in orders)
        {
            var customer = context.Customers.Find(order.customer_id);
            Console.WriteLine($"Customer: {customer.first_name} {customer.last_name}, Order ID: {order.order_id}, Order Date: {order.order_date}, Total Amount: {order.total_amount:C}");
        }
    }

    static void ShowAllCustomers(PizzeriaEntities context)
    {
        var customers = context.Customers.ToList();

        foreach (var customer in customers)
        {
            Console.WriteLine($"ID: {customer.customer_id}, Name: {customer.first_name} {customer.last_name}, Phone: {customer.phone_number}, Email: {customer.email}");
        }
    }
}
