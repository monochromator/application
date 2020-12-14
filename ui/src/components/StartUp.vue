<template>
  <div>
    <md-dialog :md-active="true">
      <md-dialog-title>
        <md-toolbar class="md-transparent" md-elevation="0">
          <span style="flex: 1">{{ $t("startup.select_device") }}</span>

          <md-button class="md-icon-button md-raised" @click="refreshDevices" :disabled="$data.detectionInProgress || $data.connectionInProgress">
            <md-icon>refresh</md-icon>
          </md-button>
        </md-toolbar>
      </md-dialog-title>

      <md-dialog-content class="device-dialog-content">
        <div v-for="device in $data.devices" v-bind:key="device">
          <md-radio v-model="$data.selectedDevice" :value="device">{{ device }}</md-radio>
        </div>
      </md-dialog-content>

      <md-dialog-actions>
        <md-button class="md-primary" @click="select($data.selectedDevice)" :disabled="$data.selectedDevice === undefined || $data.detectionInProgress || $data.connectionInProgress">{{ $t("startup.select") }}
        </md-button>
        <md-button class="md-primary" @click="autodetect" :disabled="$data.detectionInProgress || $data.connectionInProgress">{{ $t("startup.autodetect") }}</md-button>
      </md-dialog-actions>
    </md-dialog>

    <md-dialog :md-active.sync="$data.detectionInProgress">
      <md-dialog-title>{{ $t("startup.autodetect_loading") }}</md-dialog-title>

      <md-dialog-content>
        <div class="md-layout md-alignment-center-center">
          <div class="md-layout-item md-size-33"></div>
          <div class="md-layout-item md-size-33">
            <md-progress-spinner md-mode="indeterminate"></md-progress-spinner>
          </div>
          <div class="md-layout-item md-size-33"></div>
        </div>
      </md-dialog-content>
    </md-dialog>

    <md-dialog :md-active.sync="$data.connectionInProgress">
      <md-dialog-title>{{ $t("startup.connection_loading") }}</md-dialog-title>

      <md-dialog-content>
        <div class="md-layout md-alignment-center-center">
          <div class="md-layout-item md-size-33"></div>
          <div class="md-layout-item md-size-33">
            <md-progress-spinner md-mode="indeterminate"></md-progress-spinner>
          </div>
          <div class="md-layout-item md-size-33"></div>
        </div>
      </md-dialog-content>
    </md-dialog>

    <md-snackbar md-position="center" :md-duration="3000" :md-active.sync="$data.errorNotification.status" md-persistent>
      <span>{{ $t($data.errorNotification.label, $data.errorNotification.parameters) }}</span>
    </md-snackbar>
  </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";
    import { HttpMethod, query } from "@/services/ChromelyService";
    import { HOME_PATH } from "@/routes";
    import ErrorNotification from "@/common/ErrorNotification";

    type Device = string;

    /**
     * Data hold by startup component
     */
    interface StartUpData {
      devices: Device[];
      selectedDevice?: Device;
      detectionInProgress: boolean;
      connectionInProgress: boolean;
      errorNotification: ErrorNotification;
    }

    /**
     * Main component
     */
    @Component
    export default class StartUp extends Vue {
        data(): StartUpData {
            return {
                devices: [],
                selectedDevice: undefined,
                detectionInProgress: false,
                connectionInProgress: false,
                errorNotification: {
                    status: false,
                    label: undefined,
                    parameters: undefined
                }
            };
        }

        mounted() {
            // Fetch devices
            this.fetchDevices();
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

        /**
         * Fetch available devices
         */
        fetchDevices() {
            query({
                method: HttpMethod.Get,
                url: "/mbed/list"
            }, response => { // Update devices
                this.$data.devices = response.Data;
            }, () => {
                this.notifyError("startup.fetch_devices_error");
            });
        }

        /**
         * Refresh devices list
         */
        refreshDevices() {
            this.fetchDevices();
            this.$data.selectedDevice = undefined;
        }

        /**
         * Detect device automatically
         */
        autodetect() {
            this.$data.detectionInProgress = true;

            query({
                method: HttpMethod.Get,
                url: "/mbed/autodetect"
            }, response => { // Select returned device
                this.$data.selectedDevice = response.Data;
                this.$forceUpdate();

                this.$data.detectionInProgress = false;
            }, () => {
                this.$data.detectionInProgress = false;

                this.notifyError("startup.devices_detection_error");
            });
        }

        /**
         * Select device
         *
         * @param device Selected device
         */
        select(device: string) {
            this.$data.connectionInProgress = true;

            query({
                method: HttpMethod.Get,
                url: "/mbed/connect",
                parameters: {
                    device: device
                }
            }, () => {
                this.$data.connectionInProgress = false;

                // Go to home
                this.$router.push({
                    path: HOME_PATH,
                    query: {
                        device: device
                    }
                });
            }, () => {
                this.$data.connectionInProgress = false;

                this.notifyError("startup.device_connection_error", {
                    device: device
                });
            });
        }
    }
</script>

<style scoped>
.device-dialog-content {
  min-width: 50vw;
  margin: 0 30px;
}
</style>
