<template>
  <div class="md-toolbar-row">
    <span class="md-title" style="flex: 1">TODO TITLE</span>

    <span style="flex: 1">
      <md-button class="md-icon-button md-dense md-raised md-accent" @click="ping()">
        <md-icon>cached</md-icon>
      </md-button>
      {{device}}
      <md-chip :class="'device-status device-' + ($data.deviceConnected ? 'connected' : 'disconnected')">{{$t("toolbar.device_" + ($data.deviceConnected ? "connected" : "disconnected"))}}</md-chip>
    </span>

    <span>TODO LANG</span>
  </div>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from "vue-property-decorator";
    import { HttpMethod, query } from "@/services/ChromelyService";

    /**
     * Data hold by toolbar component
     */
    interface ToolBarData {
      deviceConnected: boolean;
    }

    /**
     * Main component
     */
    @Component
    export default class ToolBar extends Vue {
      @Prop({ required: true })
      public device?: string;

      data(): ToolBarData {
        return {
          deviceConnected: true
        };
      }

      /**
       * Ping device and update its status
       */
      ping() {
        query({
          method: HttpMethod.Get,
          url: "/mbed/current/ping"
        }, () => {
          this.$data.deviceConnected = true;
        }, () => {
          this.$data.deviceConnected = false;
        });
      }
    }
</script>

<style scoped>
.device-status {
  text-transform: uppercase;
  color: white;

  margin-left: 10px;
}

.device-connected {
  background-color: green;
}

.device-disconnected {
  background-color: red;
}
</style>
