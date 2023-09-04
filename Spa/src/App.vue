<template>
  <metainfo>
    <template v-slot:title="{ content }">{{ `${content} | ${appSetting.name}` }}</template>
    <template v-slot:og(title)="{ content }">{{ `${content} | ${appSetting.name}` }}</template>
    <template v-slot:twitter(title)="{ content }">{{ `${content} | ${appSetting.name}` }}</template>
  </metainfo>
  <AppNavbar />
  <router-view />
  <AppFaq />
  <AppFooter />
</template>

<style>
@import 'bootstrap/dist/css/bootstrap.rtl.min.css';
@import 'vazirmatn/Round-Dots/misc/Farsi-Digits/Vazirmatn-RD-FD-font-face.css';
@import '@fortawesome/fontawesome-free/css/all.min.css';
@import 'assets/main.css';
</style>

<script lang="ts">
import { useMeta } from 'vue-meta'
import AppSettings from '@/AppSettings'
import 'bootstrap/dist/js/bootstrap.min.js'

export default {
  setup () {
    useMeta({
      title: AppSettings.getInstance().title,
      description: AppSettings.getInstance().description,
      keywords: AppSettings.getInstance().keywords,
      author: AppSettings.getInstance().author,
      og: {
        title: AppSettings.getInstance().title,
        description: AppSettings.getInstance().description,
        image: AppSettings.getInstance().logo,
        site_name: AppSettings.getInstance().name,
        locale: 'fa_IR'
      },
      twitter: {
        title: AppSettings.getInstance().title,
        description: AppSettings.getInstance().description,
        image: AppSettings.getInstance().logo,
        site: AppSettings.getInstance().name,
        creator: AppSettings.getInstance().author
      }
    })

    return { appSetting: AppSettings.getInstance() }
  }
}
</script>
