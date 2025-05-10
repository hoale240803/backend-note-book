# Overview
1. What's c#?
- Là ngôn ngữ hướng đối tượng, được phát triển bởi Microsoft, đầu năm 2000s. Được lãnh đạo bởi Anders Hejlberg. Nó sử dụng rỗng rãi cho asp.net, winform, wpf, unity.

https://en.wikipedia.org/wiki/C_Sharp_(programming_language)

2. Tại sao nó lại được tạo ra?
- Microsoft cần một ngôn ngữ để cạnh tranh với java và tránh rắc rồi pháp lý với Sun microsystems.

- James Gosling đã nói rằng c# là một phiên bản copy của java.

- Hejlsberg nói c# gần giống với c++ hơn.
- C# bắt nguồn từ một ký hiệu cao hơn nữa cung trong âm nhạc. # có nghĩa là 4 dấu cộng => phiên bản cao hơn c++

3. Những đặc trưng của ngôn ngữ này là gì?

4. Record vs class?

5. Type safety?

# 2. Fundamental
# 2.1 General Structure of C# program
C# program có thể có một hoặc nhiều file. 
Mỗi file chứa 0 or nhiều namespaces.
Mỗi namespace có chứa types class, struct, interfaces, enumeration, delegates hoặc namespaces khác.

Chỉ có 1 file kiểu top-level statement thôi. Điểm đầu vào của chương trình sẽ là dòng đầu tiên.

```
Console.WriteLine("Hello world!");
```

```
using System;

Console.WriteLine("Hello world!");

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
```

Get cert ở đây => https://code.visualstudio.com/docs/csharp/get-started

# 2.2. Type system
https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/

C# là một ngôn ngữ thiên về type. Mỗi variables và constant có một type, cũng như expression đều đánh giá được 1 giá trị.
Mỗi method khởi tạo điều cần phải chỉ rõ 1 cái tên, 1 cái type và một cái kind (value, reference, or output) cho mỗi input parameter và cho return value. Mỗi .NET library định nghĩa kiểu số built-in và kiểu phức tạp để trình bày một số lượng lớn constructs. Một chương trình c# điển hình sẽ sử dụng types từ class library và user-defined types. Khái niệm model là để chỉ cụ thể tới domain vấn đề của chường trình.

Nhưng thông tin được lưu trữ trong type có thể bao gồm items:
- **storage space** bộ nhớ một biến của cái type đó
- Maximum and mimimum values mà nó có thể biểu diễn
- **members** (methods, fields, events, ....) mà nó có chứa
- **base type** nó kế từ đâu
- **interfaces** mà nó implements
- **operations** mà nó được phép

# 2.2.1 Chỉ rõ types trong khai báo biến
Khi bạn khai báo một biến hay là một constant trong một program, bạn nên chỉ rõ nó là kiểu(type) gì hoặc dùng **use** keyword
để cho compliler can thiệp tới type. Dưới đây là những khai báo biến sử dụng built-in numberic types và user-defined types.

```
// Declaration only:
float temperature;
string name;
MyClass myClass;

// Declaration with initializers (four examples):
char firstLetter = 'C';
var limit = 3;
int[] source = [0, 1, 2, 3, 4, 5];
var query = from item in source
            where item <= limit
            select item;
```

Những types của method parameters và return values đã được chỉ định sẵn trong method declaration. Dưới đây là những signature thể hiện một method rằng yêu cầu một biến int như là một đối số và biến trả về là kiếu string.

```
public string GetName(int ID)
{
    if (ID < names.Length)
        return names[ID];
    else
        return String.Empty;
}
private string[] names = ["Spencer", "Sally", "Doug"];
```

Sau khi bạn khai báo biến này rồi. Bạn không thể tạo lại một new type, và bạn cũng không thể gán 1 value không tương thích với type mà đã khai báo. Ví dụ, bạn không thể khai báo một biến kiểu int và sau đó gán cho nó giá trị Boolean là true được. Tuy nhiên, những giá trị có thể được chuyển đổi thành những types khác, ví dụ khi mà chúng đang được gán vào những biến mới hoặc là truyền như là một tham số của một phương thức. một **Type Conversion** nó không phải là data bị mất đi do được thực hiện tự động bởi complỉe. Một cuộc chuyển đổi rằng sự mất mát của data yêu cầu một cast gì đó bên trong source code.

Đọc thêm: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/casting-and-type-conversions

# 2.2.1.1 Casting and type conversions

Bởi vì c# được ép kiểu cố định tại thời điểm biên dịch, sau khi một biến được khai báo, nó không thể được khai báo lại thêm một lần nào nữa hoặc gán vào một giá trị của một type khác mà không phải là một kiểu ngầm định có thể thể chuyên đổi sang type của biến đó. Ví dụ, string không thể ngầm định chuyển đổi sang int. vì thế, sau khi bạn khai báo biến i như một biến int, bạn không thể gán một string "Hello" cho nó được.

```
int i;

// error CS0029: can't implicitly convert type 'string' to 'int'
i = "Hello";

```

Tuy nhiên, ban có thể thỉnh thoảng cần copy value vào bên trong variable hoặc method parameter của một type khác. Ví dụ,
bạn có một biến integer bạn muốn truyền vào một method như là môt tham số như kiểu double. Hoặc bạn có thể cần gán một biến của một class vào trong một biến của interface. Những kiểu hoạt động như này thì được gọi là **type conversions**. Trong c#, bạn có thể thực hiện theo những cách chuyển đổi như sau:

- Implicit conersions: Không một cú pháp nào được yêu cầu ở đây bởi vì sự chuyển đổi này luôn thành công và không có bất kì sự mất mát data nào ở đây cả.

- Explicit conversion (casts): ở đây nó cần tường minh, rõ ràng và yêu cần môt cái **cast expression**. Việc đúc dữ liệu kiểu này có thể bị mất mát dữ liệu khi chuyển đổi, hoặc do việc chuyển đổi không thành công do nhiều lý do khác. Ví dụ điển hình là việc chuyển đổi số sang một kiểu có độ chính xác thấp hơn hoặc biên độ giá trị nhỏ hơn, và việc chuyển đổi của một base-class instance tời derived class.

- User-defined conversions: user-defined conversions sử dụng một phương thức đặc biệc rằng bạn có thể sử dụng những phương thức đặc biệt để bật explicit và implicit conversions giữa những custom types mà không phải có mối quan hệ base class-derived. 
Xem thêm ở đây: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/operators/user-defined-conversion-operators

# 2.2.1.1 Implicit conversions
Đối với nhưng kiểu built-in numeric, một cuộc chuyển đổi ngầm có thể làm ra khi mà giá trị được lưu trữ phù hợp với biến mà không bị lược bỏ hay làm tròn. Ví dụ kiểu integer types, sự giới hạn này có nghĩa là một khoảng giá trị của kiểu nguồn là đúng hoàn toàn với subset của kiểu đích. Ví dụ, một biến kiểu long(64bit integer) có thể lưu bất kì giá trị nào của int(32integer).

```
int num = 2147483647;
long bigNum = num;
```

Với kiểu tham chiếu, một cuộc chuyển đổi luôn luôn tồn tại từ một class cho tới bất kì một cách trực tiếp hay gián tiếp các lớp cha hay là interface. Không cần cú pháp nào thực hiện ở đây bởi vì dẫn xuất(lớp con) luôn luôn kế thừa các members của 1 lớp cơ cở (lớp cha)

```
Derived d = new Derived();

// Always OK.
Base b = d;
```
- Conversions with helper classes:

# 2.2.1.2 Explicit conversions
Tuy nhiên, nếu một chuyển đổi không thể thực hiện mà không có rủi ro của sự mất mát thông tin, lúc này compiler sẽ yêu cầu bạn cần được thực hiện ép kiểu một cách rõ ràng, nó gọi là **cast**. Một cast là một con đương chuyển đổi rõ ràng. Nó chỉ ra rằng bạn nên nhận thức được mất mát dữ liệu có thể xảy ra, hoặc cast này có thể thất bại ở thời gian chạy.

```
double x = 1234.7;
int a;
// Cast double to int.
a = (int)x;
Console.WriteLine(a);
// Output: 1234
```

# 2.2.1.3 Type conversion exceptions at run time
Trong một số cuộc chuyển đổi reference type, complier không thể xác định được rằng liệu cast này có hợp lệ hay không. Điều này có thể là khi một hành động cast được biên dịch thành công nhưng lại fail ở thời gian chạy. Như ví dụ dưới đây.

```
Animal a = new Mammal();
Reptile r = (Reptile)a; // InvalidCastException at run time
```
Ép kiểu tường mình đối số **a** sang **Reptile** tạo nên một giả sử nguy hiểm. Nó sẽ an toàn hơn nếu không đưa ra giả định này, mà hãy là kiểm tra type

# 2.2.1.4 C# language specification

# 2.2.2 Built-in types

# 2.2.3 Custom types


# 2.2.2. Stack vs Heap
Stack: nhanh, nhỏ, và tự động.
- Dùng cho: 
    - value types int, double, bool, struct
    - Method paramerers
    - Local varibles

```
void Add() {
    int x = 5; // 🎒 Goes on the stack
}
```
- Khi mà cuối **Add()** thì **x** sẽ tự động xóa đi.

Heap: flexible, larger, but slower

Dùng cho:
- References type: class, string, array, List<T>
- Objects mà nằm bên ngoài một method call

```
void Add() {
    var user = new User(); // 🧳 Goes on the heap
}
```
sau khi **Add()** thì user vẫn còn sống cho tới **GC** sẽ thu thập nó


Summary:

Value Type:
- Built-in type: 
    - Integral types: int, long, short, ushort, int, unit, long, ulong.
    - Floating point: float, double
    - Demical: decimal
    - Boolean: bool
    - Character: char
- User define type: struct, enum
- Others: Datetime, TimeSpan, Guid, Nulllable<T>

Reference Type:
- Built-in type: object, string, dynamic, array[]
- User define type: class, interface, delegate, record


Key differences
| Feature               | Value Type                        | Reference Type        |
| --------------------- | --------------------------------- | --------------------- |
| **Memory Allocation** | Stack                             | Heap                  |
| **Contains**          | Actual data                       | Reference to data     |
| **Assignment**        | Copies the value                  | Copies the reference  |
| **Nullability**       | Not nullable (unless using `?`)   | Nullable by default   |
| **Performance**       | Generally faster (no GC overhead) | May incur GC overhead |

Ref: https://www.shekhali.com/value-type-and-reference-type-in-c/



