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

# 2.2.2 Built-in types
https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/built-in-types

# 2.2.3 Custom types
Bạn có thể dùng struc, clas, intèace, enum và record xây dựng nên type cho riêng mình. .NET library bản thân nó là một tập hợp các custom types mà bạn có thể sử dụng trong ứng dụng của bạn. Mặc định, cái used types mà thường xuyên sử dụng nhất trong thư viện thì đã có sẵn ở trong chương trình c#. Những types các khác thì chỉ có sẵn khi bạn thêm rõ ràng một tham chiếu project tới assembly mà đã định nghĩa chung. Sau khi trình biên dịch có tham chiếu tời assembly, bạn có thể khai báo biến (và constants) của tyeps đã được khai báo trong assembly bên trong source code. Thêm chi tiết: https://learn.microsoft.com/en-us/dotnet/standard/class-library-overview

Một trong những quyết định đầu tiên bạn làm khi định nghĩa một type là định nghĩa cái hàm dựng nào được sử dụng cho type của bạn. Danh sách dưới đây giúp bạn tạo ra nhưng khởi tạo ban đầu. Có sự trùng lặp trong việc lựa chọn. Nhưng hầu hết các trường hợp, nhiều hơn 1 sẽ là lựa chọn hợp lý.
- Nếu data storage size mà nhỏ, không quá 64 bytes, chọn 1 **struct** hoặc là **record struct**.
- Nếu type là bất biến -immutable, hoặc bạn chọn một đột biến không phá hủy sự thay đổi, chọn struct hoặc record struct.
- Nếu type chủ yếu là dùng cho lưu trữ data, không có các hành vi, chọn một **record class** or **record struct**
- Nếu type là một phần của kế thừa cấp bậc, chọn **record class** hoăc một **class**

# 2.2.4 The common type system
Điều quan trọng nhất là phải hiểu 2 điểm quan trọng nhất về type system trong .net:
- Nó hổ trợ nguyên tắc về kế thừa. Những kiểu này có thể dẫn xuất từ kiểu khác, nó được gọi là base types. Các type dẫn xuất này kế thừa (một vài sự ngăn chặn) ví dụ như là methodss, thuộc tính và một số members của basse type. Cái base type có thể nằm bên trong dẫn xuất từ một có type khác, trong trường hợp nó dẫn xuất type được ké thừa từ nhnưgx membé của cả base type trong kế thừa cấp bậc. Tất cả các types, nó bao gồm cả built-in numeric types vd system.int32(c# từ khóa int)
nó dẫn xuất cuối cùng từ một base type duy nhất, nó là system.Object(c# keyword: object). Dieuè nàu đã thhông nhất typ thoe cấp bậc nó được gọi là CTS(CommonTypeSystem). Thêm thông tin: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/object-oriented/inheritance

- Mỗi type trong CTS đã được định nghĩa như là một value type hoặc là reference type. Những types này bao gôm tất cả custom types trong .NET clas library và cũng có các usẻ-defined types. Những types mà bạn định nghĩa bằng struct keyword là value types; bất cả built-in numberric types là structs. Những types mà bạn khai báo sử dụng là class hoặc là record keyword thì đều là reference types. Những ref types và value types này có nhiều cái quý tắc compile-time khác nhau, và run-time behavior cũng khác nhau.

Chú ý: bạn có thể thấy rằng cái used types mà hay sử dụng nhất là tất cả được tổ chức trong **System** namespace. Tuy nhiên, cái namespace thì nó không có chưa quan hệ tới liệu rằng nó là value type hay ref type cả.

Classes và Structs là 2 constructss cơ bản của common type system trong .net. Chúng điều là quan trong đối với dữ liệu kiến trúc mà được đóng gói theo tập data và behaviors nó buộc về với nhau nhưng là một đơn vị logic. Cái data và behavior là nhưng members của class, struct, record. Những members bao gồm những methods, properties, events, ...của nó. Nó được liệt kê dươis đây.

Một class, struct, hoặc là record khai báo ra nó như là một blueprint that nó tạo ra một instances hoặc objects tại runtime. Nếu bạn define 1 class, struct, record tên là Person, Person là tên của type. Nếu bạn khai báo và khởi tạo 1 viến p của type Person, p nó được gọi là một object hoặc một thực thể của Person. Nhiều instances của cùng một Person type có thể được tảo a và mỗi instan can thể nhiều giá trị khác nhau bên trong properties và fields.

Một class là một referent type. Khí một object của type được tạo ra, biến của object đó được tham chiếu tới bộ nhớ. Khi một object reference được gán cho một biến mới, thì biến mới đó sẽ tham chiếu tới original object. Những thay đổi được tạo ra thông qua một biến được phản ánh qua object khác bởi vì cả hai chúng điều tham chiếu tới cùng dữ liệu.

Một struct là một value type. khi mà một struct được tạo ra, biến của struct sẽ được gán giữ chọ data thực. Khi mà struct đc gán cho một biến mới, nghía là nó được copied. Biến mới này và biến cũ vì thế chưa 2 phiên bản copies khác nhau nhưng giống về data. Nhưng thay đổi của một object sẽ không bị ảnh hưởng bởi cái còn lại.

Nói tóm lại, những classes được sử dụng cho model có những behavior phức tap. Classes điển hình thì nó lưu trữ data mà nó dự định sẽ thay đổi sau khi được tạo ra. Structs một phương án tốt nhất cho cấu trúc data nhỏ. Struct điển hình chỉ luwu trữ data mà không dự định thay đổi sau khi struct được tạo ra. Record type là cấu trúc data có thêm thành viên của trình biên dịch. Records điển hình để lưu trữ data mà không có dự định thay đổi sau khi tạo ra.

# 2.2.5 Value types
Value types dẫn xuất từ **System.ValueType**, nó được dẫn xuất từ **System.Object**. Những types  dẫn xuất từ **System.ValueType** có nhưng behavior đặt biệt từ CLR. Những biến Value type trực tiếp chứa những values của chúng. Bộ nhớ cho một struct nó được cấp phát bên trong những gì context của varible được khai báo. Không có cấp phát bộ nhớ heap hoặc tập rác quá nhiều cho biến value type. Bạn có thể khai báo record struct types mà những value types và tập hợp những members cho records.

Có 2 kiểu value types: struct và enum:

cái built-in numeric types là structss, vàchúng có những fields mà methodss mà bạn có thể tiếp cận.

```
// constant field on type byte.
byte b = byte.MaxValue;
```

Value types được niêm phong. Bạn không thể dẫn xuất một type nào khác, ví dụ như **Systemt.Int32**. Bạn không theẻ định nghĩa một struct cho việc ké thừa từ user-defined class hoặc struct bời vì struct chỉ kế thừ từ **System.ValueType**. Tuy nhiên, một struct có thể implêmnt một hay nhiều interfaces. Bạn có thể ép kiểu một struct type tời bất kì  interface type nòa mà nó implêmntss. Việc casst này là nguyên nhân của việc boxing để wrapt the struct inside một reference type object vào heap đã quản ly. Boxing hoạt động này xảy ra khi bạn truyền một value type cho một method mà đảm nhiệm bởi System.Object hoặc bất kì interface type nào nhưn là một input parramêtr. Thêm chi tiết: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/types/boxing-and-unboxing

Một số kieuẻ value types khác như là enum. Một enum được định nghĩa như một tập kiểm số nguyên dương hằng số. Ví dụ như System.IO.FileMode enumeration trong .NET library có chứa một tập tcác hằng số theo kiểu số nguyên nó chỉ ra rằng làm sao một file nên được mở ra. Nó cũng được định nghĩa theo ví dụ dưới đây

```
public enum FileMode
{
    CreateNew = 1,
    Create = 2,
    Open = 3,
    OpenOrCreate = 4,
    Truncate = 5,
    Append = 6,
}
```

System.IO.FileMode.Create hằng số này có giá trị là 2. Tuy nhiên, tên thì mang nhiều ý nghĩa hơn cho con người để đọc trong source code, và chính vì lý do này nó sẽ tốt hơn để sử dụng thay thế có constant.

Tất cả các enums đều kế thừa thừ System.Enum, nó kế thừa từ **System.ValueType**
Thêm chi tiết: https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/enum


# 2.2.6 Reference types
Một type mà định nghĩa như là class, record, delegate, array, hoặc là interface thì chúng được coi là reference type.

Khi bạn khai bao biến của một reference type, nó có chưa giá trị null cho tới khi bạn gán cho nó một instance của một type hoặc bạn tạo ra một using với new operator. Tạo và gán của một class được mô tả theo ví dụ dưới đây.

```
MyClass myClass = new MyClass();
MyClass myClass2 = myClass;
```
Một interface không thể trực tiếp khởi tạo bằng việc sử dụng new dược. Thay vao đó tạo và gán một instance của một class mà nó implementes cái interface.

```
MyClass myClass = new MyClass();

// Declare and assign using an existing value.
IMyInterface myInterface = myClass;

// Or create and assign a value in a single statement.
IMyInterface myInterface2 = new MyClass();
```

Khi mà một object được tạo ra, bộ nó được cấp phát trên managed heap. Biến chỉ giữ một tham chiếu tới vị trí của object. Những types nằm trên heap yêu cầu cao cả về chúng được phân bổ và thu hôi như thế nào. GC sẽ tự động quản lý bộ nhớ của CLR, điều này được thực hiện chó việc thu hồi. Tuy nhiên GC cũng được tối ưu cao, và hầu hết các trường hợp nó không bị vấn đề về hiệu năng nào.
Xem thêm: https://learn.microsoft.com/en-us/dotnet/standard/automatic-memory-management

Tất cả các arráy là reference types, thậm chí nếu tất cả element là value types. Arrays nó sẽ dẫn xuất rõ ràng từ System.Array class. Bạn khai báo và sử dụng chúng với cú pháp đơn giản cung cấp vởi c#:
```
// Declare and initialize an array of integers.
int[] nums = [1, 2, 3, 4, 5];

// Access an instance property of System.Array.
int len = nums.Length;
```

Reference types hoàn toàn hổ trợ kế thừa. Khi mà bạn tạo ra một class, bạn có thể kế thừa từ bất kì inteface hay là class mà nó không định nghĩa là **sealed**. Những lớp khác có thể kế thừa thừ lớp của bạn và override tại nhưng virtual methods của bạn.


# 2.2.7 Generic types
Một type có thể dược khai báo với một hoặc nhiều type parameters mà nó đóng vai trò như là chổ giữ chổ cho actual type (concrete type). Khách hàng sẽ cung cấp một concrete type khi mà nó khởi tạo một instant của type đó. Ví dụ types gọi là generic types.
**System.Collections.Generic.List<T>** có một type parameter thoe convention cho thên là T. Khi bạn tạo một instance của type này, bạn chỉ rõ ra cái kiểu type objects mà list này cân chứa.

```
List<string> stringList = new List<string>();
stringList.Add("String example");
// compile time error adding a type other than a string:
stringList.Add(4);
```

Việc sử dụng type parameter này làm cho nó có thể tái sử dụng nếu trùng class để giữ bất kì phần tử nào, mà không phải convert mỗi phần tử sang object. Generic collection classé được gọi là strongly typed collections bởi vì trình biên dịch biết cái type cụ thể của  các phần từ trong collection này và có thể báo lỗi ở compile time.

# 2.2.8 Implicit types, anonymous types, and nullable value types
Bạn có thể ngâm định kiể cho một biến local (nhưng không phải là một member của class) bằng việc sử dụng var keyword.

Biến thì vẫn nhận một type tại thời điểm biên dịch, nhưng type mà được cung cấp cho compiler.

Xem thêm: https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/implicitly-typed-local-variables

Nó có thể không thuâtnj tiên đêr tạo một named type cho một tập giá trị liên quan đơn giản, rằng bạn không có ý định lưu giữ nó truyền ra ngoài boundaries. Bạn có thể tạo ra anonymous cho mục địch này.
Xem thêm: https://learn.microsoft.com/en-us/dotnet/csharp/fundamentals/types/anonymous-types

Thông thường thì các value types không thể có giá trị null. Nhưng mà, bạn có thể tạo ra null value types bằng việc thêm dấu hỏi đằng sau type đó. Ví dụ, int? là một kiểu int type mà có thể có giá trị null. Nullable value types là những cái instances của generic struct type **System.Nullable<T>**. Nullable value types nó đặc biệt hữu ích khi mà bạn truyền data xuống và nhận lên từ databases, những numeric values có thể bị null. 
Xem thêm https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/nullable-value-types


# 2.2.9 Compile-time type and run-time type
Một biến có compile-time và run-time types khác nhau. compile-time type là được khai báo hoặc được suy ra từ một biến trong source code. run-time type là một type của instance tham chiếu tới biến đó. Thường thì 2 types này giống nhau, nhưn ví dụ dưới đây:

```
string message = "This is a string of characters";
```

Một trường hợp khác, compile-time type là khác nhau, ví dụ như 2 cái dưới đây:

```
object anotherMessage = "This is another string of characters";
IEnumerable<char> someCharacters = "abcdefghijklmnopqrstuvwxyz";
```

Cả hai ví dụ trước, run-time type là một string, compile-time là một object ở dòng đầu tiên, và IEnummerable<char> dòng thứ 2.

Nếu 2 types là khác cho một viến, điều quan trọng để hiểu khi mà compile-time type và run-time type được apply. compile-time type xác định tất cả các hành động đảm nhiệm bởi compiler. Những hành động của compiler bao gồm các phương thức gọi resolution, overload resoution và biến ngầm định và explicit casts. run-time type xác định tất cả hành động mà đã được giải quyết ở run-time. Những hành động ở thời điểm run-time này bao gồm gửi virual method calls, đánh giá **is** và **switch** expression, và nhiều type testing apis khác. Để hiểu hơn về làm sao mà code của bạn có thể tương tác với nhiều types, và nhận định được hành động nào sẽ ap đặt lên type nào.


# 2.2.10 Stack vs Heap
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

# 3. Object-oriented programming
https://en.wikipedia.org/wiki/Object-oriented_programming
- Lập trình hướng đối tượng là gì? là một mô hình lập trình dựa vào khái niệm *object*. Object có thể chứa (fields, attributes, properties) và hành động họ có thể thực hiện ( procedure or methods). Trong oop, một chương trình máy tính được thiết kế sao cho những object này tương tác với nhau.

C# cũng là một ngôn ngữ lập trình hướng đối tượng. Có 4 nguyên tắc sau:

Abstraction: mô hình hóa các thuộc tính **attributes** và **interactions** tương tác giữa các thực thể như là một class, định nghĩa thành một trình bày trừu tượng cho một hệ thống.

Encapsulation: ẩn đi những đi những **state** trạng thái và các **functionality** chức năng của một object và chỉ cho phép truy cập thông qua một public set function.

Inheritance Ability: tạo ra những cái abstractions mà dựa vào cái abstraction trước đó

Polymorphism có thể kế thừa những thuộc tính hoặc phương thức theo nhiều cách khác nhau thông qua các abstractions.


# 4. Functional techniques

# 5. Exceptions and errors

# 6. Coding styles

## 6.1 C# identifier names

Tools có thể giúp nhóm tăng cường convetion. Bạn có thể bật code analysis để đặt rules mà bạn thích hơn.
Bạn cũng có thể tạo ra một **editorconfig** visual studio tự động tạo để tăng cường hướng dẫn style của bạn. Như một điểm khởi đầu, bạn có thể copy file dotnet/dóc.editổcnfig để sử dụng theo style của chúng tôi.

Những tools này giúp team bạn đáp ứng được về guidelines. Visual studio áp dụng những rule này trong tất cả file. editorconfig file trong scope để fỏmat code của bạn. Bạn có thể sử dụng nhiều file configủaiton để tăng cường convention cho toàn tập đoàn của bạn, team thập chí là một dự án nhỏ.

### 6.1.1 