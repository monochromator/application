<template>
  <div class="md-toolbar-row">
    <md-button class="md-button md-raised md-accent" @click="showComparisonInputDialog">{{ $t("home_content_toolbar.compare_button") }}</md-button>
    <span style="flex: 1"></span>

    <md-button class="md-button md-raised md-accent" @click="showCalibrationDialog" :disabled="$data.analysisRunning">
      {{ $t("home_content_toolbar.calibration_button") }}
    </md-button>
    <md-button class="md-button md-raised md-accent" @click="showAnalysisDialog" :disabled="$data.analysisRunning">
      {{ $t("home_content_toolbar.analysis_button") }}
    </md-button>

    <ComparisonInputDialog ref="comparisonInputDialog" />
    <AnalysisDialog ref="analysisDialog" :addAnalysis="addAnalysis" :updateAnalysis="updateAnalysis" :updateAnalysisStatus="updateAnalysisStatus" />
    <CalibrationDialog ref="calibrationDialog" />
  </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from "vue-property-decorator";
    import AnalysisDialog from "@/components/Home.components/HomeContent.components/Toolbar.components/AnalysisDialog.vue";
    import AnalysisMetaData from "@/common/AnalysisMetaData";
    import ComparisonInputDialog
        from "@/components/Home.components/HomeContent.components/Toolbar.components/ComparisonInputDialog.vue";
    import CalibrationDialog
        from "@/components/Home.components/HomeContent.components/Toolbar.components/CalibrationDialog.vue";

    /**
     * Data hold by ToolBar component
     */
    interface ToolBarData {
        analysisRunning: boolean;
    }

    /**
     * Main component
     */
    @Component({
      components: { ComparisonInputDialog, CalibrationDialog, AnalysisDialog }
    })
    export default class ToolBar extends Vue {
        @Prop({ required: true })
        public addAnalysis: (data: [number, number][], name?: string) => string;

        @Prop({ required: true })
        public updateAnalysis: (id: string, data: [number, number][]) => void;

      @Prop({ required: true })
      public analysesSupplier: () => [AnalysisMetaData, [number, number][]][];

      data(): ToolBarData {
            return {
                analysisRunning: false
            };
        }
        /**
         * Show analysis dialog
         */
        showAnalysisDialog() {
            (this.$refs.analysisDialog as AnalysisDialog).showDialog();
        }

        /**
         * Show comparison input dialog
         */
        showComparisonInputDialog() {
            (this.$refs.comparisonInputDialog as ComparisonInputDialog).showDialog(this.analysesSupplier());
        }

        /**
         * Show calibration dialog
         */
        showCalibrationDialog() {
            (this.$refs.calibrationDialog as CalibrationDialog).showDialog();
        }

        /**
         * Update analysis status
         *
         * @param running
         */
        updateAnalysisStatus(running: boolean) {
            this.$data.analysisRunning = running;
        }
    }
</script>
