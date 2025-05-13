# OOPs là gì?

[OOP](https://en.wikipedia.org/wiki/Object-oriented_programming) 
Lập trình hướng đối tượng là gì? là một mô hình lập trình dựa vào khái niệm *object*. Object có thể chứa (fields, attributes, properties) và hành động họ có thể thực hiện ( procedure or methods). Trong OOP, một chương trình máy tính được thiết kế sao cho những object này tương tác với nhau.

C# cũng là một ngôn ngữ lập trình hướng đối tượng. Có 4 nguyên tắc sau:

- Abstraction: mô hình hóa các thuộc tính **attributes** và **interactions** tương tác giữa các thực thể như là một class, định nghĩa thành một trình bày trừu tượng cho một hệ thống.

- Encapsulation: ẩn đi những đi những **state** trạng thái và các **functionality** chức năng của một object và chỉ cho phép truy cập thông qua một public set function.

- Inheritance Ability: tạo ra những cái abstractions mà dựa vào cái abstraction trước đó

- Polymorphism có thể kế thừa những thuộc tính hoặc phương thức theo nhiều cách khác nhau thông qua các abstractions.

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

Cái bài này sẽ cùng bạn tạo những types mới để thể hiện cho một tài khoản ngân hàng. Điển hình một dev phải định nghĩa những class trong những file text khác nhau. Điều này làm nó dễ hơn trong việc quản lý như là một chương trình phát triển size lớn dần lên. Bạn tạo ra một file **BankAccount.cs** trong thư thư mục **Classes**. Những classes có chưa dòng code mà nó biểu diễn những entity cụ thể. Cái lớp BankAccount là đại diện cho một tài khoản ngân hàng. Đoạn code thực thi thì sẽ gồm có những chức năng xuyên suốt các methods và properties. Trong hướng dẫn này, tài khoản ngân hàng hổ trở nhiều hành vi:

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
OOP là gì?

OOP là mô hình lập trình, tổ chức phần mềm theo class và object hơn là tính năng và thủ tục.

Class được định nghĩa như là một cấu trúc có sẵn (blueprint) có các properties(fields) và behavior(methods) của một objects.
Một object là một instance của class, được tạo ra ở thời gian chạy có chứa những data và behavior như đã được định nghĩa trong class.

**Encapsulation**: là nguyên tắc đóng gói các properties và methods trong một class để nó vận hành bên trong một đơn vị duy nhất (class), trong khi đó chặn các quyền truy cập từ các object's components

**Inheritance**: cho phép các lớp con - lớp dẫn xuất có thể kế thừa các properties, methods từ một lớp đã tạo ra trước đó.

**Polymorphism**: cho phép các lớp khác có thể đối xử, xử lý các objects của common class, tăng tính linh hoạt trong việc các methods được thực thi.

**Abstract**: là định nghĩa sẵn các implement chi tiết phức tạp bên trong và chỉ exposing các features quan trọng của một object. Bằng việc sử dụng interface abstract classes or interface.

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

**1. SRP(Single responsible):**
- Mỗi class/method chỉ có một việc

**EmailNofificationSender**: send email noti.
**SlackNotificationSender**: send Slack noti.
**ConsoleNotificationLogger**: logs infomation.
**NotificationService**: điều hướng gửi và logging.
**StandardNotificationProcessor**: Process notifications.
* methods **Send** & **Log** tập trung vào một nhiệm vụ

**2. Open/closed principle (OCP)**
- **NotificationService** là mở cho extension: thêm mới một sender (SMS sender) bằng việc **INotificationSender** mà không cần thay đổi **NotificationService**. 
- Không cần phải thay đổi code cũ để đáp ứng nhu cầu cho kênh thông báo mới.

**3. Liskov Substitution Principle(LSP)**
- **StandardNotificationProcessor** kế thừa từ **NotificationProcessor** và có thể sử dụng ở bất kì đâu trong **NotificationProccesor** được trông chơ không phá vỡ hành vi của nó.

Cái lớp cơ bản **NotificationProccessor** định nghĩa một hợp đồng (**ProcessNotifications**) mà các lớp con có thể kế thừa

**4. Interface vừa nhỏ và tập trung.**
- INotificationSender: chỉ dành cho việc notifications
- INotificationLogger: chỉ dành cho logging.

**5. Dependency Inversion Principle (DIP)**

- **NotificationService** phụ thuộc vào abstract (INotificationSender, INotificationLogger), không phải phụ thuộc vào lớp cụ thể.

**Design Pattern**
[Link](https://refactoring.guru/design-patterns/creational-patterns)

**1. Design pattern là gì?**
Design pattern là nhưng giải pháp điển hình cho những vấn đề xảy ra thường ngày trong thiết kế phần mềm. Chúng giống như là một bản giàn giáo dựng sẵn mà bạn có thể tùy chỉnh để giải quyết những vấn đề về thiết kế trong code.

Bạn không thể chỉ tìm xem một pattern và copy nó vào trong chương trình của bạn, giống như cách có thể với nhưng function hay thư viện có sẵn. Pattern không chỉ là một mảnh code, nó là một khái niệm chung cho việc giải quyết một vấn đề cụ thể nào đó. Bạn có thể đi theo cái pattern chi tiết và thực thi solution mà nó phù hợp với thực tế với chương trình của bạn.

Pattern thường hay nhầm lẫn với thuật toán, bởi vì cả hai khái niệm đều mô tả cách giải quyết về những vấn đề đã biết. Trong khi thuật toán luôn luôn định nghãi một tập các hành động rõ ràng để đạt được một vài mục tiêu, thì pattern là một mô tả cho cho giải pháp ở mức độ cao hơn. Đoạn code cùng pattern ứng dụng vào 2 chương trình khác nhau có thể khác nhau.

Một phép so sánh cho một thuật toán là công thức nấu ăn: cả hai đều là các bước rõ ràng để đạt được mục tiêu. Hay nói cách khác, một pattern giống với bản thiết kế hơn:bạn có thể xem những kết quả và những tính nă của nó là gì, nhưng thứ tự thực hiện thì lại phụ thuộc vào bạn.

**2. Pattern bao gồm những gì**
Hầu hết các patterns được mô tả rất bình thường, mọi người có thể tái sử dụng trong nhiều bối cảnh. Dưới đây là những phần mà nó thường xuyên trình bày trong một pattern:

**Intent** của một pattern mô tả ngắn gọn cả về vấn đề và solution.
**Motivation** giải thích sâu xa cho một vấn đề và giải pháp mà pattern có thể làm
**Structure** của những classes chỉ ra rằng mỗi phần của pattern và chúng được kết nối với nhau như thể nào.
**CodeExample** trong một số ngôn ngữ lập trình phổ biến nó có thể dễ dàng năm bắt được những idea đằng sau pattern 

**3. Lịch sử của pattern**
Ai là người tạo ra pattern? Thật tốt, nhưng không chính xác cho lắm. Design pattern không phải là những khái niệm khó hiểu, phức tạp mà là ngược lại. Pattern là những giải pháp điển hình có những vấn đề chung trong thiết kế hướng đối tượng. Khi một giải pháp lặp đi lặp lại ở nhiều project, một ai đó sẽ đưa ra một cái tên cho nó và mô tả giải pháp một cách chi tiết. Đây chính là điều cơ ban làm sao mà một pattern được khám phá ra.

Khái niệm của patterns lần đầu tiên mô tả là bởi Christopher Alexander trong quyển sách **A Patern Language Towns, Buildings, Construction**. Quyển sách này mô tả một "Ngôn ngữ" cho thiết kế môi trường đô thị. Những đơn vị của ngôn ngữ này là những pattern. Họ có thể mô tả những cửa sổ cao sẽ nên như thế nào, có bao nhiêu mức độ của tòa nhà nên đươc dựng, những khu vực xanh rộng lớn sẽ được bố trí như thế nào trong hàng xóm, và ...

Cái ý tưởng này được nhặt lên bởi 4 tác giả, Erich Gamma, John Vlissides, Raplh Johnson và Richard Heml. Năm 1994, họ đã xuất bản quyển sách **Design pattern: Elements of Reusable Object-Oriented Software**, trong đó họ đã ứng dụng khái niệm của design patterns vào trong lập trình. Quyển sách phát hành 23 pattern để giải quyết nhiều vấn đề của thiết kế hướng đối tượng và trở nên bán chạy rất nhanh. Chính vì nên quá dài nên mọi người bắt đầu gọi nó là "cuốn sách của 4 người", nó ngắn hơn là "The GoF book".

Từ đây, hàng tá các pattern hướng đối tượng ra đời. "Tiếp cận theo hướng pattern" này dần trờ nên phổ biến trong lĩnh vực lập trình, vì thế mà nhiều pattern ngày nay đã tồn tại bên ngoài thiết kế hướng đối tượng.

**3. Có mấy loại design pattern?**
Các design pattern khác nhau về độ phức tạp của chúng, mức độ chi tiết và mở rộng của việc ứng dụng cho toàn bộ hệ thống đang được thiết kế. Tôi thích sự giống nhau với xây dựng đường bộ: bạn có thể tạo ra một giao lộ an toàn bằng việc đặt một vài đèn giao thông hoặc xây một nút giao thông nhiều tầng với đường ngầm cho người đi bộ.

Cơ bản nhất và thấp nhất thường được gọi là idoms. Chúng thường chỉ đượng ứng dụng cho một chương trình đơn.

Cái cao nhất và các mẫu pattern cấp cao là **architectural pattern**. Lập trình viên có thể làm những pattern theo cách ảo hóa cho bất kì ngôn ngữ nào. Không giống như những pattern khác, họ có thể được sử dụng thiết kế kiến truc cho toàn bộ ứng dụng.

Hơn nữa, tất cả những pattern này được phân loại theo các intent hay mục đích của nó. Ở đây quyển sách này chỉ bao gồm 3 nhóm chính

**Creational pattern:** cung cấp cơ chế tạo object mà gia tăng sự linh hoạt và tái sử dụng code trước đó.

**Structural pattern** giải thích làm sao để tích hợp, láp ráp các đối tượng, class vào trong một cấu trúc lớn, trong khi vẫn giữ những cấu trúc đó được linh hoạt và hiệu quả.

**Behavioral pattern** chăm sóc tới việc giao tiếp hiệu quả và phân công trách nhiệm giữa những objects.

 **4. Creational Design Pattern**
 **Factory Method**: cung cấp một inteface việc tạo ra những đối tượng trong một lớp cha, nhưng cho những lớp con có thể thay đổi những kiểu object mà nó sẽ tạo ra.

# Problem
Tưởng tượng rằng bạn đang tạo một ứng dụng quản lý vận chuyển hàng hóa. Phiên bản đầu tiên của ứng dụng chỉ có thể xử lý các hình thức vận chuyển bằng xe tải, vì thế mà hàng đống code của bạn sống trong Truck Class.

Sau một khoản thời gian, ứng dụng của bạn trở nên phổ biến hơn. Mỗi ngày bạn nhận tới hàng nghìn requíe từ vận chuyển đường biển để tích hợp vận tải biển vào trong app của bạn.

Tin tốt đúng không? Code của bạn trông sẽ như thể nào? Tại thời điểm hiện tại, hầu hết code của bạn đều dính chặt vào trong Truck class. Giờ mà thêm Ships vào trong app sẽ yêu cầu thay đổi toàn bộ codebase. Hơn thế nữa, nếu bạn quyết định thêm một phương thức vận chuyển mới như hàng không, tàu lữa thì lại phải change thêm một lần nữa.

Như một điều tất yếu, bạn sẽ làm nó với những đoạn code khó hiểu, chằng chịt những điều kiện, để chuyển đổi hành vi dựa vào đối tượng vận chuyển.

Pre-refactor code
```
public class DeliveryService
{
    public void Deliver(string transportType, string cargo, int distance)
    {
        if (transportType == "truck")
        {
            var truck = new Truck();
            Console.WriteLine($"Using: {truck.Name}");
            truck.Deliver(cargo);
            Console.WriteLine($"Cost: {truck.CalculateCost(distance)}");
        }
        else if (transportType == "ship")
        {
            var ship = new Ship();
            Console.WriteLine($"Using: {ship.Name}");
            ship.Deliver(cargo);
            Console.WriteLine($"Cost: {ship.CalculateCost(distance)}");
        }
        else
        {
            Console.WriteLine("Unknown transport type");
        }
    }
}

After refactoring
```
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
```

public class Truck
{
    public string Name => "Truck";

    public void Deliver(string cargo)
    {
        Console.WriteLine($"Driving cargo: {cargo}");
    }

    public decimal CalculateCost(int distance)
    {
        return distance * 1.5m;
    }
}

public class Ship
{
    public string Name => "Ship";

    public void Deliver(string cargo)
    {
        Console.WriteLine($"Sailing with cargo: {cargo}");
    }

    public decimal CalculateCost(int distance)
    {
        return distance * 0.9m;
    }
}
```


 **Abstract Factory**: để bạn có thể sản xuất những dòng đối tượng có liên quan đến nhau mà không cần phải chỉ rõ những lớp cụ thể là gì.

 **Builder**: bạn có thể xây dựng những object phức tạp từng bước một. Pattern sẽ cho phép bạn sản xuất nhiều types khác nhau và trình bày một object sử dụng cùng cấu trúc code.

 **Prototype**: bạn có thể sao chép những objects mà không làm làm cho code của bạn phụ thuộc vào những lớp của nó.

 **Singleton**: bạn có thể đảm bảo rằng một class chỉ có một instance, trong khi nó được cung cấp điểm tiếp cận cho toàn cầu cho chính instance đó. 
