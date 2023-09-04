<template>
  <div class="container py-3">
    <p>صفحه شما یافت نشد :(</p>

    <hr />

    <div class="row" v-if="posts">
      <h3 class="block-title text-center py-3 mt-1">دسته‌بندی‌های مرتبط</h3>
      <div class="col-md-3" v-for="item of posts.data" :key="item.id">
        <PostCard v-bind:post=item />
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
  name: 'PageNotFoundView',
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
