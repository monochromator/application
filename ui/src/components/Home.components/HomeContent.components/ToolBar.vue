<template>
  <div class="md-toolbar-row">
    <span style="flex: 1"></span>

    <md-button class="md-button md-raised md-accent" @click="showCalibrationDialog">
      {{ $t("home_content_toolbar.calibration_button") }}
    </md-button>
    <md-button class="md-button md-raised md-accent" @click="showAnalysisDialog">
      {{ $t("home_content_toolbar.analysis_button") }}
    </md-button>

    <AnalysisDialog ref="analysisDialog" :addAnalysis="addAnalysis"/>
    <CalibrationDialog ref="calibrationDialog" />
  </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from "vue-property-decorator";
    import AnalysisDialog from "@/components/Home.components/HomeContent.components/Toolbar.components/AnalysisDialog.vue";
    import CalibrationDialog
        from "@/components/Home.components/HomeContent.components/Toolbar.components/CalibrationDialog.vue";

    /**
     * Main component
     */
    @Component({
      components: { CalibrationDialog, AnalysisDialog }
    })
    export default class ToolBar extends Vue {
        @Prop({ required: true })
        public addAnalysis: (data: [number, number][], name?: string) => void;

        /**
         * Show analysis dialog
         */
        showAnalysisDialog() {
            (this.$refs.analysisDialog as AnalysisDialog).showDialog();
        }

        /**
         * Show calibration dialog
         */
        showCalibrationDialog() {
            (this.$refs.calibrationDialog as CalibrationDialog).showDialog();
        }
    }
</script>
