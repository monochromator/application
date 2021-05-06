<template>
  <div>
    <md-dialog :md-active.sync="status" :md-close-on-esc="false" :md-click-outside-to-close="false">
      <md-dialog-title>{{$t("calibration_dialog.title")}}</md-dialog-title>

      <md-dialog-content style="width: 50vw" md-dynamic-height>
        <md-field :class="wavelengthIsValid() ? '' : 'md-invalid'">
          <label>{{$t("calibration_dialog.wavelength_name")}}</label>
          <md-input v-model="$data.form.wavelength" type="number" required></md-input>
          <span class="md-error">{{$t("calibration_dialog.wavelength_error")}}</span>
        </md-field>
      </md-dialog-content>

      <md-dialog-actions>
        <md-button class="md-primary md-raised" @click="closeDialog()">{{$t("calibration_dialog.close")}}</md-button>
        <md-button class="md-primary md-raised" @click="calibrate">{{$t("calibration_dialog.run")}}</md-button>
      </md-dialog-actions>
    </md-dialog>

    <md-snackbar md-position="center" :md-duration="3000" :md-active.sync="$data.errorNotification.status" md-persistent>
      <span>{{ $t($data.errorNotification.label, $data.errorNotification.parameters) }}</span>
    </md-snackbar>

    <md-snackbar md-position="left" :md-duration="Infinity" :md-active="$data.calibrationRunning" md-persistent>
      <span>{{$t("calibration_dialog.progress_msg")}}</span>
    </md-snackbar>
  </div>
</template>

<script lang="ts">
    import ErrorNotification from "@/common/ErrorNotification";
    import { Component, Prop, Vue } from "vue-property-decorator";
    import { HttpMethod, query } from "@/services/ChromelyService";

    /**
     * Data contained by calibration form
     */
    interface CalibrationForm {
      wavelength: number;
    }
    /**
     * Data hold by calibration dialog
     */
    interface CalibrationDialogData {
      status: boolean;
      calibrationRunning: boolean;
      errorNotification: ErrorNotification;
      form: CalibrationForm;
    }

    const CALIBRATION_ERROR_EVENT = "calibration.error";
    const CALIBRATION_END_EVENT = "calibration.end";

    /**
     * Main component
     */
    @Component
    export default class CalibrationDialog extends Vue {
        @Prop({ required: true })
        public updateComputingStatus: (running: boolean) => void;

        data(): CalibrationDialogData {
            return {
                status: false,
                calibrationRunning: false,
                errorNotification: {
                    status: false,
                    label: undefined,
                    parameters: undefined
                },
                form: {
                    wavelength: 500
                }
            };
        }

        beforeMount() {
            window.addEventListener(CALIBRATION_END_EVENT, this.onCalibrationEnd);
            window.addEventListener(CALIBRATION_ERROR_EVENT, this.onCalibrationError);
        }

        beforeUpdate() {
            this.updateComputingStatus(this.$data.calibrationRunning);
        }

        beforeDestroy() {
            window.removeEventListener(CALIBRATION_END_EVENT, this.onCalibrationEnd);
            window.removeEventListener(CALIBRATION_ERROR_EVENT, this.onCalibrationError);
        }

        /**
         * Callback on calibration end
         */
        onCalibrationEnd() {
            this.$data.calibrationRunning = false;
        }

        /**
         * Callback on calibration error
         */
        onCalibrationError() {
            this.notifyError("calibration_dialog.calibration_error");
            this.$data.calibrationRunning = false;
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
         * Callibrate monochromator
         */
        calibrate() {
            this.closeDialog();

            query({
                method: HttpMethod.Post,
                url: "/calibration/run",
                postData: {
                    wavelength: parseFloat(this.$data.form.wavelength)
                }
            }, () => {
                this.$data.calibrationRunning = true;
            }, () => {
                this.notifyError("calibration_dialog.calibration_error");
            });
        }

        /**
         * Test whether wavelength is valid
         */
        wavelengthIsValid() {
            const wavelengthAsNumber = parseFloat(this.$data.form.wavelength);

            return !isNaN(wavelengthAsNumber) && wavelengthAsNumber > 0;
        }

        /**
         * Show dialog
         */
        showDialog() {
            this.$data.status = true;
        }

        /**
         * Close dialog
         */
        closeDialog() {
            this.$data.status = false;
        }
    }
</script>
