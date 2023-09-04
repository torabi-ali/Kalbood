<template>
  <nav class="navbar navbar-expand-lg navbar-light border-bottom">
    <div class="container-fluid">
      <a class="navbar-brand" href="/">
        <img class="rounded mx-auto d-block" src="@/assets/image/kalbood-logo-64x64.png" title="استودیو کالبود" alt="استودیو کالبود">
      </a>
      <button class="navbar-toggler" type="button" data-bs-toggle="collapse" data-bs-target="#navbarNav"
        aria-controls="navbarNav" aria-expanded="false" aria-label="Toggle navigation">
        <span class="navbar-toggler-icon"></span>
      </button>
      <div class="collapse navbar-collapse" id="navbarNav">
        <ul class="navbar-nav" v-if="menus && menus.length">
          <span v-for="item of menus" :key="item.id">
            <li class="nav-item dropdown" v-if="item.subMenus && item.subMenus.length">
              <a class="nav-link dropdown-toggle" href="#" id="navbarDropdown" role="button" data-bs-toggle="dropdown"
                aria-expanded="false">
                {{ item.title }}
              </a>
              <ul class="dropdown-menu" aria-labelledby="navbarDropdown">
                <li v-for="subMenu of item.subMenus" :key="subMenu.id">
                  <a class="dropdown-item" aria-current="page" :href="subMenu.url">{{ subMenu.title }}</a>
                </li>
              </ul>
            </li>
            <li class="nav-item" v-else>
              <a class="nav-link" aria-current="page" :href="item.url">{{ item.title }}</a>
            </li>
          </span>
        </ul>
      </div>
    </div>
  </nav>
</template>

<script lang="ts">
import { defineComponent, ref, onMounted } from 'vue'
import useGetApi from '@/composables/useGetApi'

interface Menu {
  id: number,
  title: string,
  url: string,
  displayOrder: number,
  parentId: number,
  subMenus: Menu[]
}

export default defineComponent({
  name: 'AppNavbar',
  setup () {
    const menus = ref<Menu[] | null>(null)
    const loading = ref(true)
    const failed = ref(false)

    onMounted(async () => {
      const { result, isFailed } = await useGetApi<Menu[]>('menus')
      menus.value = result.value
      failed.value = isFailed.value
      loading.value = false
    })

    return { menus, loading, failed }
  }
})
</script>

<style>
.navbar-nav,
.navbar-nav .dropdown-menu,
.navbar-nav .dropdown-item,
.navbar-nav .nav-link {
    font-size: inherit;
    color: inherit;
    font-weight: 500;
}

a.nav-link:hover {
    color: var(--primary) !important;
}
</style>
