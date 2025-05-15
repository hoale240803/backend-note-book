1. Filter là gì?

2. Nó hoạt động như thế nào?
3. Nó được dùng cho những mục đích nào?
Logging: thêm một endpoints xung quanh controller actions và endpoint executions để nắm bắt các thông tin liên quan đến method, parameter, duration, outcomes.

Caching: sử dụng Resource Filters, nó có thể phân tích requests sớm và trả về response đã được cached mà không cần vào trong Controller logic.
- Cache output cho anonymoú GET request để giảm tải vào database
- Giảm thiểu các dữ liệu tốn kém nếu data được lưu vào bộ nhớ đệm

Validation: sử dụng ActionFilter và EndpointFilters
Custom validation filters có thấy được model state hoặc validate data đầu vào trước khi hành động được thực thi.

Auditing: dùng Action Filter
Có thể biết được ai là người thực hiện các hành động, khi nào, với data gì. Bạn có thể lưu trữ thông tin này trong database hoặc gửi cho audit service.

- Lưu lại hoạt động người dùng cho những hoạt động nhạy cảm.
- Lưu trữ ảnh chụp nhanh dữ liệu trước và sau

Input Sanitization: dùng ActionFilter
vệ sinh đầu vào, thay đổi hoặc vệ sinh đầu vào của model trước khi chúng chạm tới controller. Sử dụng những trimming strings, xóa đi những HTML không mong muốn, hoặc để chuẩn hóa các giá trị
- Xóa thể html ở nội dung bình luận
- Chuyển emails format sang lowercase trước khi validation

Metrics: Resource Filter hoặc Action Filter
Track thời gian thực thi, yêu cầu đếm, hoặc xảy ra lỗi cho endpoints.

4. Filter và middleware khác nhau điểm gì?
Filter và Middleware cả hai đều để cho bạn tiêm logic vào trong đường ống xử lý request, nhưng chúng vận hành theo các cấp độ khác nhau và phục vụ cho nhiều mục đích khác nhau. Trộn lẫn chúng như một cách sẽ biến app của bạn thành một nhà máy spaghetti không có lối thoát.

Middleware
Middleware chạy bên ngoài của MVC pipline và đóng gói toàn bộ HTTP request. Nó là điểm đầu và điểm cuối chạm vào request và response. Middleware không có khái niệm của controllers, actions, routing, hay model binding. Chỉ là nó làm việc với HTTP request thô.

Sử dụng middleware khi:
- Bạn cần sử lý một logic cho toàn bộ request, bất kể kiểu endpoint là (MVC, Razor Pages, Minimal APIs, static files, etc...)
- Bạn không quan tâm về controller/action layer
- Bạn cần kiểm soát sớm request (authentication, logging, cors, request buffering)
Examples:
Authentication/authorization middleware (useAuthentication, useAuthorization)
Logging and telemetry
Response compression
Custom headers for all response

Filters: nó chạy bên trong MVC hoặc là minimal API pipeline. Chúng nhận thức được routing, model binding, action results và exceptions.
Chúng được thiết kế cho controller hoắc end-point logic.

Sử dụng Filter khi:
Bạn đang làm việc bên trong MVC hoặc minimal APIs
Bạn cần tiếp cận action parameter, model state, hoặc action results
Bạn muốn thêm một logic vào cụ thể controller và actions
Bạn cần mỗi endpoint phải có những hành vi như là validation, logging, wrapping result,...

Examples:
Validating model state
Đóng gói response trong một hình dạng thống nhất
Thay đổi ủy quyền logic trên mỗi controller
Lưu lại lịch sử người dùng nào đã thực hiện những hành động nào.



https://codewithmukesh.com/blog/filters-in-aspnet-core/