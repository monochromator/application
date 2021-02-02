<template>
  <div>
    <md-dialog :md-active.sync="status" :md-fullscreen="true">
      <md-dialog-content md-dynamic-height>
        <div id="comparison_graph" />
      </md-dialog-content>

      <md-dialog-actions>
        <md-button class="md-primary md-raised" @click="closeDialog">TODO CLOSE</md-button>
      </md-dialog-actions>
    </md-dialog>
  </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";
    import AnalysisMetaData from "@/common/AnalysisMetaData";

    /**
     * Data hold by comparison dialog
     */
    interface ComparisonDialogData {
      status: boolean;
    }

    /**
     * Main component
     */
    @Component({ })
    export default class ComparisonInputDialog extends Vue {
        data(): ComparisonDialogData {
            return {
                status: false
            };
        }

        /**
         * Show dialog
         *
         * @param analyses Analyses to display
         */
        showDialog(analyses: [AnalysisMetaData, [number, number][]][]) {
            this.$data.status = true;
            this.$data.analyses = analyses;

            // TODO: Create graph
        }

        /**
         * Close dialog
         */
        closeDialog() {
            this.$data.status = false;
        }
    }
</script>
