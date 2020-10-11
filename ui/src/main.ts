import Vue from "vue";
import "vue-material/dist/theme/default.css";
import "vue-material/dist/vue-material.min.css";
import App from "./App.vue";

Vue.config.productionTip = false;

// Import material components (N.B: Import one by one and not the full bundle!!!)

new Vue({
    render: h => h(App)
}).$mount("#app");
