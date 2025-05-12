namespace HelloWorld.Classes
{
    public interface ITransport
    {
        string Name { get; }
        decimal Capacity { get; }
        void Deliver(decimal cargo);
        decimal CalculateCost(int distance, decimal cargo);
    }

    public class Truck : ITransport
    {
        public string Name => "Truck";

        public decimal Capacity => 100;

        public decimal CalculateCost(int distance, decimal cargo)
        {
            return distance * 1.5m * cargo / 100;
        }

        public void Deliver(decimal cargo)
        {
            Console.WriteLine($"Truck delivering {cargo:F2} units by road.");
        }
    }

    public class Ship : ITransport
    {
        public string Name => "Ship";

        public decimal Capacity => 1000;

        public decimal CalculateCost(int distance, decimal cargo)
        {
            return distance * 0.8m * cargo / 1000;
        }

        public void Deliver(decimal cargo)
        {
            Console.WriteLine($"Ship delivering {cargo:F2} units by sea.");
        }
    }

    public abstract class TransportFactory
    {
        public abstract ITransport CreateTransport();
    }

    public class TruckFactory : TransportFactory
    {
        public override ITransport CreateTransport()
        {
            return new Truck();
        }
    }

    public class ShipFactory : TransportFactory
    {
        public override ITransport CreateTransport()
        {
            return new Ship();
        }
    }

    public class LogisticsManager
    {
        private readonly TransportFactory _truckFactory;
        private readonly TransportFactory _shipFactory;

        public LogisticsManager(TransportFactory truckFactory, TransportFactory shipFactory)
        {
            _truckFactory = truckFactory ?? throw new ArgumentNullException(nameof(truckFactory));
            _shipFactory = shipFactory ?? throw new ArgumentNullException(nameof(shipFactory));
        }

        public void DeliverCargo(decimal cargo, int distance)
        {
            ITransport truck = _truckFactory.CreateTransport();
            ITransport ship = _shipFactory.CreateTransport();

            // Algorithm: Split cargo based on truck capacity
            decimal truckCargo = Math.Min(cargo, truck.Capacity);
            decimal shipCargo = cargo - truckCargo;

            Console.WriteLine($"Processing {cargo:F2} units of cargo over {distance} units:");

            // Deliver with truck
            if (truckCargo > 0)
            {
                truck.Deliver(truckCargo);
                decimal truckCost = truck.CalculateCost(distance, truckCargo);
                Console.WriteLine($"Truck cost: ${truckCost:F2}");
            }
            else
            {
                Console.WriteLine("No cargo for truck.");
            }

            // Deliver excess with ship
            if (shipCargo > 0)
            {
                ship.Deliver(shipCargo);
                decimal shipCost = ship.CalculateCost(distance, shipCargo);
                Console.WriteLine($"Ship cost: ${shipCost:F2}");
            }
            else
            {
                Console.WriteLine("No cargo for ship.");
            }
        }
    }

    // public class Program
    // {
    //     public static void Main()
    //     {
    //         // Create factories
    //         TransportFactory truckFactory = new TruckFactory();
    //         TransportFactory shipFactory = new ShipFactory();

    //         // Initialize logistics manager
    //         LogisticsManager manager = new LogisticsManager(truckFactory, shipFactory);

    //         // Test case 1: Cargo within truck capacity
    //         Console.WriteLine("Test Case 1: 80 units (within truck capacity)");
    //         manager.DeliverCargo(80, 100);
    //         Console.WriteLine("---");

    //         // Test case 2: Cargo exceeds truck capacity
    //         Console.WriteLine("Test Case 2: 150 units (truck + ship)");
    //         manager.DeliverCargo(150, 100);
    //     }
    // }
}