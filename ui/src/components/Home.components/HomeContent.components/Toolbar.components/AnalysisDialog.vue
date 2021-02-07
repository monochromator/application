<template>
  <div>
    <md-dialog :md-active.sync="status" :md-close-on-esc="false" :md-click-outside-to-close="false">
      <md-dialog-title>{{$t("analysis_dialog.title")}}</md-dialog-title>

      <md-dialog-content style="width: 50vw" md-dynamic-height>
        <div style="margin-bottom: 10px">
          <div class="md-subheading" style="margin-bottom: 10px">{{$t("analysis_dialog.presets.section_name")}}</div>

          <md-chip v-for="preset in this.presets" :md-clickable="true" :key="preset.name" @click="applyPreset(preset)">{{$t("analysis_dialog.presets." + preset.name)}}</md-chip>
        </div>
        <md-divider></md-divider>

        <md-field :class="nameIsValid() ? '' : 'md-invalid'">
          <label>{{$t("analysis_dialog.name_label")}}</label>
          <md-input v-model="$data.form.name" type="text" :maxlength="this.nameLimit"></md-input>
          <span class="md-error">{{$t("analysis_dialog.name_error", { max: this.nameLimit })}}</span>
        </md-field>

        <md-field :class="rangeIsValid() ? '' : 'md-invalid'">
          <label>{{$t("analysis_dialog.range_start_label")}}</label>
          <md-input v-model="$data.form.start" type="number" required></md-input>
          <span class="md-error">{{$t("analysis_dialog.range_start_error")}}</span>
        </md-field>

        <md-field :class="rangeIsValid() ? '' : 'md-invalid'">
          <label>{{$t("analysis_dialog.range_end_label")}}</label>
          <md-input v-model="$data.form.end" type="number" required></md-input>
          <span class="md-error">{{$t("analysis_dialog.range_end_error")}}</span>
        </md-field>

        <md-field :class="stepIsValid() ? '' : 'md-invalid'">
          <label>{{$t("analysis_dialog.range_step_label")}}</label>
          <md-input v-model="$data.form.step" type="number" required></md-input>
          <span class="md-error">{{$t("analysis_dialog.range_step_error")}}</span>
        </md-field>

        <md-field :class="iterationsIsValid() ? '' : 'md-invalid'">
          <label>{{$t("analysis_dialog.iterations_label")}}</label>
          <md-input v-model="$data.form.iterations" type="number" required></md-input>
          <span class="md-error">{{$t("analysis_dialog.iterations_error")}}</span>
        </md-field>
      </md-dialog-content>

      <md-dialog-actions>
        <md-button class="md-primary md-raised" @click="closeDialog()">{{$t("analysis_dialog.close")}}</md-button>
        <md-button class="md-primary md-raised" @click="analyse" :disabled="!stepIsValid() || !rangeIsValid() || !nameIsValid()">{{$t("analysis_dialog.run")}}</md-button>
      </md-dialog-actions>
    </md-dialog>

    <md-snackbar md-position="left" :md-duration="Infinity" :md-active="analysisIsRunning()" md-persistent>
      <span>{{$t("analysis_dialog.looper.message", { count: $data.analysisStatus.remaining })}}</span>
      <md-button class="md-primary" @click="stopAnalysis()">{{$t("analysis_dialog.looper.stop")}}</md-button>
    </md-snackbar>

    <md-snackbar md-position="center" :md-duration="3000" :md-active.sync="$data.errorNotification.status" md-persistent>
      <span>{{ $t($data.errorNotification.label, $data.errorNotification.parameters) }}</span>
    </md-snackbar>
  </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from "vue-property-decorator";
    import ErrorNotification from "@/common/ErrorNotification";
    import { HttpMethod, query } from "@/services/ChromelyService";
    import { NotCalibrated } from "@/common/ErrorCodes";

    /**
     * Data contained by analysis form
     */
    interface AnalysisForm {
      name?: string;
      start: number;
      end: number;
      step: number;
      iterations: number;
    }

    /**
     * Analysis status
     */
    interface AnalysisStatus {
      remaining?: number;
      lastAnalysis?: string;
    }

    /**
     * Data hold by analysis dialog
     */
    interface AnalysisDialogData {
      status: boolean;
      analysisStatus: AnalysisStatus;
      errorNotification: ErrorNotification;
      form: AnalysisForm;
    }

    /**
     * Analysis preset
     */
    interface Preset {
      name: string;
      start: number;
      end: number;
    }

    const ANALYSIS_RESULTS_EVENT = "analysis.end";
    const ANALYSIS_ERROR_EVENT = "analysis.error";

    /**
     * Main component
     */
    @Component
    export default class AnalysisDialog extends Vue {
        @Prop({ required: true })
        public addAnalysis: (data: [number, number][], name?: string) => string;

        @Prop({ required: true })
        public updateAnalysis: (id: string, data: [number, number][]) => void;

        @Prop({ required: true })
        public updateAnalysisStatus: (running: boolean) => void;

        private nameLimit = 30;
        private presets: Preset[] = [
          {
            name: "visible",
            start: 380,
            end: 750
          }
        ];

        data(): AnalysisDialogData {
            return {
                status: false,
                analysisStatus: {
                    remaining: undefined,
                    lastAnalysis: undefined
                },
                errorNotification: {
                    status: false,
                    label: undefined,
                    parameters: undefined
                },
                form: {
                    name: undefined,
                    start: 380,
                    end: 750,
                    step: 1,
                    iterations: 1
                }
            };
        }

        beforeMount() {
            window.addEventListener(ANALYSIS_RESULTS_EVENT, this.onAnalysisResults);
            window.addEventListener(ANALYSIS_ERROR_EVENT, this.onAnalysisError);
        }

        beforeUpdate() {
            this.updateAnalysisStatus(this.analysisIsRunning());
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
            const detail = (event as CustomEvent).detail;

            // Decrement remaining analysis count
            let relaunch = false;
            if (this.$data.analysisStatus.remaining !== undefined) {
                relaunch = --this.$data.analysisStatus.remaining > 0;
            }

            // Add or update analysis
            if (this.$data.analysisStatus.lastAnalysis !== undefined) {
                this.updateAnalysis(this.$data.analysisStatus.lastAnalysis, detail.data);
            } else {
                this.$data.analysisStatus.lastAnalysis = this.addAnalysis(detail.data, detail.name);
            }

            // Relaunch another analysis
            if (relaunch) {
                query({
                    method: HttpMethod.Post,
                    url: "/analysis/run",
                    postData: {
                        name: this.$data.form.name === undefined || this.$data.form.name.length === 0 ? undefined : this.$data.form.name,
                        start: parseFloat(this.$data.form.start),
                        end: parseFloat(this.$data.form.end),
                        step: parseFloat(this.$data.form.step)
                    }
                }, () => {
                    // Do nothing
                }, e => {
                    this.notifyError(e.StatusText === NotCalibrated ? "analysis_dialog.process.not_calibrated" : "analysis_dialog.process.start_error");

                    // Cancel analysis process
                    this.$data.analysisStatus = {
                        remaining: undefined,
                        lastAnalysis: undefined
                    };
                });
            }
        }

        /**
         * Callback on analysis error
         */
        onAnalysisError() {
            this.notifyError("analysis_dialog.process.error");

            // Cancel analysis process
            this.$data.analysisStatus = {
                remaining: undefined,
                lastAnalysis: undefined
            };
        }

        /**
         * Run analysis
         */
        analyse() {
            this.closeDialog();

            query({
                method: HttpMethod.Post,
                url: "/analysis/run",
                postData: {
                    name: this.$data.form.name === undefined || this.$data.form.name.length === 0 ? undefined : this.$data.form.name,
                    start: parseFloat(this.$data.form.start),
                    end: parseFloat(this.$data.form.end),
                    step: parseFloat(this.$data.form.step)
                }
            }, () => {
                this.$data.analysisStatus = {
                    remaining: parseInt(this.$data.form.iterations),
                    lastAnalysis: undefined
                };
            }, e => {
                this.notifyError(e.StatusText === NotCalibrated ? "analysis_dialog.process.not_calibrated" : "analysis_dialog.process.start_error");
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
        }

        /**
         * Test whether analysis step is valid
         */
        stepIsValid() {
          const stepAsNumber = parseFloat(this.$data.form.step);

          return !isNaN(stepAsNumber) && stepAsNumber > 0;
        }

        /**
         * Test whether analysis iterations is valid
         */
        iterationsIsValid() {
          const iterationsAsNumber = parseInt(this.$data.form.iterations);

          return !isNaN(iterationsAsNumber) && iterationsAsNumber > 0;
        }

        /**
         * Test whether analysis range is valid
         */
        rangeIsValid() {
            const startAsNumber = parseFloat(this.$data.form.start);
            const endAsNumber = parseFloat(this.$data.form.end);

            // Avoid comparison between non numbers
            if (isNaN(startAsNumber) || isNaN(endAsNumber)) {
                return false;
            }

            return startAsNumber <= endAsNumber;
        }

        /**
         * Test whether analysis name is valid
         */
        nameIsValid() {
            return this.$data.form.name === undefined || this.$data.form.name.length <= this.nameLimit;
        }

        /**
         * Apply the given preset
         *
         * @param preset Preset to apply
         */
        applyPreset(preset: Preset) {
            this.$data.form.start = preset.start;
            this.$data.form.end = preset.end;
        }

        /**
         * Test whether an analysis is running
         */
        analysisIsRunning() {
            return this.$data.analysisStatus.remaining !== undefined && this.$data.analysisStatus.remaining > 0;
        }

        /**
         * Stop running analysis
         */
        stopAnalysis() {
            this.$data.analysisStatus.remaining = 0;

            // TODO: Implement analysis killer
        }
    }
</script>
