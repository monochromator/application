<template>
  <div>
    <md-app md-waterfall md-mode="fixed" id="home-app">
      <md-app-toolbar class="md-primary">
        <ToolBar :device="$data.device"></ToolBar>
      </md-app-toolbar>

      <md-app-content>
        <Content/>
      </md-app-content>
    </md-app>
  </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";
    import ToolBar from "@/components/Home.components/ToolBar.vue";
    import Content from "@/components/Home.components/Content.vue";

    /**
     * Data hold by home component
     */
    interface HomeData {
      device: string;
    }

    /**
     * Main component
     */
    @Component({
      components: { Content, ToolBar }
    })
    export default class Home extends Vue {
        data(): HomeData {
            return {
                device: this.$route.query.device + ""
            };
        }
    }
</script>

<style scoped>
#home-app {
  width: 100vw;
  height: 100vh;
}
</style>
