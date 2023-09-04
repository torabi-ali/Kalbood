<template>
  <div class="container py-3">
    <div v-if="failed">
      <p>بارگذاری با خطا مواجه شد.</p>
    </div>
    <div class="text-center" v-else-if="loading">
      <div class="spinner-border" role="status">
        <span class="visually-hidden">در حال بارگذاری ...</span>
      </div>
    </div>
    <article class="category" v-else>
      <div class="row">
        <div class="col-md-12" v-if="category?.imageUrl">
          <img class="rounded mx-auto d-block" :src="'https://kalbood.ir' + category?.imageUrl" :title="category?.title"
            :alt="category?.title">
        </div>
        <div class="col-md-12">
          <h2>{{ category?.title }}</h2>

          <div v-html="category?.text"></div>
        </div>
      </div>
    </article>

    <hr />

    <div class="row" v-if="category?.posts">
      <h3 class="block-title text-center py-3 mt-1">پست‌های مرتبط</h3>
      <div class="col-md-4" v-for="item of category?.posts" :key="item.id">
        <PostCard v-bind:post=item />
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, onMounted, ref } from 'vue'
import { CategoryDetail } from '@/types/def'
import PostCard from '@/components/PostCard.vue'
import useGetApi from '@/composables/useGetApi'

export default defineComponent({
  name: 'CategoryDetailView',
  props: { categoryUrl: String },
  components: { PostCard },
  setup (props) {
    const category = ref<CategoryDetail | null>(null)
    const loading = ref(true)
    const failed = ref(false)

    onMounted(async () => {
      const { result, isFailed } = await useGetApi<CategoryDetail>('categories/' + props.categoryUrl)
      category.value = result.value
      failed.value = isFailed.value
      loading.value = false
    })

    return { category, loading, failed }
  }
})
</script>

<style>
article.category img {
  max-height: 500px;
}

article.category h2 {
  text-align: center;
  line-height: 3rem;
}
</style>
