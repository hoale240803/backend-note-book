## PHẦN 1: KIẾN THỨC CƠ BẢN (15 câu)

### React.js (15 câu)

#### **16. Virtual DOM trong React hoạt động như thế nào?**

- Virtual DOM ( Document Object Model ảo) là một cơ chế quan trọng trong React giúp cải thiện hiệu suất bằng giảm thiểu các thao tác trực tiếp lên DOM thật.

  - Reconciliation process(Quá trình đối chiếu): Khi trạng thái (state) của một component thay đổi, React sẽ xây dựng lại một cây Virtual DOM mới. Sau đó, nó so sánh cây Virtual DOM mới này với cây Virtual DOM trước đó để xác định những thay đổi cần thiết.

  - Diffing algorithm: (Thuật toán so sánh): Đây là thuật toán được sử dụng để so sánh hai cây Virtual DOM. Thuật toán này rất hiệu quả, chỉ tập trung vào việc tìm ra sự khác biệt giữa các phần tử và thuộc tính, k cần phải tạo lại toàn bộ cây DOM.

  - Performance benefits ( Lợi ích về hiệu suất): Thay vì thao tác trực tiếp lên DOM thât(là một quá trình tốn kém), React sẽ thực hiện tất cả phép tính trên Virtual DOM. Khi đã tìm ra sự thay đổi, nó sẽ nhóm chúng lại và chỉ cập nhật DOM thật một lần duy nhất, giúp tối ưu hiệu suất đáng kể.

**1. Tại sao thao tác trực tiếp trên DOM thật lại châm, và bị ảnh hưởng hiệu năng?**

    1. DOM thật là một API của trình duyệt
        - Mỗi lần bạn thay đổi DOM thật (VD: element.innerText = "new value"), trình duyệt phải:
            - Truy cập cấu trúc cây DOM
            - Cập nhật node.
            - Tính toán lại layout(reflow).
            - Vẽ lại(repaint)
        - Những thao tác này rất tốn chi phí tính toán, đặc biệt khi có nhiều phần tử hoặc thay đổi xảy ra liên tục.

    2. Reflow & Repaint(tái bố trí và vẽ lại)
        - Reflow xảy ra khi trình duyệt cần tính toán lại vị trí & kích thước các phần tử do thay đổi layout.
        - Repaint là khi trình duyệt phải vẽ lại các pixel trên màn hình
        - Càng nhiều thao tác DOM -> càng nhiều reflow/repaint -> càng chậm

```html
<ul id="list"></ul>

<script>
  const ul = document.getElementById("list");
  for (let i = 0; i < 1000; i++) {
    const li = document.createElement("li");
    li.innerText = `Item ${i}`;
    ul.appendChild(li); // Thao tác DOM 1000 lần!
  }
</script>
```

- Ở ví dụ trên, trình duyệt có thể phải reflow/repaint hàng trăm lần do mỗi appendChild là một thay đổi DOM.
- Cách React xử lý (Virtual DOM)

```jsx
const items = Array.from({ length: 1000 }, (_, i) => <li key={i}>Item {i}</li>);
return <ul>{items}</ul>;
```

- React sẽ:
  - Tạo cây virtual DOM trước.
  - So sánh với DOM cũ(nếu có)
  - Nhóm tất cả thay đổi thành một lần update duy nhất
  - Gửi update này với DOM thật -> Chỉ tốn một lần reflow/repaint.

#### **17. State vs Props trong React**

- Khái niệm cốt lõi của React để quản lý dữ liệu trong các component
  - Immutable props(Props bất biến): - Props là dữ liệu được truyền từ component cha xuống component con. - Nghĩa là component con không thể trực tiếp thay đổi giá trị của props mà nó nhận được.

```js
// Component cha
function ParentComponent() {
  const name = "Alice";
  return <ChildComponent userName={name} />;
}

// Component con
function ChildComponent(props) {
  // props.userName là 'Alice' và không thể thay đổi trực tiếp ở đây
  return <h1>Hello, {props.userName}</h1>;
}
```

    - Mutable state (State có thể thay đổi): State là dữ liệu được quản lý bên trong một component và có thể thay đổi theo thời gian.
    - Nó là có thể thay đổi, nhưng việc thay đổi phải thông qua hàm `setState` (đối với class components) hoặc hàm setter cuar useState(đối với functional components) để React có thể nhận biết và render lại component.
    ví dụ:

```jsx
import React, { useState } from "react";

function Counter() {
  const [count, setCount] = useState(0); // count là state có thể thay đổi

  const increment = () => {
    setCount(count + 1); // Thay đổi state
  };

  return (
    <div>
      <p>Count: {count}</p>
      <button onClick={increment}>Increment</button>
    </div>
  );
}
```

- Data flow trong components (Luồng dữ liệu trong component):
  - React có luồng dữ liệu một chiều(unidirectional data flow), từ trên xuống dưới (parent to child). Props luôn truyền từ cha xuống con.
  - State chỉ thuộc về component mà chỉ nó được định nghĩa và có thể dược truyền xuống các compoennt con dưới dạng props

1. Có cách nào để truyền dữ liệu từ con lên cha được không?

- Điều này là có thể bằng cahcs truyền callback function từ cha xuống con, và con sẽ gọi lại hàm đó khi cần gửi dữ liệu ngược lên.

```jsx
function Parent() {
  const handleDataFromChild = (data) => {
    console.log("Dữ liệu từ con:", data);
  };

  return <Child onSendData={handleDataFromChild} />;
}
```

```jsx
function Child({ onSendData }) {
  return (
    <button onClick={() => onSendData("Hello từ con")}>
      Gửi dữ liệu lên cha
    </button>
  );
}
```

Tóm tắt:

- Luồng dữ liệu mặc định: Cha - Con qua props
- Dữ liệu từ con lên cha: Con gọi hàm được truyền từ cha xuống
- React giữ unidirectional data flow nhưng vẫn linh hoạt qua pattern "lifting state up";

#### **18. React Hooks là gì? Tại sao được tạo ra?**

React Hooks là các hàm đặc biệt cho phép ta "móc nối" vào các tính năng của React (như state và lifecycle methods) và các functional components.

- useState, useEffect, useContext:
  `useState`: Cho phép thêm state vào functional components
  `useEffect`: Cho phép thực hiện các side effects (tác dụng phụ) như fetch dữ liệu, subscribe sự kiện, thao tác DOM trực tiếp sau khi render.
  `useContext`: Cho phép component subscribe vào React context.

- Thay thế class components: Hooks được tạo ra để giải quyết một số vấn đề mà class components gặp phải, bao gồm:
  - Logic trùng lặp và khó tái sử dụng: Với class components, việc tái sử dụng logic stateful giữa các component rất khó khăn.
  - Classes gây nhầm lẫn cho người mới học: Khái niệm `this` trong Javascript có thể gây khó hiểu.
  - Components phức tạp khó hiểu và kiểm thử: Các lifecyle methods trong class components có thể khiến logic liên quan bị phân tán.
- Custom hooks: Bạn có thể tự tạo các custom hooks để tái sử dụng logic stateful phức tạp giữa các component, giúp code gọn gàng và dễ quản lý hơn.
- Ví dụ: useFetch, useLocalStorage.

3. **useEffect hook hoạt động như thế nào?**

   - Dependency array: Quy định khi nào `useEffect` chạy lại (chỉ khi dependencies thay đổi)
   - Cleanup functions: Hàm trả về từ `useEffect` để dọn dẹp (ví dụ: hủy timer subscription).
   - Component lifecycle equivalent: Tương đương `componentDidMount`,`componentDidUpdate`, `componentWillUnmount`.

```jsx
function App() {
  const [count, setCount] = React.useState(0);
  React.useEffect(() => {
    const timer = setInterval(() => console.log(count), 1000);
    return () => clearInterval(timer); // Cleanup
  }, [count]); // Dependency array
  return <button onClick={() => setCount(count + 1)}>{count}</button>;
}
```

4. **Context API vs Redux**

   - State management solutions
   - Khi nào sử dụng Context API
   - Redux pattern và middleware

5. **React component lifecycle**

   - Mount, Update, Unmount phases
   - Functional vs Class components
   - Hooks equivalents

6. **Controlled vs Uncontrolled components**

   - Form handling
   - Ref usage
   - Best practices

7. **React Router**

   - Client-side routing
   - Route parameters
   - Protected routes

8. **Higher-Order Components (HOC) vs Render Props**

   - Pattern comparison
   - Use cases
   - Modern alternatives with hooks

9. **Memoization trong React**

   - React.memo()
   - useMemo vs useCallback
   - Performance optimization

10. **Error Boundaries trong React**

    - Catching JavaScript errors
    - Fallback UI
    - Error reporting

11. **Keys trong React lists**

    - Tại sao cần keys
    - Best practices
    - Performance impact

12. **React.StrictMode là gì?**

    - Development mode benefits
    - Double rendering
    - Deprecated API detection

13. **Server-Side Rendering (SSR) vs Client-Side Rendering (CSR)**

    - SEO implications
    - Performance comparison
    - Next.js framework

14. **React performance optimization**
    - Code splitting
    - Lazy loading
    - Bundle optimization

### Tình huống React.js (15 câu)

46. **Optimize React app có loading time chậm (5s)**

    - Code splitting
    - Lazy loading components
    - Bundle analysis
    - Image optimization

47. **Xử lý form phức tạp với 50+ fields và validation**

    - Form libraries (Formik, React Hook Form)
    - Validation strategies
    - Performance optimization
    - User experience

48. **Implement infinite scrolling cho danh sách 10,000 items**

    - Virtual scrolling
    - Pagination strategies
    - Memory management
    - Loading states

49. **State management cho large-scale application**

    - Redux architecture
    - State normalization
    - Middleware usage
    - Testing strategies

50. **Implement real-time data updates trong React**

    - WebSocket integration
    - Socket.io client
    - State synchronization
    - Conflict resolution

51. **Debug React component re-render issues**

    - React DevTools Profiler
    - Unnecessary re-renders
    - Memoization strategies
    - Performance monitoring

52. **Implement responsive design cho mobile và desktop**

    - CSS-in-JS solutions
    - Media queries
    - Touch interactions
    - Performance considerations

53. **Xử lý error boundary cho production app**

    - Error catching strategies
    - Fallback UI design
    - Error reporting
    - Recovery mechanisms

54. **Implement complex routing với authentication**

    - Protected routes
    - Route guards
    - Dynamic routing
    - Permission-based navigation

55. **Optimize bundle size từ 5MB xuống 1MB**

    - Tree shaking
    - Code splitting
    - Library optimization
    - Asset optimization

56. **Implement multi-language support (i18n)**

    - Translation management
    - Dynamic language switching
    - Date/number formatting
    - RTL support

57. **Testing strategy cho React application**

    - Unit testing components
    - Integration testing
    - E2E testing
    - Test coverage

58. **Implement PWA features**

    - Service workers
    - Offline functionality
    - Push notifications
    - App manifest

59. **Migrate class components sang functional components**

    - Hook equivalents
    - Lifecycle methods
    - State management
    - Testing updates

60. **Implement complex data visualization**
    - Chart libraries integration
    - Performance with large datasets
    - Interactive features
    - Responsive charts

## TIPS CHUẨN BỊ PHỎNG VẤN

### Cách trả lời hiệu quả:

1. **Structured approach**: Problem → Solution → Implementation → Results
2. **Provide examples**: Concrete code examples hoặc real-world scenarios
3. **Explain trade-offs**: Ưu nhược điểm của mỗi approach
4. **Show debugging skills**: Cách bạn identify và solve problems
5. **Mention best practices**: Industry standards và patterns

### Những điều cần nhớ:

- Luôn hỏi clarifying questions trước khi trả lời
- Admit khi không biết và show willingness to learn
- Demonstrate problem-solving process
- Mention relevant tools và libraries
- Show understanding of performance implications
