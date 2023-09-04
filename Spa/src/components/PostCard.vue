<template>
  <div class="card mb-3 post-card">
    <router-link :to="{name: 'postByUrl', params: {postUrl: post?.url}}">
      <img class="rounded mx-auto d-block" :src="'https://kalbood.ir' + post?.imageUrl" :title="post?.title" :alt="post?.title">
      <div class="card-body">
        <h5 class="card-title pb-1">{{ post?.title }}</h5>
        <p class="card-text text-muted">{{ post?.summary }}</p>
      </div>
      <div class="card-footer">
        <small class="text-muted">تعداد بازدید: {{ post?.views }}</small>
      </div>
  </router-link>
  </div>
</template>

<script lang="ts">
import { defineComponent, PropType } from 'vue'
import { Post } from '@/types/def'

export default defineComponent({
  name: 'PostCard',
  props: {
    post: Object as PropType<Post>
  }
})

</script>
