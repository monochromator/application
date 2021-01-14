<template>
  <div class="md-toolbar-row">
    <span style="flex: 1"></span>

    <md-button class="md-button md-raised md-accent">TODO CALIBRATION</md-button>
    <md-button class="md-button md-raised md-accent" @click="showAnalysisDialog">{{ $t("home_content_toolbar.analysis_button") }}
    </md-button>

    <AnalysisDialog ref="analysisDialog" :addAnalysis="addAnalysis"/>
  </div>
</template>

<script lang="ts">
    import { Component, Prop, Vue } from "vue-property-decorator";
    import AnalysisDialog from "@/components/Home.components/HomeContent.components/Toolbar.components/AnalysisDialog.vue";

    /**
     * Main component
     */
    @Component({
      components: { AnalysisDialog }
    })
    export default class ToolBar extends Vue {
        @Prop({ required: true })
        public addAnalysis: (data: [number, number][]) => void;

        /**
         * Show analysis dialog
         */
        showAnalysisDialog() {
            (this.$refs.analysisDialog as AnalysisDialog).showDialog();
        }
    }
</script>
