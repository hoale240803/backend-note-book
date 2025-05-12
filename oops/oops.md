# OOPs là gì?
https://en.wikipedia.org/wiki/Object-oriented_programming
- Lập trình hướng đối tượng là gì? là một mô hình lập trình dựa vào khái niệm *object*. Object có thể chứa (fields, attributes, properties) và hành động họ có thể thực hiện ( procedure or methods). Trong oop, một chương trình máy tính được thiết kế sao cho những object này tương tác với nhau.

C# cũng là một ngôn ngữ lập trình hướng đối tượng. Có 4 nguyên tắc sau:

Abstraction: mô hình hóa các thuộc tính **attributes** và **interactions** tương tác giữa các thực thể như là một class, định nghĩa thành một trình bày trừu tượng cho một hệ thống.

Encapsulation: ẩn đi những đi những **state** trạng thái và các **functionality** chức năng của một object và chỉ cho phép truy cập thông qua một public set function.

Inheritance Ability: tạo ra những cái abstractions mà dựa vào cái abstraction trước đó

Polymorphism có thể kế thừa những thuộc tính hoặc phương thức theo nhiều cách khác nhau thông qua các abstractions.


Để dễ hiểu về oop thì mình xin phép được tạo ra một cái ứng dụng chuyển tiền đơn giản để mô phỏng và giải thích kĩ các tính chất của oop nha.

Sử dụng terminal widow, tạo ra một thư mục đặt tên là **Classes**. Bạn sẽ xây dựng app ở đó. Bạn vào trong thư mục đó chạy lệnh

```
dotnet new console
```
Dòng cmd sẽ tạo ra môt ứng dụng. Bạn mở ra một class Program.cs thì bạn sẽ thấy một dòng command.
```
// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");
```

Cái bài này sẽ cùng bạn tảo a những types mới để thể hiện cho một tài khoản ngân hàng. Điển hình một dev phải định nghĩa những class trong những file text khác nhau. Điều này làm nó dễ hơn trong việc quản lý như là một chương trình phát triển size lớn dần lên. Bạn tạo ra một file **BankAccount.cs** trong thư thư mục **Classes**. Những classes có chưa dòng code mà nó biểu diễn những entity cụ thể. Cái lớp BankAccount là đại diện cho một tài khoản ngân hàng. Đoạn code thực thi thì sẽ gồm có những chức năng xuyên suốt các methods và properties. Trong hướng dẫn này, tài khoản ngân hàng hổ trở nhiều hành vi:

* Nó có 10 chữ số, mà nó là duy nhất để xác định tài khoản ngân hàng
* Nó có một chuỗi các tên cửa hàng và tên những người chủ của nó
* Có thể gọi cái số dư 
* Nó chấp nhận deposits
* Nó cho phép rút tiền
* số dư phải luôn dương
* Rút tiền thì không thể là số âm

```
namespace Classes;

public class BankAccount
{
    public string Number { get; }
    public string Owner { get; set; }
    public decimal Balance { get; }

    public void MakeDeposit(decimal amount, DateTime date, string note)
    {
    }

    public void MakeWithdrawal(decimal amount, DateTime date, string note)
    {
    }
}
```

Trước khi bắt đầu, hãy nhìn vào những gì mà chúng ta đã xây dựng. Cái namespace cung cấp một cách để tổ chức code một cách logic. Cái ví dụ này nhỏ nên tôi sẽ đặt vào bên trong 1 namespace.

**public class BankAccount** điều này định nghĩa class, type. Tất cả những thứ bên trong **{}**, điều này đi theo định nghĩa khai báo các trang thái và hành vi của class. Có năm **members** của **BankAccount** class. 3 cái đầu tiên được gọi là **properties**. Properties là data elements và có thể có code mà tăng cường validation hoặc nhưng nguyên tắc. 2 cái cuối cùng là **methods**. Methods là blocks của code mà có thể thực hiện ở trong một function. Đọc tên của mỗi members nên cung cấp đầy đủ thông tin cho những dev khác biết để hiểu clas này làm gì.

Open a new account
Cái điều đầu tiên để làm là nó mở một tài khoản ngân hàng. Khi một customer mở một account, họ cần cung cấp một khởi tạo balance, và thông tin về chủ hoặc nhiều chủ của account đó.

Tạo ra một object của **BankAccount** type nghĩa là bạn cần định nghĩa ra một **constructor** mà đã được gán nhưng values. Một **constructor** là một member mà có tên trùng với class.

tobecontinue


# Short version

1. OOP là gì?
- oop là mô hình lập trình, tổ chức phần mềm theo class và object hơn là tính năng và thủ tục.

Class được định nghĩa như là một cấu trúc có sẵn (blueprint) có các properties(fields) và behavior(methods) của một objects.
Một object là một instance của class, được tạo ra ở thời gian chạy có chứa những data và behavior như đã được định nghĩa trong class.

2. Encapsulation: là nguyên tắc đóng gói các properties và methods trong một class để nó vận hành bên trong một đơn vị duy nhất (class), trong khi đó chặn các quyền truy cập từ các object's components

3. Inheritance: cho phép các lớp con - lớp dẫn xuất có thể kế thừa các properties, methods từ một lớp đã tạo ra trước đó.

4. Polymorphism: cho phép các lớp khác có thể đối xử, xử lý các objects của common class, tăng tính linh hoạt trong việc các methods được thực thi.

5. Abstract: là định nghĩa sẵn các implement chi tiết phức tạp bên trong và chỉ exposing các features quan trọng của một object. Bằng việc sử dụng interface abstract classes or interface.

C# example cho tất cả các khái niệm.
Giả sử ta có một class **Vehicle**
Encapsulation: Vehicle sử dụng một **model** field, chỉ được tiếp cận từ Model property, để đảm bảo kiểm soát truy cập.
Inheritance: **Car** và **Motorcycle** kế thừa từ Vehicle, sử dụng lại cái property **Model** và **StartEngine** method.
Polymorphism: 
- Runtime: **Drive** method được overridden trong **Car** và **Motorcycle**, base class sẽ tham chiếu tới cái hàm Drive tương ứng.
- Compile-time: Bạn có thể thêm một cái hàm overloaded method (e.g Drive(int speed))

Abstraction: **Vehicle** ở base class mình sẽ định nghĩa một hàm abstract là Drive và bắt các class kế thừa từ nó phải override lại hàm này.


# SOLID
- S: Single responsibility: một class hay method chỉ nên làm một việc. Dễ hiểu, dễ test, dễ thay đổi, dễ fix bug

- O: Open/Closed Principle: Mở cho extesion, closed cho modification. Thêm mới features mà không cần đụng vào code cũ. Tránh rủi ro + hạn chế viết lại unit test.

- L: Liskov substitution: Nhưng **subclasses** phải có khả năng thay thế lớp cha mà không thay đổi sự đúng đắn của chúng.

- I: Interface segregation: sử dụng nhưng interface nhỏ và tập trung. Cung cấp các inteface vừa đủ, không quá phụ thuộc vào nhưng cái không cần thiết. Chia nhỏ những interface lớn thành những cái nhỏ hơn.

vd: thay vì mock một hàm printer có cả chức năng faxes và scan, chúng ta chỉ cần một interface IPrinter, IScanner thôi đơn giản và dễ test.

- D: Dependency inversion: phụ thuộc vào các abstractions, không phải vào một lớp cụ thể nào đó.
vd: chúng ta từ thông báo email sẽ dễ dàng chuyển Slack notification trong vòng 10 phút - chỉ cần thay đổi cái DI registration.

Giả sử chúng ta có một hệ thống thông báo, ở đó các message được gửi qua nhiều channel khác nhau(email, slack, googlechat,...). Làm sao có thể làm cái hệ thống đó theo nguyên tắc solid đc. Hay theo chân mình nhá.

1. SRP(Single responsible):
- Mỗi class/method chỉ có một việc

**EmailNofificationSender**: send email noti.
**SlackNotificationSender**: send Slack noti.
**ConsoleNotificationLogger**: logs infomation.
**NotificationService**: điều hướng gửi và logging.
**StandardNotificationProcessor**: Process notifications.
* methods **Send** & **Log** tập trung vào một nhiệm vụ

2. Open/closed principle (OCP)

- **NotificationService** là mở cho extension: thêm mới một sender (SMS sender) bằng việc **INotificationSender** mà không cần thay đổi **NotificationService**. 
- Không cần phải thay đổi code cũ để đáp ứng nhu cầu cho kênh thông báo mới.

3. Liskov Substitution Principle(LSP)
- **StandardNotificationProcessor** kế thừa từ **NotificationProcessor** và có thể sử dụng ở bất kì đâu trong **NotificationProccesor** được trông chơ không phá vỡ hành vi của nó.

Cái lớp cơ bản **NotificationProccessor** định nghĩa một hợp đồng (**ProcessNotifications**) mà các lớp con có thể kế thừa

4. Interface vừa nhỏ và tập trung.
- INotificationSender: chỉ dành cho việc notifications
- INotificationLogger: chỉ dành cho logging.

5. Dependency Inversion Principle (DIP)

- **NotificationService** phụ thuộc vào abstract (INotificationSender, INotificationLogger), không phải phụ thuộc vào lớp cụ thể.

- 