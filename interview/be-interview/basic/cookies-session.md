1. Cookies

Một cookies ( biết như là một web cookies hoặc trình duyệt cookie) là một miếng nhỏ data của server gửi cho trình duyệt web của người dùng.
Trình duyệt có thể lưu trữ cookies, tạo ra cookies mới, thay đổi, và gửi nó trở lại cho server trước đó với những request khác. Cookies bật ứng dụng web để lưu trữ một lượng lớn dữ liệu và nhớ trạng thái thông tin; mặc định thì nó dùng HTTP protocol là **stateless**

Trong bài này chúng ta sẽ khám phá chức năng chính của cookies, giải thích cách dùng tốt nhất với nó và xem qua về chính sách riêng từ và tác động bảo mật của nó.

Cookies được sử dụng cho

Điển hình, server sẽ sử dụng nội dung của HTTP cookies để xác định liệu rằng nhiều requests khác nhau có đến từ một brower/user và sau đó phát hành một phản hồi cá nhân hóa hoặc chung chung cho phù hợp. Những mô tả dưới đây là hệ thống đăng nhập người dùng cơ bản:

1. Người dùng gửi thông tin đăng nhập tới máy chủ, ví dụ thông qua một form đăng nhập.
2. Nếu thông tin này hợp lệ, thì server sẽ thay đổi UI để chỉ ra rằng người dùng đã đăng nhập, và trả về một cookie có chứa một session Id và nó có lưu lại trạng thái đang nhập của browser.
3. vào môt thời điểm sau đó, người dùng chuyển sang một page khác mà cùng site. Trình duyệt sẽ gửi cookie có chứa session Id kèm với request tương ứng để chỉ ra rằng nó vẫn là người dùng đã đăng nhập.
4. Server kiếm tra session Id và nếu nó vẫn hợp lệ, gửi tới người dùng một phiên bản cá nhân hóa của trang mới đó. Nếu nó không hợp lệ, sessionId sẽ bị xúa và người dùng sẽ được xem một trang chung chung của page đó (hoặc là có thể thấy một trang access denied và yêu cầu đăng nhập lại)

Cookies chủ yếu sử dụng cho 3 mục đích chính:
**Quản lý phiên đăng nhập**: người dùng đăng ký trang thái, quản lý giỏ hàng, lưu bảng điểm trong game, hoặc những thông tin liên quan tới phiên của người dùng mà server cần phải nhớ.
**Cá nhân hóa**: Sở thích người dùng như là hiển thị ngôn ngữ và màn hình nên UI.
**Theo dõi**: lưu trữ và phân tích hành vi người dùng.

Data Storage ( dung lượng dữ liệu)
Nhưng ngày đầu của web không có một lựa chọn nào khác, thì cookies được sử dụng mục đích lưu trữ dữ liệu chung của khách hàng. Lưu trữ hiện đại ngày này giới thiệu nhiều phương án khác, ví dụ nhưn **Web storage API** và **sessionStorage** và IndexedDB.

Họ được thiết kế với dung lượng trong đầu, không bao giờ gửi data cho server và không đi kèm với nhược điểm khác khi sử dụng cookie cho việc lưu trữ:
- Những trình duyệt thường sẽ giới hạn tối đa số lượng cookies trên một domain (thay đổi tùy theo trình duyệt, thường thì hăng trăm), kích thước tối đa của một cookie là (4kb). Storage APIs có thể lưu trữ lớn hơn.

- Cookies được gửi với mỗi request, vì thế chúng có thể làm chậm hiệu năng đi (ví dụ chậm kết nối trên mobile), đặc biệt là nếu bạn có nhiều tập cookies.





