
using System.Runtime.InteropServices;
using HelloWorld;

var number1 = new IntNumber
{
    IntValue = 5,
};

var number2 = number1;
number1.IntValue = number1.IntValue + 1;
var temp = number1.IntValue;
Console.WriteLine(number1.IntValue); // Output: 6, no ref = 5
Console.WriteLine(number2.IntValue); // Output: 6, no ref = 6

DoubleValue(ref temp);
Test.TestMethodParameterHint(1);
Console.WriteLine($"{number1.IntValue}"); // output: 12, noref = 10
Console.WriteLine($"{number2.IntValue}"); // output: 12, noref = 13
CalculateSum(1, 2);

Type type = typeof(MyStruct);
Console.WriteLine($"Base type: {type.BaseType}");
Console.WriteLine($"Is ValueType: {type.IsValueType}");
Console.WriteLine($"Is Class: {type.IsClass}");
Console.WriteLine($"Is Class: {type.IsSealed}");

string str = String.Empty;
// var sizeOfString = sizeof(aInt);
var sizeOf = Marshal.SizeOf<MyStruct>();

Console.WriteLine(sizeOf);

void DoubleValue(ref int x)
{
    x = x * 2; // 10
}


Animal a = new Mammal();

if (a is Reptile)
{
    Reptile r = (Reptile)a; // InvalidCastException at run time
}
int CalculateSum(int a, int b)
{
    return a + b;
}

[StructLayout(LayoutKind.Sequential)]
public struct MyStruct
{
    public int A; // 4 bytes
    public double B; // 8 bytes
    public long C; // 8 bytes
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
class TestClassWithSnippet
{

}