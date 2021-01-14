<template>
  <div>
    <md-dialog :md-active.sync="status" :md-close-on-esc="false" :md-click-outside-to-close="false">
      <md-dialog-title>{{$t("analysis_dialog.title")}}</md-dialog-title>

      <md-dialog-content md-dynamic-height>
        <md-field :class="rangeIsValid() ? '' : 'md-invalid'">
          <label>{{$t("analysis_dialog.range_start_label")}}</label>
          <md-input v-model="$data.form.start" type="number"></md-input>
          <span class="md-error">{{$t("analysis_dialog.range_start_error")}}</span>
        </md-field>

        <md-field :class="rangeIsValid() ? '' : 'md-invalid'">
          <label>{{$t("analysis_dialog.range_end_label")}}</label>
          <md-input v-model="$data.form.end" type="number"></md-input>
          <span class="md-error">{{$t("analysis_dialog.range_end_error")}}</span>
        </md-field>

        <md-field :class="stepIsValid() ? '' : 'md-invalid'">
          <label>{{$t("analysis_dialog.range_step_label")}}</label>
          <md-input v-model="$data.form.step" type="number"></md-input>
          <span class="md-error">{{$t("analysis_dialog.range_step_error")}}</span>
        </md-field>
      </md-dialog-content>

      <md-dialog-actions>
        <md-button class="md-primary md-raised" @click="closeDialog()">{{$t("analysis_dialog.close")}}</md-button>
        <md-button class="md-primary md-raised" @click="analyse" :disabled="!stepIsValid() || !rangeIsValid() || $data.analysisRunning">{{$t("analysis_dialog.run")}}</md-button>
        <span style="width: 10px" v-if="$data.analysisRunning" />
        <md-progress-spinner md-mode="indeterminate" :md-diameter="30" :md-stroke="5" v-if="$data.analysisRunning"></md-progress-spinner>
      </md-dialog-actions>
    </md-dialog>

    <md-snackbar md-position="center" :md-duration="3000" :md-active.sync="$data.errorNotification.status" md-persistent>
      <span>{{ $t($data.errorNotification.label, $data.errorNotification.parameters) }}</span>
    </md-snackbar>
  </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from "vue-property-decorator";
    import ErrorNotification from "@/common/ErrorNotification";
    import { HttpMethod, query } from "@/services/ChromelyService";

    /**
     * Data contained by analysis form
     */
    interface AnalysisForm {
      start: number;
      end: number;
      step: number;
    }

    /**
     * Data hold by analysis dialog
     */
    interface AnalysisDialogData {
      status: boolean;
      analysisRunning: boolean;
      errorNotification: ErrorNotification;
      form: AnalysisForm;
    }

    const ANALYSIS_RESULTS_EVENT = "analysis.end";
    const ANALYSIS_ERROR_EVENT = "analysis.error";

    /**
     * Main component
     */
    @Component
    export default class AnalysisDialog extends Vue {
        @Prop({ required: true })
        public addAnalysis: (data: [number, number][]) => void;

        data(): AnalysisDialogData {
            return {
                status: false,
                analysisRunning: false,
                errorNotification: {
                    status: false,
                    label: undefined,
                    parameters: undefined
                },
                form: {
                    start: 380,
                    end: 750,
                    step: 1
                }
            };
        }

        beforeMount() {
            window.addEventListener(ANALYSIS_RESULTS_EVENT, this.onAnalysisResults);
            window.addEventListener(ANALYSIS_ERROR_EVENT, this.onAnalysisError);
        }

        beforeDestroy() {
            window.removeEventListener(ANALYSIS_RESULTS_EVENT, this.onAnalysisResults);
            window.removeEventListener(ANALYSIS_ERROR_EVENT, this.onAnalysisError);
        }

        /**
         * Callback on analysis results
         *
         * @event Event
         */
        onAnalysisResults(event: Event) {
            this.addAnalysis((event as CustomEvent).detail);
            this.closeDialog();
        }

        /**
         * Callback on analysis error
         */
        onAnalysisError() {
            this.notifyError("home_content_toolbar.analysis_error");
            this.closeDialog();
        }

        /**
         * Run analysis
         */
        analyse() {
            this.$data.analysisRunning = true;

            query({
                method: HttpMethod.Post,
                url: "/analysis/run",
                postData: {
                    start: parseFloat(this.$data.form.start),
                    end: parseFloat(this.$data.form.end),
                    step: parseFloat(this.$data.form.step)
                }
            }, () => {
                // Do nothing
            }, () => {
                this.notifyError("home_content_toolbar.analysis_start_error");
                this.closeDialog();
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
            this.$data.analysisRunning = false;

            // TODO: Implement close with analysis killer
        }

        /**
         * Test whether analysis step is valid
         */
        stepIsValid() {
            return this.$data.form.step > 0;
        }

        /**
         * Test whether analysis range is valid
         */
        rangeIsValid() {
            return this.$data.form.start <= this.$data.form.end;
        }
    }
</script>
