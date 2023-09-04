import { createRouter, createWebHistory, RouteRecordRaw } from 'vue-router'
import HomeView from '@/views/HomeView.vue'

const routes: Array<RouteRecordRaw> = [
  {
    path: '/',
    name: 'home',
    component: HomeView
  },
  {
    path: '/categories',
    name: 'categoryList',
    component: () => import('@/views/CategoryListView.vue')
  },
  {
    path: '/categories/:categoryUrl',
    name: 'categoryByUrl',
    component: () => import('@/views/CategoryDetailView.vue'),
    props: true
  },
  {
    path: '/posts',
    name: 'postList',
    component: () => import('@/views/PostListView.vue')
  },
  {
    path: '/posts/:postUrl',
    name: 'postByUrl',
    component: () => import('@/views/PostDetailView.vue'),
    props: true
  },
  {
    path: '/:pathMatch(.*)*',
    name: 'notFound',
    component: () => import('@/views/PageNotFoundView.vue')
  }
]

const router = createRouter({
  history: createWebHistory(process.env.BASE_URL),
  routes
})

export default router
