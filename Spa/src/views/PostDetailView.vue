<template>
  <div class="container py-3">
    <div class="container py-3">
      <div v-if="failed">
        <p>بارگذاری با خطا مواجه شد.</p>
      </div>
      <div class="text-center" v-else-if="loading">
        <div class="spinner-border" role="status">
          <span class="visually-hidden">در حال بارگذاری ...</span>
        </div>
      </div>
      <article class="post" v-else>
        <div class="row">
          <div class="col-md-12" v-if="post?.imageUrl">
            <img class="rounded mx-auto d-block" :src="'https://kalbood.ir' + post?.imageUrl" :title="post?.title"
              :alt="post?.title">
          </div>
          <div class="col-md-12">
            <h2>{{ post?.title }}</h2>

            <div v-html="post?.text"></div>
          </div>
        </div>
      </article>

      <hr />

      <div class="row" v-if="post?.categories">
        <h3 class="block-title text-center py-3 mt-1">دسته‌بندی‌های مرتبط</h3>
        <div class="col-md-3" v-for="item of post?.categories" :key="item.id">
          <CategoryCard v-bind:category=item />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue'
import { PostDetail } from '@/types/def'
import CategoryCard from '@/components/CategoryCard.vue'
import useGetApi from '@/composables/useGetApi'

export default defineComponent({
  name: 'PostDetailView',
  props: { postUrl: String },
  components: { CategoryCard },
  setup (props) {
    const post = ref<PostDetail | null>(null)
    const loading = ref(true)
    const failed = ref(false)

    onMounted(async () => {
      const { result, isFailed } = await useGetApi<PostDetail>('posts/' + props.postUrl)
      post.value = result.value
      failed.value = isFailed.value
      loading.value = false
    })

    return { post, loading, failed }
  }
})
</script>

<style>
article.post img {
  max-height: 500px;
}

article.post h2 {
  text-align: center;
  line-height: 3rem;
}
</style>
