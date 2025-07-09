# Top 30 câu hỏi thường gặp trong Nodejs

# Câu hỏi phỏng vấn Fullstack JavaScript - Node.js & React.js

## PHẦN 1: KIẾN THỨC CƠ BẢN (30 câu)

### Node.js (15 câu)

1. **Node.js là gì? Tại sao nó được gọi là runtime environment?**

   - Node.js là runtime environment cho JavaScript được xây dựng trên V8 engine của Chrome
   - Cho phép chạy JavaScript ngoài trình duyệt

2. **Event Loop trong Node.js hoạt động như thế nào?**

   - Single-threaded event loop
   - Non-blocking I/O operations
   - Callback queue và call stack

3. **Phân biệt require() và import trong Node.js**

   - require() là CommonJS module system
   - import là ES6 module syntax
   - Cách sử dụng và khác biệt

4. **Middleware trong Express.js là gì?**

   - Functions chạy trong request-response cycle
   - Có thể modify req, res objects
   - Ví dụ: authentication, logging, error handling

5. **Async/Await vs Promise vs Callback trong Node.js**

   - Callback hell và cách giải quyết
   - Promise chains
   - Async/await syntax sugar

6. **Buffer và Stream trong Node.js**

   - Buffer để xử lý binary data
   - Stream để xử lý data chunks
   - Readable, Writable, Duplex, Transform streams

7. **Process.nextTick() và setImmediate() khác nhau như thế nào?**

   - Thứ tự thực hiện trong event loop
   - Khi nào sử dụng từng loại

8. **Cluster module trong Node.js**

   - Fork multiple processes
   - Load balancing
   - Scaling Node.js applications

9. **Error handling trong Node.js**

   - Try-catch với async/await
   - Error-first callbacks
   - Unhandled promise rejections

10. **Package.json và package-lock.json khác nhau gì?**

    - Dependency management
    - Version locking
    - Npm vs yarn

11. **REST API vs GraphQL**

    - Ưu nhược điểm của mỗi approach
    - Khi nào sử dụng GraphQL

12. **Authentication vs Authorization**

    - JWT tokens
    - Session-based auth
    - OAuth implementation

13. **Database connection pooling**

    - Tại sao cần connection pooling
    - Cách implement với MongoDB/MySQL

14. **Caching strategies trong Node.js**

    - Redis caching
    - In-memory caching
    - CDN caching

15. **Testing trong Node.js**
    - Unit testing với Jest/Mocha
    - Integration testing
    - API testing
