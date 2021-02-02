<template>
  <div>
    <md-dialog :md-active.sync="status" :md-fullscreen="true">
      <md-dialog-content style="max-width: 90vw; width: 75vw" md-dynamic-height>
        <div :id="GRAPH_ID" style="max-width: 100% !important;" />
      </md-dialog-content>

      <md-dialog-actions>
        <md-button class="md-primary md-raised" @click="closeDialog">{{$t("comparison_dialog.close")}}</md-button>
      </md-dialog-actions>
    </md-dialog>
  </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";
    import AnalysisMetaData from "@/common/AnalysisMetaData";
    import * as Plotly from "plotly.js";
    import { createLayout } from "@/common/Graph";

    /**
     * Data hold by comparison dialog
     */
    interface ComparisonDialogData {
      status: boolean;
      analyses: [AnalysisMetaData, [number, number][]][];
    }

    /**
     * Main component
     */
    @Component({ })
    export default class ComparisonInputDialog extends Vue {
        private GRAPH_ID = "comparison_graph";

        data(): ComparisonDialogData {
            return {
                status: false,
                analyses: []
            };
        }

        async updated() {
            try {
                // Reset graph
                await this.resetGraph();

                // Update graph
                await this.updateGraph();
                await Plotly.relayout(this.GRAPH_ID, createLayout(this));
            } catch (e) {
                // Do nothing
            }
        }

        /**
         * Reset graph
         */
        async resetGraph() {
            // Init plot
            await Plotly.react(this.GRAPH_ID, [], createLayout(this), {
                displayModeBar: false,
                responsive: true
            });
            window.dispatchEvent(new Event("resize"));
        }

        /**
         * Update graph data
         */
        async updateGraph() {
          await Plotly.react(this.GRAPH_ID, this.$data.analyses.map(ComparisonInputDialog.toPlotlyData));
        }

        /**
         * Show dialog
         *
         * @param analyses Analyses to display
         */
        showDialog(analyses: [AnalysisMetaData, [number, number][]][]) {
            this.$data.status = true;
            this.$data.analyses = analyses;
        }

        /**
         * Close dialog
         */
        closeDialog() {
            this.$data.status = false;
        }

        /**
         * Convert an analysis to plotly data
         *
         * @param analysis Analysis to convert
         */
        static toPlotlyData(analysis: [AnalysisMetaData, [number, number][]]): Plotly.Data {
            const lineX: number[] = [];
            const lineY: number[] = [];

            analysis[1].forEach((specPoint: [number, number]) => {
              lineX.push(specPoint[0]);
              lineY.push(specPoint[1]);
            });

            return {
              name: analysis[0].name,
              mode: "lines",
              line: { shape: "linear" },
              x: lineX,
              y: lineY
            };
        }
    }
</script>
