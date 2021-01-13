<template>
  <div class="md-toolbar-row">
    <span class="md-title" style="flex: 1">TODO TITLE</span>

    <span style="flex: 1">{{device}} TODO ADD STYLE</span>

    <span>TODO LANG</span>
  </div>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from "vue-property-decorator";

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
      public device: string;
    }
</script>
