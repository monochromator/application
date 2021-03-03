<template>
  <div>
    <md-dialog :md-active.sync="status">
      <md-dialog-title>{{$t("save_dialog.title")}}</md-dialog-title>

      <md-dialog-content style="width: 50vw" md-dynamic-height>
        <md-field>
          <md-textarea v-model="$data.analysis" :md-autogrow="true" readonly></md-textarea>
        </md-field>

        <div style="display: flex">
          <md-divider style="flex: 1"></md-divider>
          <span style="flex: 1; text-align: center;text-transform: uppercase">{{$t("save_dialog.or")}}</span>
          <md-divider style="flex: 1"></md-divider>
        </div>

        <div style="display: flex; justify-content: center">
          <md-button class="md-button md-raised md-primary" @click="save()"
          >{{ $t("save_dialog.save_btn_title") }}
            <md-icon>save</md-icon></md-button
          >
          <a
              :href="`data:text/csv;charset=utf-8,${encodeURI($data.analysis)}`"
              download="analysis.csv" style="display: none" ref="downloadLink">
          </a>
        </div>
      </md-dialog-content>
    </md-dialog>
  </div>
</template>

<script lang="ts">
    import { Component, Vue } from "vue-property-decorator";
    import { toCSV } from "@/services/CSVService";

    /**
     * Data hold by save dialog
     */
    interface SaveDialogData {
      status: boolean;
      analysis: string;
    }

    /**
     * Main component
     */
    @Component
    export default class SaveDialog extends Vue {
        data(): SaveDialogData {
            return {
                status: false,
                analysis: ""
            };
        }

        /**
         * Show dialog
         *
         * @param analysis Analysis to save
         */
        showDialog(analysis: [number, number][]) {
            this.$data.status = true;
            this.$data.analysis = toCSV(analysis);
        }

        /**
         * Save analysis
         */
        save() {
            (this.$refs.downloadLink as HTMLAnchorElement).click();
        }
    }
</script>
