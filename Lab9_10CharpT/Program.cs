using System;

namespace CarEvents
{
    class Money
    {
        protected int nominal;
        protected int num;

        public Money(int nominal, int num)
        {
            this.nominal = nominal;
            this.num = num;
        }

        public void Print() => Console.WriteLine($"Номінал: {nominal}, Кількість: {num}");

        public bool CanBuy(int totalAmount) => totalAmount <= nominal * num;

        public int CalculateItems(int totalAmount) => nominal * num / totalAmount;

        public int Nominal
        {
            get { return nominal; }
            set { nominal = value; }
        }

        public int Num
        {
            get { return num; }
            set { num = value; }
        }

        public int TotalAmount
        {
            get { return nominal * num; }
        }
    }




    public delegate void CarEventHandler(object sender, CarEventArgs e);

    /// Модель автомобіля з подіями та реакцією сервісів на них.
    public class Car
    {
        string carModel; // модель автомобіля
        double mileage; // пробіг автомобіля
        int days; // кількість днів подій

        // сервіси автомобіля
        Maintenance maintenanceService;
        Repair repairService;
        Fuel fuelService;

        // події автомобіля
        public event CarEventHandler? CarEvent;

        // результати дій сервісів
        string[]? serviceResults;

        // випадкові події
        Random rnd = new Random();

        // ймовірність виникнення проблеми з автомобілем в поточний день
        double problemProbability;

        /// Конструктор автомобіля.
        /// Створює сервіси та розпочинає спостереження за подіями.

        public Car(string model, double initialMileage, int days)
        {
            carModel = model;
            mileage = initialMileage;
            this.days = days;
            problemProbability = 0.01; // встановлення початкової ймовірності проблеми

            // створення сервісів
            maintenanceService = new Maintenance(this);
            repairService = new Repair(this);
            fuelService = new Fuel(this);

            // підключення до спостереження за подіями
            maintenanceService.On();
            repairService.On();
            fuelService.On();
        }

        /// Запуск події автомобіля.
        /// Послідовно викликаються обробники події.

        protected virtual void OnCarEvent(CarEventArgs e)
        {
            const string MESSAGE_EVENT = "У автомобіля {0} виникла проблема! Пробіг: {1} км. День {2}-й";
            Console.WriteLine(string.Format(MESSAGE_EVENT, carModel, mileage, e.Day));

            if (CarEvent != null)
            {
                Delegate[] eventHandlers = CarEvent.GetInvocationList();
                serviceResults = new string[eventHandlers.Length];
                int k = 0;
                foreach (CarEventHandler eventHandler in eventHandlers)
                {
                    eventHandler(this, e);
                    serviceResults[k++] = e.Result;
                }
            }
        }


        /// Моделювання життя автомобіля.

        public void Drive()
        {
            const string ALL_CLEAR = "У автомобіля {0} все в порядку! Проблем не виявлено.";
            bool hadProblem = false;

            for (int day = 1; day <= days; day++)
            {
                if (rnd.NextDouble() < problemProbability)
                {
                    CarEventArgs e = new CarEventArgs(day);
                    OnCarEvent(e);
                    hadProblem = true;
                    for (int i = 0; i < serviceResults.Length; i++)
                    {
                        Console.WriteLine(serviceResults[i]);
                    }
                }
                // приріст пробігу
                mileage += rnd.Next(50, 200);
            }

            if (!hadProblem)
            {
                Console.WriteLine(string.Format(ALL_CLEAR, carModel));
            }
        }
    }


    public abstract class CarService
    {
        protected Car car;
        protected Random rnd = new Random();

        public CarService(Car car)
        {
            this.car = car;
        }

        public void On()
        {
            car.CarEvent += new CarEventHandler(HandleEvent);
        }

        public void Off()
        {
            car.CarEvent -= new CarEventHandler(HandleEvent);
        }

        public abstract void HandleEvent(object sender, CarEventArgs e);
    }

    public class Maintenance : CarService
    {
        public Maintenance(Car car) : base(car) { }

        public override void HandleEvent(object sender, CarEventArgs e)
        {
            const string OK = "Регулярне технічне обслуговування завершено!";
            const string NOK = "Необхідно провести технічне обслуговування!";
            if (rnd.Next(0, 10) > 7)
                e.Result = OK;
            else
                e.Result = NOK;
        }
    }

    public class Repair : CarService
    {
        public Repair(Car car) : base(car) { }

        public override void HandleEvent(object sender, CarEventArgs e)
        {
            const string OK = "Пошкодження виправлено!";
            const string NOK = "Потрібен ремонт!";
            if (rnd.Next(0, 10) > 5)
                e.Result = OK;
            else
                e.Result = NOK;
        }
    }

    public class Fuel : CarService
    {
        public Fuel(Car car) : base(car) { }

        public override void HandleEvent(object sender, CarEventArgs e)
        {
            const string OK = "Бак заправлений!";
            const string NOK = "Потрібно заправити бак!";
            if (rnd.Next(0, 10) > 3)
                e.Result = OK;
            else
                e.Result = NOK;
        }
    }


    /// Клас, що задає вихідні параметри події автомобіля.

    public class CarEventArgs : EventArgs
    {
        int day;
        string result;

        public int Day { get { return day; } }
        public string Result
        {
            get { return result; }
            set { result = value; }
        }

        public CarEventArgs(int day)
        {
            this.day = day;
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            try
            {
                Console.OutputEncoding = System.Text.Encoding.UTF8;
                Console.WriteLine("\t\t\tTask 1 ");
                Money wallet = new Money(100, 5);

                Console.WriteLine("Гаманець:");
                wallet.Print();
                Console.WriteLine($"Загальна сума грошей: {wallet.TotalAmount}");


                int itemPrice = 230;
                Console.WriteLine($"\nТовар на {itemPrice} гривень:");
                if (wallet.CanBuy(itemPrice))
                {
                    int numItems = wallet.CalculateItems(itemPrice);
                    Console.WriteLine($"Можна купити {numItems} штук товару.");
                }
                else
                {
                    Console.WriteLine("Недостатньо грошей для покупки товару.");
                }
            }
            catch (ArrayTypeMismatchException e)
            {
                Console.WriteLine($"Помилка типу масиву: {e.Message}");
            }
            catch (DivideByZeroException e)
            {
                Console.WriteLine($"Помилка ділення на нуль: {e.Message}");
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine($"Вихід за межі масиву: {e.Message}");
            }
            catch (InvalidCastException e)
            {
                Console.WriteLine($"Недійсне приведення типів: {e.Message}");
            }
            catch (OutOfMemoryException e)
            {
                Console.WriteLine($"Недостатньо пам'яті: {e.Message}");
            }
            catch (OverflowException e)
            {
                Console.WriteLine($"Переповнення: {e.Message}");
            }
            catch (StackOverflowException e)
            {
                Console.WriteLine($"Переповнення стеку: {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"Загальна помилка: {e.Message}");
            }

            Console.WriteLine("\n\t\t\t\tTask 2 ");
            Console.WriteLine("\t\t\tПроект 'Життя автомобіля'");

            Car myCar = new Car("Toyota Camry", 5000, 550);
            myCar.Drive();


        }
    }
}