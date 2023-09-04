<template>
  <section class="py-3">
    <div class="container">
      <h2 class="block-title text-center py-3">دید کالبود</h2>
      <div class="pt-3 pb-5">
        <table class="table table-kalbood table-borderless">
          <tr>
            <td>انسان</td>
            <td><i class="fa fa-arrow-left"></i></td>
            <td>انسان فاعلی دارای فکر و انتخاب می‌باشد و از این جهت اثر گذار بر گستره خلقت است.</td>
          </tr>
          <tr>
            <td>جهان هستی</td>
            <td><i class="fa fa-arrow-left"></i></td>
            <td>جهان هستی بستر و اتمسفری برای استفاده، سکنی گزیدن و زندگی این فاعل است.</td>
          </tr>
          <tr>
            <td>معماری</td>
            <td><i class="fa fa-arrow-left"></i></td>
            <td>معماری علمی بین فاعل و بستر می‌باشد و از عوامل کلیدی اتصال این دو به یکدیگر است.</td>
          </tr>
          <tr>
            <td>نتیجه</td>
            <td><i class="fa fa-arrow-left"></i></td>
            <td>عادل رابطه فاعل و بستر - انسان و طبیعت - سالم‌ترین حالت زندگی است و معماری می‌تواند در ایجاد این تعادل اثر
              گذار باشد.</td>
          </tr>
        </table>
      </div>
    </div>
  </section>

  <section class="bg-secondary py-3">
    <div class="container">
      <h2 class="block-title text-center py-3">دسته‌بندی‌های محبوب</h2>
      <div class="row pt-3" v-if="homePage">
        <div class="col-md-4" v-for="item of homePage.pinnedCategories" :key="item.id">
          <CategoryCard v-bind:category=item />
        </div>
      </div>
    </div>
  </section>

  <section class="py-3">
    <div class="container">
      <h2 class="block-title text-center py-3">پست‌های محبوب</h2>
      <div class="row pt-3" v-if="homePage">
        <div class="col-md-4" v-for="item of homePage.pinnedPosts" :key="item.id">
          <PostCard v-bind:post=item />
        </div>
      </div>
    </div>
  </section>

  <section class="bg-secondary py-3">
    <div class="container">
      <h2 class="block-title text-center py-3">جدیدترین پست‌ها</h2>
      <div class="row pt-3" v-if="homePage">
        <div class="col-md-4" v-for="item of homePage.newPosts" :key="item.id">
          <PostCard v-bind:post=item />
        </div>
      </div>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue'
import { Post, Category } from '@/types/def'
import PostCard from '@/components/PostCard.vue'
import CategoryCard from '@/components/CategoryCard.vue'
import useGetApi from '@/composables/useGetApi'

interface HomePage {
  pinnedCategories: Category[],
  pinnedPosts: Post[],
  newPosts: Post[],
}

export default defineComponent({
  name: 'HomeView',
  components: { PostCard, CategoryCard },
  setup () {
    const homePage = ref<HomePage | null>(null)
    const loading = ref(true)
    const failed = ref(false)

    onMounted(async () => {
      const { result, isFailed } = await useGetApi<HomePage>('home')
      homePage.value = result.value
      failed.value = isFailed.value
      loading.value = false
    })

    return { homePage, loading, failed }
  }
})
</script>

<style>
h1,
h2,
h3 {
  line-height: 3rem !important;
}

table.table.table-kalbood {
    margin: auto;
    width: 75% !important;
    line-height: 2rem;
}

table.table.table-kalbood tr td:nth-child(1) {
    font-weight: 600;
}

table.table.table-kalbood tr td:nth-child(2) {
    font-weight: 600;
    letter-spacing: 1rem;
}
</style>
