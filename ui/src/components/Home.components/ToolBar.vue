<template>
  <div class="md-toolbar-row">
    <span style="flex: 1">
      <md-button class="md-icon-button" @click="$data.showConfirmQuitDialog = true">
        <md-icon>home</md-icon>
      </md-button>
    </span>

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

    <md-dialog-confirm
        :md-active.sync="$data.showConfirmQuitDialog"
        :md-title="$t('toolbar.confirm_quit_dialog.title')"
        :md-content="$t('toolbar.confirm_quit_dialog.text')"
        :md-confirm-text="$t('common.yes')"
        :md-cancel-text="$t('common.no')"
        @md-cancel="$data.showConfirmQuitDialog = false"
        @md-confirm="goToStartUp()" />

    <md-snackbar md-position="center" :md-duration="3000" :md-active.sync="$data.errorNotification.status" md-persistent>
      <span>{{ $t($data.errorNotification.label, $data.errorNotification.parameters) }}</span>
    </md-snackbar>
  </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from "vue-property-decorator";
    import { HttpMethod, query } from "@/services/ChromelyService";
    import ErrorNotification from "@/common/ErrorNotification";
    import { STARTUP_PATH } from "@/routes";

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
      showConfirmQuitDialog: boolean;
      errorNotification: ErrorNotification;
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
          deviceConnected: true,
          showConfirmQuitDialog: false,
          errorNotification: {
            status: false,
            label: undefined,
            parameters: undefined
          }
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

      /**
       * Change application language
       *
       * @param lang New language
       */
      changeLang(lang: SupportedLanguages) {
        this.$root.$i18n.locale = lang;
      }

      /**
       * Disconnect current controller and go to startup screen
       */
      goToStartUp() {
        query({
          method: HttpMethod.Get,
          url: "/mbed/current/disconnect"
        }, () => {
          this.$router.push({
            path: STARTUP_PATH
          });
        }, () => {
          this.notifyError("toolbar.disconnect_error");
        });
      }

      /**
       * Notify an error to user
       *
       * @param label Error label
       * @param parameters Parameters
       */
      notifyError(label: string, parameters?: { [key: string]: string }) {
        this.$data.errorNotification.label = label;
        this.$data.errorNotification.status = true;
        this.$data.errorNotification.parameters = parameters;
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
