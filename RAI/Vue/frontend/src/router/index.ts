import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    {
      path: '/',
      name: 'home',
      component: HomeView,
    },
    {
      path: '/about',
      name: 'about',
      // route level code-splitting
      // this generates a separate chunk (About.[hash].js) for this route
      // which is lazy-loaded when the route is visited.
      component: () => import('../views/AboutView.vue'),
    },
    {
      path: '/login',
      name: 'Login',
      component: () => import('../views/LoginView.vue')
    },
    {
      path: '/saved',
      name: 'Saved',
      meta: { requiresAuth: true },
      component: () => import('../views/SavedView.vue')
    },
    {
      path: '/register',
      name: 'Register',
      component: () => import('../views/RegisterView.vue')
    }
  ],
})

router.beforeEach(async (to, from, next) => {
  if(to.matched.some(record => record.meta.requiresAuth)) {
    const token = localStorage.getItem('token')

    if(token) {
      return next()
    }

    return next('/login')
  }
  
  if (to.name === 'Login' && localStorage.getItem('token')) {
    next({ name: 'home' })
  } else if (to.name === 'Register' && localStorage.getItem('token')) {
    next({ name: 'home' })
  } else {
    next()
  }
});

export default router
