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

Console.WriteLine($"{number1.IntValue}"); // output: 12, noref = 10
Console.WriteLine($"{number2.IntValue}"); // output: 12, noref = 13

void DoubleValue(ref int x)
{
    x = x * 2; // 10
}
Animal a = new Mammal();

if (a is Reptile)
{
    Reptile r = (Reptile)a; // InvalidCastException at run time
}


class IntNumber
{
    public int IntValue { get; set; }
}

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