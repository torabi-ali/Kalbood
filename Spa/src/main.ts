import { createApp } from 'vue'
import { createMetaManager, plugin as metaPlugin } from 'vue-meta'
import App from './App.vue'
import router from './router'

// #region   Custom Components
import AppNavbar from './components/AppNavbar.vue'
import AppFooter from './components/AppFooter.vue'
import AppFaq from './components/AppFaq.vue'
import PostCard from './components/PostCard.vue'
import CategoryCard from './components/CategoryCard.vue'
// #endregion

const app = createApp(App)
app.use(router)
app.use(createMetaManager())
app.use(metaPlugin)

// #region   Custom Components
app.component('AppNavbar', AppNavbar)
app.component('AppFooter', AppFooter)
app.component('AppFaq', AppFaq)
app.component('PostCard', PostCard)
app.component('CategoryCard', CategoryCard)
// #endregion

app.mount('#app')
