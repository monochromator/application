import Vue from "vue";
import "vue-material/dist/theme/default.css";
import "vue-material/dist/vue-material.min.css";
import VueMaterial from "vue-material";
import translations from "./assets/lang";
import App from "./App.vue";
import VueRouter from "vue-router";
import VueI18n from "vue-i18n";
import { routes } from "./routes";
import CountryFlag from "vue-country-flag";

Vue.config.productionTip = false;

// Import material components
Vue.use(VueMaterial);

// Import flag icons
Vue.use(CountryFlag);

// Router
Vue.use(VueRouter);
const router = new VueRouter({
    routes: routes
});

// i18n
Vue.use(VueI18n);
const i18n = new VueI18n({
    messages: translations,
    locale: "fr",
    fallbackLocale: "en",
    silentFallbackWarn: true
});

new Vue({
    i18n: i18n,
    router: router,
    render: h => h(App)
}).$mount("#app");
