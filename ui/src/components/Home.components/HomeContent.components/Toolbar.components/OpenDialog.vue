<template>
  <div>
    <md-dialog :md-active.sync="status">
      <md-dialog-title>{{$t("open_dialog.title")}}</md-dialog-title>

      <md-dialog-content style="width: 50vw" md-dynamic-height>
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
                }
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
                    const csv = fromCSV(event.target.result.toString());

                    if (csv.length > 0) {
                        this.addAnalysis(csv, files[0].name);
                    }
                }
                this.$data.status = false;
            };
            reader.readAsText(files[0]);
        }
    }
</script>
