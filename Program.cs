
using System.Runtime.InteropServices;
using HelloWorld.Classes;


var number1 = new IntNumber
{
    IntValue = 5,
};

var number2 = number1;
number1.IntValue = number1.IntValue + 1;
var temp = number1.IntValue;
Console.WriteLine(number1.IntValue); // Output: 6, no ref = 5
Console.WriteLine(number2.IntValue); // Output: 6, no ref = 6
TestClassWithSnippet class1 = new DerivedTestClassWithSnippet();
class1.PrintSomething2();


ClassA classA = new ClassA();
ISubInterface inter = classA;
inter.MyMethod("");

Animal a = new Mammal();

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

static void ProcessVehicle(IVehicle vehicle)
{
    Console.WriteLine($"Processing vehicle: {vehicle.Model}");
    vehicle.StartEngine();
    vehicle.Drive();
    Console.WriteLine();
}
[StructLayout(LayoutKind.Sequential)]
public struct MyStruct
{
    public int A; // 4 bytes
    public double B; // 8 bytes
    public long C; // 8 bytes

    int CalculateSum(int a, int b)
    {
        return a + b;
    }

    double CalculateSum(int a, int b, double c)
    {

        return 0;
    }

}

class IntNumber
{
    public int IntValue { get; set; }
}

public readonly record struct Coordinates(double Latitude, double Longitude);

public class Animal
{

}

public class Mammal : Animal
{

}

public class Reptile : Animal
{

}

namespace YourNamespace
{
    class YourClass
    {
    }

    struct YourStruct
    {
    }

    interface IYourInterface
    {
        int UserId { get; set; }
        void GetUsers();
    }

    delegate int YourDelegate();

    enum YourEnum
    {
    }

    namespace YourNestedNamespace
    {
        struct YourStruct
        {
        }
    }
}

public interface ITestInterface
{
    public int Age { get; set; }
    public int MyProperty { get; set; }
}

public interface ISubInterface : ITestInterface
{
    public void MyMethod(string parameter)
    {
        System.Console.WriteLine("Hello");
    }
}

class ClassA : ISubInterface
{
    public int Age { get; set; }
    public int MyProperty { get; set; }

    // public void MyMethod(string temp)
    // {

    // }
}

public abstract class TestClassWithSnippet
{
    public virtual void PrintSomething()
    {
    }

    public abstract void PrintSomething2();
}

public class DerivedTestClassWithSnippet : TestClassWithSnippet
{
    public override void PrintSomething2()
    {
        throw new NotImplementedException();
    }
}