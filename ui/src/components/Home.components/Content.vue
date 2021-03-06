<template>
  <div>
    <ToolBar :addAnalysis="addAnalysis" :updateAnalysis="updateAnalysis" :analyses-supplier="() => $data.analyses"/>

    <div style="min-height: 100%; max-height: 100%">
      <md-empty-state md-rounded class="md-accent md-app-content" md-icon="bar_chart" :md-label="$t('home_content.empty_graphs_title')"
                      :md-description="$t('home_content.empty_graphs_description')" v-if="$data.analyses.length === 0">
      </md-empty-state>

      <md-tabs md-alignment="centered" :md-active-tab="$data.activeTab" :md-swipeable="true" v-if="$data.analyses.length > 0">
        <md-tab v-for="analysis in $data.analyses" v-bind:key="analysis[0].id" :id="analysis[0].id" :md-label="analysis[0].name">
          <Spectrum :id="analysis[0].id" :spectrum-data="analysis[1]" :remove-graph="() => removeGraph(analysis[0].id)"></Spectrum>
        </md-tab>
      </md-tabs>
    </div>
  </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";
    import ToolBar from "@/components/Home.components/HomeContent.components/ToolBar.vue";
    import { v4 as uuidv4 } from "uuid";
    import moment from "moment";
    import Spectrum from "@/components/visu/Spectrum.vue";
    import AnalysisMetaData from "@/common/AnalysisMetaData";

    const TIME_FORMAT = "YYYY-MM-DD HH:mm:ss";

    /**
     * Data hold by content component
     */
    interface ContentData {
      analyses: [AnalysisMetaData, [number, number][]][];
      activeTab?: string;
    }

    /**
     * Main component
     */
    @Component({
      components: { Spectrum, ToolBar }
    })
    export default class Content extends Vue {
        data(): ContentData {
            return {
                analyses: [],
                activeTab: undefined
            };
        }

        /**
         * Add a new analysis
         *
         * @param data Analysis' data
         * @param name Analysis' name
         * @return {string} Analysis's id
         */
        addAnalysis(data: [number, number][], name?: string) {
            // Add analysis
            const id = uuidv4();
            this.$data.analyses.push([ {
                name: name === undefined ? moment().format(TIME_FORMAT) : name,
                id: id
            }, data ]);

            // Select the new tab
            this.$data.activeTab = id;
            return id;
        }

        /**
         * Update analysis
         *
         * @param id Analysis's id
         * @param data New analysis data
         */
        updateAnalysis(id: string, data: [number, number][]) {
            const index = this.$data.analyses.findIndex((analysis: [AnalysisMetaData, [number, number][]]) => analysis[0].id === id);

            if (index >= 0) {
                this.$data.analyses[index][1] = data;
                this.$forceUpdate();
            }
        }

        /**
         * Remove a graph with its ID
         *
         * @param id ID
         */
        removeGraph(id: string) {
            this.$data.analyses.splice(this.$data.analyses.findIndex((analysis: [AnalysisMetaData, [number, number][]]) => analysis[0].id === id), 1);

            // Select the first tab
            if (this.$data.analyses.length > 0 && this.$data.activeTab !== this.$data.analyses[0][0].id) {
                this.$data.activeTab = this.$data.analyses[0][0].id;
            }
        }
    }
</script>
