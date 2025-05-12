namespace HelloWorld.Classes
{
    // Abstraction: Interface for vehicle behavior
    public interface IVehicle
    {
        string Model { get; }
        void StartEngine();
        void Drive();
    }

    // Base abstract class for shared logic
    public abstract class VehicleBase : IVehicle
    {
        private const string DefaultEngineStartMessage = "Engine started.";

        // Encapsulation: Private field with public property
        private readonly string _model;
        public string Model => _model;

        protected VehicleBase(string model)
        {
            _model = string.IsNullOrEmpty(model) ? "Unknown" : model;
        }

        // Single responsibility: Start engine logic
        public virtual void StartEngine()
        {
            LogEngineStart();
        }

        // Single responsibility: Drive logic (to be implemented by subclasses)
        public abstract void Drive();

        // Helper method to avoid code duplication
        protected void LogEngineStart(string message = DefaultEngineStartMessage)
        {
            Console.WriteLine($"{message} (Model: {Model})");
        }
    }

    // Concrete class: Car
    public class Car : VehicleBase
    {
        private const string CarDriveMessage = "driving smoothly";
        private const string CarEngineStartMessage = "roars to life";

        public Car(string model) : base(model) { }

        // Single responsibility: Override drive behavior
        public override void Drive()
        {
            LogVehicleAction(CarDriveMessage);
        }

        // Single responsibility: Override engine start
        public override void StartEngine()
        {
            LogEngineStart($"{Model}'s engine {CarEngineStartMessage}");
        }

        // Helper method for reusable logging
        private void LogVehicleAction(string action)
        {
            Console.WriteLine($"{Model} is {action}.");
        }
    }

    // Concrete class: Motorcycle
    public class Motorcycle : VehicleBase
    {
        private const string MotorcycleDriveMessage = "zooming fast";

        public Motorcycle(string model) : base(model) { }

        // Single responsibility: Override drive behavior
        public override void Drive()
        {
            LogVehicleAction(MotorcycleDriveMessage);
        }

        // Helper method for reusable logging
        private void LogVehicleAction(string action)
        {
            Console.WriteLine($"{Model} is {action}.");
        }
    }

    public class Program
    {
        public static void Main()
        {
            // Polymorphism: Using interface for loose coupling
            IVehicle[] vehicles =
                [
                    new Car("Toyota"),
                    new Motorcycle("Harley")
                ];

            foreach (var vehicle in vehicles)
            {
                ProcessVehicle(vehicle);
            }
        }

        // Single responsibility: Process a vehicle
        private static void ProcessVehicle(IVehicle vehicle)
        {
            Console.WriteLine($"Processing vehicle: {vehicle.Model}");
            vehicle.StartEngine();
            vehicle.Drive();
            Console.WriteLine();
        }
    }
}