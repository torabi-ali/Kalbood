<template>
  <footer class="footer border-top">
    <div class="container">
      <footer class="pt-5">
        <div class="row">
          <div class="col-6 col-md-3">
            <h5>Section</h5>
            <ul class="nav flex-column">
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">Home</a></li>
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">Features</a></li>
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">Pricing</a></li>
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">FAQs</a></li>
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">About</a></li>
            </ul>
          </div>

          <div class="col-6 col-md-3">
            <h5>Section</h5>
            <ul class="nav flex-column">
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">Home</a></li>
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">Features</a></li>
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">Pricing</a></li>
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">FAQs</a></li>
              <li class="nav-item mb-2"><a href="#" class="nav-link p-0 text-muted">About</a></li>
            </ul>
          </div>

          <div class="col-md-6">
            <a class="navbar-brand" href="/">
              <img class="rounded mx-auto d-block" src="@/assets/image/kalbood-logo-128x128.png" :title="appSettings.title" :alt="appSettings.title">
            </a>

            <p class="text-center">{{ appSettings.description }}</p>
          </div>
        </div>

        <div class="d-flex flex-column flex-sm-row justify-content-between pt-4 mt-4 border-top">
          <p class="small">© {{ new Date().getFullYear() }} تمامی حقوق محفوظ است.</p>
          <ul class="list-unstyled d-flex">
            <li class="ms-3"><a class="link-dark" :href="appSettings.whatsapp"><i class="fa-brands fa-whatsapp"></i></a></li>
            <li class="ms-3"><a class="link-dark" :href="appSettings.instagram"><i class="fa-brands fa-instagram"></i></a></li>
            <li class="ms-3"><a class="link-dark" :href="appSettings.telegram"><i class="fa-brands fa-telegram"></i></a></li>
          </ul>
        </div>
      </footer>
    </div>
  </footer>
</template>

<script lang="ts">
import { defineComponent } from 'vue'
import AppSettings from '@/AppSettings'

export default defineComponent({
  name: 'AppFooter',
  setup () {
    const appSettings = AppSettings.getInstance()
    return { appSettings }
  }
})
</script>
