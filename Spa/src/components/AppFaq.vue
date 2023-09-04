<template>
  <section class="py-3">
    <div class="container">
      <h3 class="block-title text-center py-3">سؤالات متداول</h3>

      <article v-for="(item, index) of faqs" v-bind:key="item.id" itemprop="mainEntity" itemtype="https://schema.org/Question">
        <div class="card mb-3 faq-card">
          <a type="button" data-bs-toggle="collapse" :data-bs-target="`#faq${index}`" aria-expanded="false" :aria-controls="`faq${index}`">
          <div class="card-header">
              <p class="card-text" itemprop="name">
                <i class="fa-solid fa-plus"></i>
                {{ item.question }}
              </p>
            </div>
          </a>
          <div class="card-body multi-collapse" :class="index > 0 ? 'collapse' : ''" :id="`faq${index}`" itemprop="acceptedAnswer" itemtype="https://schema.org/Answer">
            <p class="card-text" itemprop="text">{{ item.answer }}</p>
          </div>
        </div>
      </article>
    </div>
  </section>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue'
import useGetApi from '@/composables/useGetApi'

interface Faq {
  id: number,
  url: string,
  question: string,
  answer: string,
  displayOrder: number
}

export default defineComponent({
  name: 'AppNavbar',
  setup () {
    const faqs = ref<Faq[] | null>(null)
    const loading = ref(true)
    const failed = ref(false)

    onMounted(async () => {
      let currentLocation = window.location.pathname
      currentLocation = currentLocation.slice(-1) === '/' ? currentLocation : currentLocation + '/'

      const { result, isFailed } = await useGetApi<Faq[]>('faqs?url=' + currentLocation)
      faqs.value = result.value
      failed.value = isFailed.value
      loading.value = false
    })

    return { faqs, loading, failed }
  }
})
</script>

<style>
article .faq-card a {
  color: var(--dark-color);
  text-decoration: none;
}

article .faq-card .card-header .card-text {
  font-weight: 600;
}

article .faq-card .card-header .card-text i {
  font-weight: inherit;
}
</style>
