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

#### **19. useEffect hook hoạt động như thế nào?**

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

#### **20. Context API vs Redux**

- Cả Context API và Redux đều là các giải pháp quảny lý trạng thái (state management) trong ứng dụng React, Nhưng chúng được thiết kế cho các trường hợp sử dụng khác nhau.

- State management solutions (Giải pháp quản lý trạng thái):
  - Cả hai đều giúp giải quyết vấn đề "prop drilling" (truyền props qua nhiều cấp component không liên quan) bằng cách cung cấp một cách để chia sẻ dữ liệu chung cho nhiều component.
- Khi nào sử dụng Context API: - Dành cho dữ liệu "ít thay đổi" và "ít khi cập nhật": Context API lý tưởng cho việc chia sẻ dữ liệu như theme(sáng/tối), thông tin người dùng đang đăng nhập, cài đặt ngôn ngữ. - Redux pattern và middleware: Khi nhu cầu quản lý trạng thái không quá phức tạp và không đòi hỏi các tính năng nâng cao như middleware, time-travel debugging. - Ít boilerplate hơn: Context API có cú pháp đơn giản và ít cần cấu hình hơn so với Redux
  - Ví dụ:

```jsx
// Tạo Context

const ThemeContext = React.createContext("light");

// Cung cấp giá trị
function App() {
  return (
    <ThemeContext.Provider value="dark">
      <Toolbar />
    </ThemeContext.Provider>
  );
}

// Sử dụng giá trị
function Toolbar() {
  const theme = useContext(ThemeContext);
  return (
    <button
      style={{
        background: theme === "dark" ? "black" : "white",
        color: theme === "dark" ? "white" : "black",
      }}
    >
      Current theme: {theme}
    </button>
  );
}
```

- Redux pattern và middleware:
  - Dành cho ứng dụng lớn, phức tạp: Redux phù hợp với các ứng dụng có trạng thái phức tạp, thường xuyên thay đổi và cần quản lý chặt chẽ.
  - Centralized store (Kho lưu trữ tập trung): Redux sử dụng một "store" để lưu trữ toàn bộ trạng thái của ứng dụng.
  - Centralized store changes(Thay đổi trạng thái có thể đự đoán): Mọi thay đổi trạng thái đều thông qua "actions" và "reducers" thuần túy, giúp dễ dàng theo dõi và debug.
  - Middleware: Redux có hệ thống middleware mạnh mẽ cho phép bạn thêm logic vào quá trình gửi action(ví dụ: `redux-thunk` cho async operations, `redux-logger` để ghi nhật ký các thay đổi trạng thái).
  - Boilerplate nhiều hơn: Redux yêu cầu nhiều code và cấu hình hơn so với Context API.

#### **21. React component lifecycle**

React component lifecycle là các giai đoạn khác nhau trong quá trình tồn tại của một component, từ khi nó được tạo ra, cập nhật, cho đến khi bị xóa khỏi DOM.

- Mount, Update, Unmount phases(các giai đoạn gắn kết, cập nhật, gỡ bõ):
  - Mounting(Gắn kết): Component được tạo và chèn vào DOM.
    - Class components: constructor, static getDerivedStateFromProps, render, componentDidMount.
    - Functional components: `useEffect` với mảng phụ thuộc rỗng (`[]`).
  - Updating (Cập Nhật): Component được render lại do thay đổi props hoặc state.
    - Class components: `static getDerivedStateFromProps`, `shouldComponentUpdate`, `render`,`getSnapshotBeforeUpdate`, `componentDidUpdate`.
    - Functional components: `useEffect` với các phụ thuộc thay đổi.
  - Unmouting(Gỡ bõ): Component bị xóa khỏi DOM.
    - Class components: componentWillUnmount.
    - Funtional components: Hàm dọn dẹp (cleanup function) được trả về từ useEffect.
- Functional vs Class components:
  - Trước khi có hooks, chỉ class components mới có thể có state và lifecycle methods.
  - Functional components trước đây được gọi là "stateless functional components" vì chúng chỉ nhận props và render UI
- Hooks equivalents:
  - useState: Thay thế cho this.state và this.setState.
  - useEffect: Thay thế cho componentDidMount, componentDidUpdate, componentWillUnmount.
  - useContext: Thay thế cho Context.Consumer và static contextType.
  - useReducer: Một lựa chọn thay thế cho `useState` cho các trường hợp state phực tạp hơn, tương tự như redux.

#### **22. Controlled vs Uncontrolled components**

Cách React quản lý dữ liệu trong các phần tử Form(input, textarea, select) dẫn đến 2 loại component: Controlled và Uncontrolled.

- Form handling(Xử lý form):
  - Controlled Components:
    - Các phần tử form mà giá trị của chúng được kiểm soát hoàn toàn bởi state của React.
    - Mỗi khi giá tị input thay đổi, một event handler (ví dụ `onChange`) sẽ cập nhật state, và giá trị hiển thị của input luôn phản ánh giá trị trong state.
    - Đây là cách được khuyến nghị trong hầu hết các trường hợp vì nó cho phép React quảny toàn bộ dữ liệu form, giúp dễ dàng validationm, reset form, và xử lý logic phức tạp.
    - Ví dụ:

```jsx
import React, { useState } from "react";

function ControlledForm() {
  const [name, setName] = useState("");

  const handleChange = (event) => {
    setName(event.target.value);
  };

  const handleSubmit = (event) => {
    event.preventDefault();
    alert(`Name submitted: ${name}`);
  };

  return (
    <form onSubmit={handleSubmit}>
      <label>
        Name:
        <input type="text" value={name} onChange={handleChange} />
      </label>
      <button type="submit">Submit</button>
    </form>
  );
}
```

- Uncontroled Components:

  - Các phần tử form mà dữ liệu của chúng được quản lý bởi DOM thay vì bởi React state.
  - Bạn sử dụng `refs` để truy cập trực tiếp vào DOM element và lấy giá trị của nó khi cần (ví dụ: khi submit form).
  - Ít được sử dụng hơn Controlled Components vì mất đi một số lợi ích của React trong việc quản lý dữ liệu.

  Ví dụ:

```jsx
import React, { useRef } from "react";

function UncontrolledForm() {
  const nameInputRef = useRef(null);

  const handleSubmit = (event) => {
    event.preventDefault();
    alert(`Name submitted: ${nameInputRef.current.value}`);
  };

  return (
    <form onSubmit={handleSubmit}>
      <label>
        Name:
        <input type="text" ref={nameInputRef} />
      </label>
      <button type="submit">Submit</button>
    </form>
  );
}
```

- Ref usage(Sử dụng ref):
  - `useRef` hook là cách phổ biến để tạo và sử dụng refs trong functional components.
  - Refs được dùng để truy cập trực tiếp vào các DOM nodes hoặc các instance của component con.
- Best practices:
  - Thường xuyên ưu tiên sử dụng Controlled Components vì chúng cung cấp một luồng dữ liệu rõ ràng và dễ dự đoán hơn.
  - Sử dụng Uncontrolled Components khi bạn cần tích hợp với thư viện DOM không phải của React, hoặc khi hiệu suất là yếu tố quan trọng và bạn không cần React render lại component sau lỗi lần gõ phím.

#### **23. React Router**

React Router là một thư viện phổ biến để quản lý việc định tuyến (routing) trong các ứng dụng React một trang (Singple Page Application - SPAs).

- Client-side routing (Định tuyến phía client):
  - React Router cho phép bạn tạo ra các URL khác nhau mà không cần tải lại toàn bộ trang từ server. Khi người dùng nhấp vào một liên kết, React Router sẽ thay đổi URL trên trình duyệt và reder component tương ứng.
  - Ví dụ:

```jsx
import { BrowserRouter as Router, Routes, Route, Link } from "react-router-dom";

function App() {
  return (
    <Router>
      <nav>
        <Link to="/">Home</Link> | <Link to="/about">About</Link>
      </nav>
      <Routes>
        <Route path="/" element={<Home />} />
        <Route path="/about" element={<About />} />
      </Routes>
    </Router>
  );
}

function Home() {
  return <h2>Home Page</h2>;
}
function About() {
  return <h2>About Page</h2>;
}
```

- Route parameters (Tham số tuyến đường):
  - Cho phép bạn truyền dữ liệu động thông qua URL. Ví dụ: `/users/:id` sẽ cho phép bạn lấy `id` của người dùng từ url.
  - Sử dụng `useParams` để truy cập các tham số này.
  - Ví dụ:

```jsx
import { useParams } from "react-router-dom";

function UserProfile() {
  const { id } = useParams(); // Lấy id từ URL
  return <h2>User ID: {id}</h2>;
}

// Trong App.js:
// <Route path="/users/:id" element={<UserProfile />} />
```

- Protected routes (Các tuyến đường được bảo vệ):
  - Cho phép bạn hạn chế truy cập vào một số tuyến đường nhất định dựa trên những điều kiện (ví dụ: người dùng đã đăng nhập chưa)
  - Thường triền khai bằng cách tạo một component wrapper kiểm tra điều kiện và điều hướng người dùng đến trang đăng nhập nếu họ không đủ quyền.
  - Ví dụ(đơn giản):

```jsx
import { Navigate } from 'react-router-dom';

function PrivateRoute({ children }) {
  const isAuthenticated = /* logic kiểm tra người dùng đã đăng nhập */;
  return isAuthenticated ? children : <Navigate to="/login" />;
}

// Trong App.js:
// <Route path="/dashboard" element={<PrivateRoute><Dashboard /></PrivateRoute>} />
```

#### **24. Higher-Order Components (HOC) vs Render Props**

HOC và Render props là hai kỹ thuật nâng cao trong React để chia sẽ logic và tái sử dụng code giữa các component.

- Pattern comparison (So sánh các mẫu):
  - Higher-Order Components (HOC):
    - Là một hàm nhận vào một component và trả về một component mới được "nâng cao"(enhanced) với các props hoặc logic bổ sung.
    - Tương tự như Higher-Order Functions trong Javascript.
    - Ví dụ: `withRouter` trong React Router v5, `connect` trong Redux.
    - Cú pháp: `const EnhancedComponent = withData(MyComponent)`
  - Render Props:
    - Là một kĩ thuật truyền một hàm thông qua một prop. Hàm này chịu trách nhiệm render phân UI mà nó cần chia sẽ logic.
    - Prop chứa hàm render thường được gọi là `render` (hoặc bất kỳ tên nào khác).
    - Cú pháp `<DataSource render={data => <MyComponent data={data} />} />`
- Use cases (Trường hợp sử dụng):

  - HOC:
    - Tái sử dụng logic không liên quan đến UI (ví dụ: quản lý authentication, logging, data, fetching)
    - Khi bạn muốn thêm hành vi cho một component mà không cần thay đổi các cấu trúc của nó.
    - Tái sử dụng các phương thức vòng đời (lifecycle methods) của class components.
  - Render Props:
    - Khi bạn muốn chia sẻ cảe logic và UI rendering linh hoạt hơn.
    - Tạo ra các compoennt có thể tùy chỉnh hiển thị thông qua mộ hàm
    - Ví dụ điển hình: `Context.Consumer` hoặc thư viện `Formik`

- Modern alternatives with hooks (Các lựa chọn thay thế hiện đại với hooks):
  - Với sự ra đời của React Hooks, cả HOC và Reder Props đều ít được sử dụng trức tiếp hơn để chia sẻ logic stateful.
  - Custom Hooks đã trở thành phương pháp ưa thích để tái sử dụng logic không liên quan đến UI. Chúng đơn giản hơn, dễ đọc hơn và không gay ra các vấn đề như "wrapper hell" (quá nhiều HOC lồng nhau) hoặc xung đột props.
  - Ví dụ:

```jsx
// HOC
function withLogger(WrappedComponent) {
  return function Logger(props) {
    useEffect(() => {
      console.log(
        `${WrappedComponent.displayName || WrappedComponent.name} mounted`
      );
    }, []);
    return <WrappedComponent {...props} />;
  };
}
const MyComponentWithLogger = withLogger(MyComponent);

// Render Props
function DataFetcher({ render }) {
  const [data, setData] = useState(null);
  useEffect(() => {
    // fetch data
    setData("Fetched data");
  }, []);
  return render(data);
}
// <DataFetcher render={(data) => <MyComponent data={data} />} />

// Custom Hook (Modern alternative)
function useLogger(componentName) {
  useEffect(() => {
    console.log(`${componentName} mounted`);
  }, [componentName]);
}

function MyComponent() {
  useLogger("MyComponent");
  return <div>Hello</div>;
}
```

#### **25. Memoization trong React**

Memoization là một kỹ thâutj tối ưu hiệu suất trong React bằng cách ghi nhớ kết quả của một hàm và trả về kết quả đã ghi nhớ đó nếu các đầu vào của hàm không thay đổi.

- React.memo():
  - Là một Higher-Order Component (HOC) dùng để tối ưu hiệu xuất cho functional components.
  - Nó sẽ ghi nhớ (memoize) kết quả render của component và chỉ re-render component đó nếu các props của nó thay đổi.
  - Giúp tránh các lần render không cần thiết cho các component con khi props của chúng không thay đổi, ngày cả khi componetn cha của chúng re-render.
  - Ví dụ:

```jsx
import React from "react";

const MyPureComponent = React.memo(function MyPureComponent(props) {
  // Component này chỉ render lại nếu props.value thay đổi
  console.log("MyPureComponent rendered");
  return <div>{props.value}</div>;
});

function Parent() {
  const [count, setCount] = React.useState(0);
  const [text, setText] = React.useState("Hello");

  return (
    <div>
      <button onClick={() => setCount(count + 1)}>Increment Count</button>
      <input
        type="text"
        value={text}
        onChange={(e) => setText(e.target.value)}
      />
      <MyPureComponent value={text} />{" "}
      {/* MyPureComponent chỉ render khi text thay đổi */}
    </div>
  );
}
```

- useMemo vs useCallback:
- `useMemo`:
  - Dùng để meomoize một giá trị được tính toán. Nó chỉ tính lại giá trị đó khi các dependencies (Các phần tử trong mảng phụ thuộc) thay đổi.
  - Hữu ích khi bạn có các phép tính tốn kém hoặc khi bạn cần truyền một đối tượng/mảng ổn định qua props để tránh re-render không cần thiết ở compoennt con đã được `React.memo` bọc.

```jsx
const memoizedValue = useMemo(() => computeExpensiveValue(a, b), [a, b]);
```

- `useCallback`
  - Dùng để memoi một hàm callback. Nó chỉ tạo lại hàm đó khi các dependencies thay đổi.
  - Quan trọng khi truyền các hàm xuống component con được bọc bởi `React.memo` để tránh việc component con re-render không cần thiết do hàm callback được tạo lại ở mỗi lần render của component cha.

```jsx
const handleClick = useCallback(() => {
  // do something
}, [dependency1, dependency2]);
```

- Performance optimization (Tối ưu hiệu suất):
  - Memoization là một công cụ mạnh mẽ để giảm số lần re-render không cần thiết, đặc biệt trong các ứng dụng lớn với nheièu component và dữ liệu phức tạp.
  - Tuy nhiên, không phải lúc nao fcũng cần ap dụng meomization. Việc sử dụng quá mức có thể gây ra overhead do chính quá trình ghi nhớ và so sánh. Chỉ sử dụng khi bạn xác định được điểm nghẽn hiệu suất.

#### **26. Error Boundaries trong React**

Error Boundaries là các React Componets đặc biệt bắt lỗi Javacscript trong cây con của chúng, ghi lại các lỗi đó và hiển thị mộit UI dự phòng thay vì làm hỏng toàn bộ ứng dung.

- Catching Javascript errors (Bắt lỗi trong Javascript):
  - Error Boundaries bắt lỗi xảy ra trong các giai đoạn render, trong lifecycle methods, và trong các constructors của toàn bộ cây con bên dưới chúng
  - Chúng không bắt được các lỗi sau:
    - Lỗi trong event handlers (ví dụ: `onClick`, `onChange`)
    - Lỗi trong code asynchronous (ví dụ: `setTimeout`, `fetch`).
    - Lỗi trong chính Error Boundary.
    - Lỗi từ Server-side Rendering.
- Fallback UI (Giao diện người dùng dự phòng):
  - Khi một lỗi được phát hiện, Error Boundary sẽ render một giao diện người dùng "dự phòng"
- Error reporting

#### **27. Keys trong React lists**

Keys là một thuộc tính đặc biệt mà bạn cần thêm vao các phân tử khi render một danh sách các component trong React.

- Tại sao cần keys:
  - React sử dụng keys để xác định duy nhất từng phần tử trong một danh sách và theo dõi sự thay đổi của chúng.
  - Khi một danh sách thay đổi (ví dụ: thêm, xóa, sắp xếp lại phần tử), React sử dụng keys để so sánh các phần tử cũ và mới một cách hiệu quả trong quá trình reconciliation (đối chiếu Virtual DOM).
  - Nếu không có keys (hoặc keys không duy nhất/ổn định), React có thể gặp vấn đề khi cập nhật DOM, dẫn đến bị lỗi hiển thị, vấn đề về hiệu suất, hoặc mất trạng thái của các component con.
  - Ví dụ: Nếu không có keys, khi bạn thêm một item vào đầu danh sách, React có thể chỉ cập nhật nội dung của các item hiện có thay vì tạo mới/ sắp xếp lại, dẫn đến sai sót.
- Best practices:
  - Keys phải là duy nhất trong danh sách anh chị em (siblings): key chỉ cần duy nhất trong cùng một cấp độ của danh sách. Bạn có thể sự dụng cùng một key ở các danh sách khác nhau.
  - Keys nên là một chuỗi hoặc số:
  - Sử dụng ID ổn đinh: Cách tốt nhất là sử dụng một ID duy nhất và ổn định cho mỗi item từ dữ liệu của bạn (ví dụ: ID từ database).
  - Tránh sử dụng index làm key: Việc sử dụng index của mảng làm key (`index` trong `map`) chỉ được chấp nhận trong các trường hợp rất cụ thể:
    - Danh sách không thay đổi (không thêm, xóa, sắp xếp lại item).
    - Danh sách không được lọc hoặc sắp xếp lại.
    - Không có các item trong danh sách có ID riêng của chúng.
    - Ví dụ:
  ```jsx
  // Tốt: Sử dụng ID duy nhất từ dữ liệu
  const todos = [
    { id: 1, text: "Learn React" },
    { id: 2, text: "Build app" },
  ];
  <ul>
    {todos.map((todo) => (
      <li key={todo.id}>{todo.text}</li>
    ))}
  </ul>;
  ```

// Không tốt: Sử dụng index làm key (nếu danh sách có thể thay đổi)
// Khi thêm/xóa/sắp xếp, có thể gây ra lỗi hoặc vấn đề hiệu suất

<ul>
  {todos.map((todo, index) => (
    <li key={index}>{todo.text}</li>
  ))}
</ul>
  ```
- Performance impact (Tác động hiệu suất):
  - Keys chính xác giúp React tối ưu hóa quá trình cập nhật DOM, giảm thiểu số lần thao tác DOM và cải thiện hiệu suất tổng thể.
  - Keys không chính xác hoặc không ổn định có thể làm giảm hiệu suất, vì React có thể phải re-render toàn bộ danh sách thay vì chỉ cập nhật những phần tử cần thiết.

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

```

```
