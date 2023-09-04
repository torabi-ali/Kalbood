<template>
  <div class="container">
    <div v-if="failed">
      <p>بارگذاری با خطا مواجه شد.</p>
    </div>
    <div class="text-center loading-overlay" v-else-if="loading">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">در حال بارگذاری ...</span>
      </div>
    </div>
    <div v-else>
      <h2 class="block-title text-center py3 mt-1">دسته‌بندی‌ها</h2>
      <div class="row pt-3" v-if="categories">
        <div class="col-md-4" v-for="item of categories.data" :key="item.id">
          <CategoryCard v-bind:category=item />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue'
import { Category, Paged } from '@/types/def'
import CategoryCard from '@/components/CategoryCard.vue'
import useGetApi from '@/composables/useGetApi'

export default defineComponent({
  name: 'CategoryListView',
  components: { CategoryCard },
  setup () {
    const categories = ref<Paged<Category> | null>(null)
    const loading = ref(true)
    const failed = ref(false)

    onMounted(async () => {
      const { result, isFailed } = await useGetApi<Paged<Category>>('categories')
      categories.value = result.value
      failed.value = isFailed.value
      loading.value = false
    })

    return { categories, loading, failed }
  }
})
</script>
