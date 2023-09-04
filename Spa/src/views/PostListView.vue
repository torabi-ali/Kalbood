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
      <h2 class="block-title text-center py-3 mt-1">پست‌ها</h2>
      <div class="row pt-3" v-if="posts">
        <div class="col-md-4" v-for="item of posts.data" :key="item.id">
          <PostCard v-bind:post=item />
        </div>
      </div>
    </div>
  </div>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue'
import { Post, Paged } from '@/types/def'
import PostCard from '@/components/PostCard.vue'
import useGetApi from '@/composables/useGetApi'

export default defineComponent({
  name: 'PostListView',
  components: { PostCard },
  setup () {
    const posts = ref<Paged<Post> | null>(null)
    const loading = ref(true)
    const failed = ref(false)

    onMounted(async () => {
      const { result, isFailed } = await useGetApi<Paged<Post>>('posts')
      posts.value = result.value
      failed.value = isFailed.value
      loading.value = false
    })

    return { posts, loading, failed }
  }
})
</script>
