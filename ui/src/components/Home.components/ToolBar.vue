<template>
  <div class="md-toolbar-row">
    <span class="md-title" style="flex: 1">{{$t("toolbar.title")}}</span>

    <span style="flex: 1">
      <md-button class="md-icon-button md-dense md-raised md-accent" @click="ping()">
        <md-icon>cached</md-icon>
      </md-button>
      {{device}}
      <md-chip :class="'device-status device-' + ($data.deviceConnected ? 'connected' : 'disconnected')">{{$t("toolbar.device_" + ($data.deviceConnected ? "connected" : "disconnected"))}}</md-chip>
    </span>

    <md-menu>
      <md-button class="md-icon-button" md-menu-trigger>
        <md-icon>translate</md-icon>
      </md-button>

      <md-menu-content>
        <md-menu-item @click="changeLang('fr')">
          <country-flag country='fr'/>
          <span>{{$t("toolbar.lang.fr")}}</span>
        </md-menu-item>

        <md-menu-item @click="changeLang('en')">
          <country-flag country='us'/>
          <span>{{$t("toolbar.lang.en")}}</span>
        </md-menu-item>
      </md-menu-content>
    </md-menu>
  </div>
</template>

<script lang="ts">
    import { Component, Vue, Prop } from "vue-property-decorator";
    import { HttpMethod, query } from "@/services/ChromelyService";

    /**
     * Supported languages
     */
    enum SupportedLanguages {
      Fr = "fr",
      En = "en"
    }

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
      public device: string;

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

      /**
       * Change application language
       *
       * @param lang New language
       */
      changeLang(lang: SupportedLanguages) {
        this.$root.$i18n.locale = lang;
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
