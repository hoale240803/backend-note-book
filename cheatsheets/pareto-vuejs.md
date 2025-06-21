# 1. File project structure (80/20 Focus)

```
/src
  ├── App.vue              # Root component
  ├── main.js              # Entry point
  ├── components/          # Reusable UI components
  ├── views/               # Pages (used with routing)
  ├── router/              # Vue Router setup
  ├── store/               # Vuex/Pinia store (if used)
  └── assets/              # Images, styles, icons

```

✅ Key Tasks:

Know App.vue wraps everything

Put routes in views/, reuse UI blocks in components/

Use main.js to configure Vue app, router, store

# 2. Routing (Vue Router)

Define routes with paths and components

Use <router-link> for navigation

Use <router-view> to render active route

Handle dynamic routes (/user/:id)

```
// router/index.js
import { createRouter, createWebHistory } from 'vue-router';
import Home from '../views/Home.vue';
import User from '../views/User.vue';

const routes = [
  { path: '/', component: Home },
  { path: '/user/:id', component: User },
];

export default createRouter({
  history: createWebHistory(),
  routes,
});

```

```
<!-- App.vue -->
<router-link to="/">Home</router-link>
<router-view />

```

Navigate with <router-link> or this.$router.push()

Extract route params with this.$route.params.id

# 3. Forms & Input Handling (with Validation)

Use v-model for 2-way data binding

Use basic required, @submit.prevent

Simple manual validation

```
<template>
  <form @submit.prevent="submit">
    <input v-model="email" type="email" required />
    <div v-if="error">{{ error }}</div>
    <button type="submit">Submit</button>
  </form>
</template>

<script>
export default {
  data() {
    return { email: '', error: '' }
  },
  methods: {
    submit() {
      if (!this.email.includes('@')) {
        this.error = 'Invalid email';
      } else {
        this.error = '';
        // submit form
      }
    }
  }
}
</script>

```

# 4. State Management (Pinia - Vue 3 preferred)

Learn Just This:
Create a store with state, actions

Use in components with useStore()

```
// store/counter.js
import { defineStore } from 'pinia';

export const useCounter = defineStore('counter', {
  state: () => ({ count: 0 }),
  actions: {
    increment() {
      this.count++;
    }
  }
});

```

```
<script setup>
import { useCounter } from '../store/counter';
const counter = useCounter();
</script>

<template>
  <p>{{ counter.count }}</p>
  <button @click="counter.increment">Add</button>
</template>

```

Use Pinia instead of Vuex (simpler)

Share global state across components

# 5. HTTP Requests (API Calls)

Use Axios or Fetch

Call APIs in mounted() or onMounted()

Handle loading + error states

```
<script>
import axios from 'axios';

export default {
  data() {
    return { users: [], loading: true, error: '' }
  },
  async mounted() {
    try {
      const res = await axios.get('https://api.example.com/users');
      this.users = res.data;
    } catch (err) {
      this.error = 'Failed to load';
    } finally {
      this.loading = false;
    }
  }
}
</script>

```

# 6. Basic UI rendering

Learn Just This:
v-for, v-if, v-bind, v-on

```
<ul>
  <li v-for="user in users" :key="user.id">
    {{ user.name }}
  </li>
</ul>

<button @click="loadMore">Load More</button>

```

✅ Core Directives to Master First:

Directive Purpose
v-for Loops through lists
v-if/v-else Conditional display
v-model Two-way data binding
v-bind Bind attribute dynamically
v-on or @ Listen to events

📌 Summary: Fastest Track with Vue (Apply Pareto)
| Task | What to Learn First |
| ------------------ | ------------------------------------------------ |
| Project Structure | `App.vue`, `main.js`, components vs views |
| Routing | `<router-link>`, `<router-view>`, dynamic params |
| Forms & Validation | `v-model`, manual check, `@submit.prevent` |
| State Management | Pinia store: `state`, `actions`, `useStore()` |
| API Calls | Axios, `mounted()`, try/catch/finally |
| UI Rendering | `v-for`, `v-if`, `v-bind`, `v-on` |
