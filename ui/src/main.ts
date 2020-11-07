import Vue from "vue";
import "vue-material/dist/theme/default.css";
import "vue-material/dist/vue-material.min.css";
import translations from "./assets/lang";
import App from "./App.vue";
import VueI18n, { LocaleMessages } from "vue-i18n";

Vue.config.productionTip = false;

// Import material components (N.B: Import one by one and not the full bundle!!!)

// i18n
Vue.use(VueI18n);
const i18n = new VueI18n({
    messages: translations as LocaleMessages,
    locale: "en",
    fallbackLocale: "en",
    silentFallbackWarn: true
});

new Vue({
    i18n: i18n,
    render: h => h(App)
}).$mount("#app");
