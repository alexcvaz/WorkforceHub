import { createRouter, createWebHistory } from 'vue-router'
import { useAuth } from '@/composables/useAuth'
import Login from '@/components/Login.vue'
import TimeTap from '@/views/TimeTap.vue'

const routes = [
  {
    path: '/login',
    name: 'Login',
    component: Login,
    meta: { requiresAuth: false }
  },
  {
    path: '/punch',
    name: 'TimeTap',
    component: TimeTap,
    meta: { requiresAuth: true }
  },
  {
    path: '/',
    redirect: '/punch'
  }
]

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes
})

// Guard de autenticação
router.beforeEach((to, from, next) => {
  const { isAuthenticated } = useAuth()

  if (to.meta.requiresAuth && !isAuthenticated.value) {
    next('/login')
  } else if (to.path === '/login' && isAuthenticated.value) {
    next('/punch')
  } else {
    next()
  }
})

export default router
