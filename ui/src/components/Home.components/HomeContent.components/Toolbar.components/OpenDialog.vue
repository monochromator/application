<template>
  <div>
    <md-dialog :md-active.sync="status">
      <md-dialog-title>{{$t("open_dialog.title")}}</md-dialog-title>

      <md-dialog-content style="width: 50vw" md-dynamic-height>
        <md-field>
          <md-textarea v-model="$data.csvInput" :placeholder="$t('open_dialog.csv_placeholder')"></md-textarea>
        </md-field>

        <div style="display: flex; justify-content: center; margin-bottom: 10px">
          <md-button class="md-button md-raised md-primary" @click="load($data.csvInput)">{{$t("open_dialog.open")}}</md-button>
        </div>

        <div style="display: flex">
          <md-divider style="flex: 1"></md-divider>
          <span style="flex: 1; text-align: center;text-transform: uppercase">{{$t("open_dialog.or")}}</span>
          <md-divider style="flex: 1"></md-divider>
        </div>

        <md-field>
          <md-file @md-change="upload" />
        </md-field>
      </md-dialog-content>
    </md-dialog>

    <md-snackbar md-position="center" :md-duration="3000" :md-active.sync="$data.errorNotification.status" md-persistent>
      <span>{{ $t($data.errorNotification.label, $data.errorNotification.parameters) }}</span>
    </md-snackbar>
  </div>
</template>

<script lang="ts">
    import ErrorNotification from "@/common/ErrorNotification";
    import { Component, Prop, Vue } from "vue-property-decorator";
    import { fromCSV } from "@/services/CSVService";

    /**
     * Data hold by calibration dialog
     */
    interface OpenDialogData {
      status: boolean;
      errorNotification: ErrorNotification;
      csvInput: string;
    }

    /**
     * Main component
     */
    @Component
    export default class OpenDialog extends Vue {
        @Prop({ required: true })
        public addAnalysis: (data: [number, number][], name?: string) => string;

        data(): OpenDialogData {
            return {
                status: false,
                errorNotification: {
                    status: false,
                    label: undefined,
                    parameters: undefined
                },
                csvInput: ""
            };
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
         * Show dialog
         */
        showDialog() {
            this.$data.status = true;
        }

        /**
         * Upload the selected file
         */
        upload(files: FileList) {
            if (files.length === 0) {
                return;
            }

            const reader = new FileReader();
            reader.onload = (event) => {
                if (event.target !== null && event.target.result !== null) {
                    this.load(event.target.result.toString(), files[0].name);
                }
            };
            reader.readAsText(files[0]);
        }

        /**
         * Load the given CSV
         *
         * @param csv CSV as string
         * @param name Name
         */
        load(csv: string, name?: string) {
            try {
              const analysis = fromCSV(csv);

              if (analysis.length > 0) {
                this.addAnalysis(analysis, name);
                this.$data.status = false;
              }
            } catch (e) {
                this.notifyError("open_dialog.load_error");
            }
        }
    }
</script>
