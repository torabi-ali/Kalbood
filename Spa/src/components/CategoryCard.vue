<template>
  <div class="card mb-3 category-card">
    <router-link :to="{name: 'categoryByUrl', params: {categoryUrl: category?.url}}">
      <img class="rounded mx-auto d-block" :src="'https://kalbood.ir' + category?.imageUrl" :title="category?.title" :alt="category?.title">
      <div class="card-body">
        <h5 class="card-title pb-1">{{ category?.title }}</h5>
        <p class="card-text text-muted">{{ category?.summary }}</p>
      </div>
      <div class="card-footer">
        <small class="text-muted">تعداد پست: {{ category?.postCount }}</small>
      </div>
  </router-link>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue'
import { Category } from '@/types/def'

export default defineComponent({
  name: 'CategoryCard',
  props: {
    category: Object as PropType<Category>
  }
})
</script>
