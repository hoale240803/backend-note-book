1. What are the differences between ref and out keywords?
C# ref keywords pass arguments by reference and not value. To use the ‘ref’ keyword, you need to explicitly mention ‘ref’. 

```
void Method(ref int refArgument)
{
   refArgument = refArgument + 10;
}
int number = 1;
Method(ref number);
Console.WriteLine(number);
// Output: 11
```

C# out keywords pass arguments within methods and functions. 
‘out’ keyword is used to pass arguments in a method as a reference to return multiple values. Although it is the same as the ref keyword, the ref keyword needs to be initialised before it is passed. Here, The out and ref keywords are useful when we want to return a value in the same variables that are passed as an argument. 

2. What is the difference between an abstract class and an interface?

Let’s dig into the differences between an abstract class and an interface:

Abstract classes are classes that cannot be instantiated ie. that cannot create an object. The interface is like an abstract class because all the methods inside the interface are abstract methods.
Surprisingly, abstract classes can have both abstract and non-abstract methods but all the methods of an interface are abstract methods.
Since abstract classes can have both abstract and non-abstract methods, we need to use the Abstract keyword to declare abstract methods. But in the interface, there is no such need.
An abstract class has constructors while an interface encompasses 

3. What are extension methods in C#?

4. What is garbage collection in C#?

5. What is Common Language Runtime (CLR)?

C# Interview Questions for Experienced

6. What are partial classes in C#?
Partial classes implement the functionality of a single class into multiple files. These multiple files are combined into one during compile time. The partial class can be created using the partial keyword. 
You can easily split the functionalities of methods, interfaces, or structures into multiple files. You can even add nested partial classes. 

7. What is the difference between String and StringBuilder in C#?
The major difference between String and StringBuilder is that String objects are immutable while StringBuilder creates a mutable string of characters. StringBuilder will make the changes to the existing object rather than creating a new object.

StringBuilder simplifies the entire process of making changes to the existing string object. Since the String class is immutable, it is costlier to create a new object every time we need to make a change. So, the StringBuilder class comes into picture which can be evoked using the System.Text namespace.

In case, a string object will not change throughout the entire program, then use String class or else StringBuilder. 

For ex:

```
string s = string.Empty; 
for (i = 0; i < 1000; i++) 
  { 
    s += i.ToString() + " "; 
  }
```

Here, you’ll need to create 2001 objects out of which 2000 will be of no use.

The same can be applied using StringBuilder:
```
StringBuilder sb = new StringBuilder(); 
for (i = 0; i < 1000; i++) 
 { 
   sb.Append(i); sb.Append(' '); 
 }
```
By using StringBuilder here, you also de-stress the memory allocator. 

8. What is the difference between constant and readonly in C#?
In C#, a const keyword is used to declare constant fields and constant local. The value of the constant field is the same throughout the program or in other words, once the constant field is assigned the value of this field is not be changed. 

In C#, constant fields and locals are not variables, a constant is a number, string, null reference, boolean values. readonly keyword is used to declare a readonly variable. This readonly keyword shows that you can assign the variable only when you declare a variable or in a constructor of the same class in which it is declared.

9. What is Reflection in C#?
Reflection in C# extracts metadata from the datatypes during runtime. 

To add reflection in the .NET framework, simply use System.Refelction namespace in your program to retrieve the type which can be anything from:

Assembly
Module
Enum
MethodInfo
ConstructorInfo
MemberInfo
ParameterInfo
Type
FieldInfo
EventInfo
PropertyInfo


10. What are the different ways in which a method can be Overloaded in C#?
Overloading means when a method has the same name but carries different values to use in a different context. Only the main() method cannot be overloaded.
In order to overload methods in C#, 

Change the number of parameters in a method, or
Change the order of parameters in a method, or
Use different data types for parameters
In these ways, you can overload a method multiple times.


11. Difference between the Equality Operator (==) and Equals() Method in C#?
Although both are used to compare two objects by value, still they both are used differently. 

Equality operator (==) is a reference type which means that if equality operator is used, it will return true only if both the references point to the same object.  

Equals() method: Equals method is used to compare the values carried by the objects. int x=10, int y=10. If x==y is compared then, the values carried by x and y are compared which is equal and therefore they return true. 

12. What are Indexers in C#?
Indexers are called smart arrays that allow access to a member variable. Indexers allow member variables using the features of an array. They are created using the Indexer keyword. Indexers are not static members. 

13. What is Boxing and Unboxing in C#?
The two functions are used for typecasting the data types:

Boxing: Boxing converts value type (int, char, etc.) to reference type (object) which is an implicit conversion process using object value. 


14. What is the difference between a struct and a class in C#? 
A class is a user-defined blueprint or prototype from which objects are created. Basically, a class combines the fields and methods(member function which defines actions) into a single unit.

A structure is a collection of variables of different data types under a single unit. It is almost similar to a class because both are user-defined data types and both hold a bunch of different data types. To read more, refer to the article: struct and class in C#

In C#, a const keyword is used to declare constant fields and constant local. The value of the constant field is the same throughout the program or in other words, once the constant field is assigned the value of this field is not be changed. In C#, constant fields and locals are not variables, a constant is a number, string, null reference, boolean values. readonly keyword is used to declare a readonly variable. This readonly keyword shows that you can assign the variable only when you declare a variable or in a constructor of the same class in which it is declared.

15. What is a delegate in C#, and how is it used?

A delegate in C# is a type that represents a reference to a method. It allows methods to be passed as arguments, 
making it a key feature for event handling, callbacks, and functional programming.

Think of it as a function pointer, but type-safe and object-oriented.

How delegates work
A delegate acts as a wrapper around a method, allowing you to store and invoke it dynamically. 
Instead of calling a method directly, you assign it to a delegate and call the delegate instead.

Multicast delegates (calling multiple methods)
Delegates can also store multiple method references, meaning you can assign more than one method to the same delegate.

Func, Action, and Predicate Delegates
C# also provides built-in generic delegates to make working with delegates easier:




C# Interview Questions for Senior .NET Developer
Easy Questions

What is the difference between public, private, protected, and internal access modifiers in C#?
Explain the concept of a class versus a struct in C#.
What is the purpose of the using statement in C#?
How does the var keyword work, and when should it be used?
What is the difference between const and readonly in C#?
What are boxing and unboxing in C#? Provide an example.
How do you declare and initialize an array in C#?
What is the role of the Main method in a C# program?
Explain the difference between string and StringBuilder in C#.
What is a nullable type in C#, and how is it declared?

Medium Questions

How does the async and await keywords work in C#? Provide an example.
What is the purpose of interface in C#, and how does it differ from an abstract class?
Explain the concept of delegates in C# with a practical example.
What are extension methods, and how do you create one in C#?
How does garbage collection work in .NET, and what is the role of the Finalize method?
What is the difference between IEnumerable and IQueryable in C#?
How can you handle exceptions in C#? Explain try, catch, finally, and throw.
What are anonymous types in C#, and when would you use them?
Explain the yield keyword in C# and its use in iterators.
What is the difference between ref and out parameters in C#?

Advanced Questions

How does the Task Parallel Library (TPL) enhance parallelism in C#? Provide an example using Parallel.For.
Explain the concept of covariance and contravariance in C# generics with examples.
How would you implement a custom awaiter in C# to support asynchronous operations?
What are the implications of using unsafe code in C#, and when might it be necessary?
How does the System.Reflection namespace work, and how can you use it to invoke methods dynamically?
Explain the role of the volatile keyword in multi-threaded programming in C#.
How would you optimize memory usage in a large-scale C# application?
What is the difference between Span<T> and Memory<T> in C#, and when would you use each?
How can you implement a custom serialization mechanism in C# using ISerializable?
Explain the intricacies of dependency injection in C# using a framework like Microsoft.Extensions.DependencyInjection.

