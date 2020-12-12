<template>
  <div class="md-toolbar-row">
    <md-button class="md-icon-button" @click="toggleMenu">
      <md-icon>menu</md-icon>
    </md-button>

    <span class="md-title" style="flex: 1">TODO TITLE</span>

    <span style="flex: 1">{{device}} TODO ADD STYLE</span>

    <span>TODO LANG</span>
  </div>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from "vue-property-decorator";
    import Home from "@/components/Home.vue";

    /**
     * Data hold by toolbar component
     */
    interface ToolBarData {
      menuVisible: boolean;
    }

    /**
     * Main component
     */
    @Component
    export default class ToolBar extends Vue {
      @Prop({ required: true })
      public device?: string;

      @Prop({ required: true })
      public parent!: Home;

      /**
       * Toggle menu
       */
      toggleMenu() {
        this.parent.toggleMenu();
      }
    }
</script>
