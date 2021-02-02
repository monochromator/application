<template>
  <div>
    <md-dialog :md-active.sync="status">
      <md-dialog-title>{{$t("comparison_dialog.input.title")}}</md-dialog-title>

      <md-dialog-content style="width: 50vw" md-dynamic-height>
        <md-empty-state md-rounded class="md-accent" md-icon="compare_arrows" :md-label="$t('comparison_dialog.input.no_analyses.title')"
                        :md-description="$t('comparison_dialog.input.no_analyses.description')" v-if="$data.analyses.length === 0">
        </md-empty-state>
        <md-checkbox v-else v-for="analysis in $data.analyses" :key="analysis[0].id" v-model="$data.selected" :value="analysis[0].id" style="display: flex">{{analysis[0].name}}</md-checkbox>
      </md-dialog-content>

      <md-dialog-actions>
        <md-button class="md-accent md-raised" @click="selectAll">{{$t("comparison_dialog.input.select_all")}}</md-button>
        <md-button class="md-primary md-raised" :disabled="$data.selected.length <= 1" @click="showComparison">{{$t("comparison_dialog.input.show_button")}}</md-button>
      </md-dialog-actions>
    </md-dialog>

    <AnalysesComparisonDialog ref="comparisonDialog" />
  </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";
    import AnalysisMetaData from "@/common/AnalysisMetaData";
    import AnalysesComparisonDialog
        from "@/components/Home.components/HomeContent.components/Toolbar.components/AnalysesComparisonDialog.vue";

    /**
     * Data hold by comparison input dialog
     */
    interface ComparisonInputDialogData {
      status: boolean;
      analyses: [AnalysisMetaData, [number, number][]][];
      selected: string[];
    }

    /**
     * Main component
     */
    @Component({
      components: { AnalysesComparisonDialog }
    })
    export default class ComparisonInputDialog extends Vue {
        data(): ComparisonInputDialogData {
            return {
                status: false,
                analyses: [],
                selected: []
            };
        }

        /**
         * Show dialog
         *
         * @param analyses Analyses to select
         */
        showDialog(analyses: [AnalysisMetaData, [number, number][]][]) {
            this.$data.selected = [];
            this.$data.status = true;
            this.$data.analyses = analyses;
        }

        /**
         * Show comparison between selected analyses
         */
        showComparison() {
            this.$data.status = false;
            (this.$refs.comparisonDialog as AnalysesComparisonDialog).showDialog(this.$data.analyses.filter((analysis: [AnalysisMetaData, [number, number][]]) => this.$data.selected.includes(analysis[0].id)));
        }

        /**
         * Select all graphs
         */
        selectAll() {
            this.$data.selected = this.$data.analyses.map((analysis: [AnalysisMetaData, [number, number][]]) => analysis[0].id);
        }
    }
</script>

<style scoped>

</style>
